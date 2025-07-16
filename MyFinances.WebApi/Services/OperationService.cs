using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using MyFinances.WebApi.Models;
using MyFinances.WebApi.Models.Converters;
using MyFinances.WebApi.Models.Domains;
using MyFinances.WebApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyFinances.WebApi.Services
{
    public class OperationService(UnitOfWork unitOfWork)
    {
        readonly UnitOfWork _unitOfWork = unitOfWork;
        readonly OperationRepository _operationRepository = unitOfWork.Operation;

        public IEnumerable<OperationDto> GetAllOperations() => _operationRepository.Get().ToDtos();

        public PagedResponse<OperationDto> GetPagedOperations(byte operationCount, int pageNumber)
        {
            //Walidacja czy operationCount nie jest ujemna nie jest potrzebna, bo i tak typ byte nie pozwala na ujemne wartości.
            if (operationCount == 0)
                throw new ValidationException("Liczba operacji musi być dodatnia.");

            if (pageNumber <= 0)
                throw new ValidationException("Numer strony musi być dodatni.");

            int total = _operationRepository.Get().Count(); // Pobranie całkowitej liczby operacji
            var items = _operationRepository.Get(operationCount, pageNumber).ToDtos();

            return new PagedResponse<OperationDto>
            {
                Items = items,
                TotalCount = total
            };
        }

        public Operations GetOperationById (int id) => _operationRepository.Get(id);

        public void AddOperation(Operations operation)
        {
            bool categoryExists = _unitOfWork.Operation.CategoryExists(operation.CategoryId);
            if (!categoryExists)
                throw new ArgumentException($"Kategoria o ID {operation.CategoryId} nie istnieje.");

            if (operation.Value <= 0)
                throw new ValidationException("Kwota musi być większa od 0."); 
            
            _unitOfWork.Operation.Add(operation);
            _unitOfWork.Complete();
        }
        public void UpdateOperation(Operations operation)
        {
            bool categoryExists = _unitOfWork.Operation.CategoryExists(operation.CategoryId);
            if (!categoryExists)
                throw new ArgumentException($"Kategoria o ID {operation.CategoryId} nie istnieje."); 
            
            if (operation.Value <= 0)
                throw new ValidationException("Kwota musi być większa od 0.");

            _unitOfWork.Operation.Update(operation);
            _unitOfWork.Complete();
        }
        public void DeleteOperation(int id)
        {
            _unitOfWork.Operation.Delete(id);
            _unitOfWork.Complete();
        }
        public int GetTotalOperationCount() => _unitOfWork.Operation.GetTotalCount();
    }
}
