using MyFinances.WebApi.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFinances.WebApi.Models.Repositories
{
    public class OperationRepository(MyFinancesContext context)
    {
        private readonly MyFinancesContext _context = context;

        public IEnumerable<Operations> Get() => _context.Operations;

        public Operations Get(int id) => _context.Operations.FirstOrDefault(x => x.Id == id);

        public IEnumerable<Operations> Get(byte operationCount, int pageNumber)
            => _context.Operations
                    .Skip((pageNumber - 1) * operationCount)
                    .Take(operationCount);
                    
        public void Add(Operations operation)
        {
            operation.Date = DateTime.Today;
            _context.Operations.Add(operation);
        }

        public void Update(Operations operation)
        {
            var operationToUpdate = _context.Operations
                .First(x => x.Id == operation.Id);

            operationToUpdate.CategoryId = operation.CategoryId;
            operationToUpdate.Description = operation.Description;
            operationToUpdate.Name = operation.Name;
            operationToUpdate.Value = operation.Value;
            operationToUpdate.Date = operation.Date;
        }

        public void Delete(int id)
        {
            var operationToDelete = _context.Operations
                .First(x => x.Id == id);

            _context.Operations.Remove(operationToDelete);
        }

        public bool CategoryExists(int categoryId)
            => _context.Categories.Any(c => c.Id == categoryId);

        public int GetTotalCount() 
            => _context.Operations.Count();

    }
}
