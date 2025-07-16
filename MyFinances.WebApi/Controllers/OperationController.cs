using Microsoft.AspNetCore.Mvc;
using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using MyFinances.WebApi.Models;
using MyFinances.WebApi.Models.Converters;
using MyFinances.WebApi.Models.Domains;
using MyFinances.WebApi.Services;
using System;
using System.Collections.Generic;

namespace MyFinances.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        readonly UnitOfWork _unitOfWork;
        readonly OperationService _operationService;
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public OperationController(MyFinancesContext context)
        {
            _unitOfWork = new UnitOfWork(context);
            _operationService = new OperationService(_unitOfWork);
        }

        /// <summary>
        /// Get all operations.
        /// </summary>
        /// <returns>A DataResponse containing a list of OperationDto objects.</returns>
        [HttpGet]
        public DataResponse<IEnumerable<OperationDto>> Get()
        {
            var response = new DataResponse<IEnumerable<OperationDto>>();

            try
            {
                response.Data = _operationService.GetAllOperations();
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));

                _logger.Error("StackTrace", exception.StackTrace);

                if (exception.InnerException != null)
                {
                    _logger.Error("InnerException", exception.InnerException.Message);
                    _logger.Error("InnerException StackTrace", exception.InnerException.StackTrace);
                }
            }

            return response;
        }

        /// <summary>
        /// Get operation by id.
        /// </summary>
        /// <param name="id">Operation Id.</param>
        /// <returns>A DataResponse containing the operation details.</returns>
        [HttpGet("{id}")]
        public DataResponse<OperationDto> Get(int id)
        {
            var response = new DataResponse<OperationDto>();

            try
            {
                response.Data = _operationService.GetOperationById(id)?.ToDto();
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));

                _logger.Error("StackTrace", exception.StackTrace);

                if (exception.InnerException != null)
                {
                    _logger.Error("InnerException", exception.InnerException.Message);
                    _logger.Error("InnerException StackTrace", exception.InnerException.StackTrace);
                }
            }

            return response;
        }

        /// <summary>
        /// Get a paginated list of operations.
        /// </summary>
        /// <remarks>
        /// Example:<br/>
        /// operationCount: 10, pageNumber: 1 -> operations 1-10<br/>
        /// operationCount: 10, pageNumber: 2 -> operations 11-20<br/>
        /// etc.<br/><br/>
        /// Example of URL for this action:<br/>
        /// https://localhost:44369/api/operation/10/1
        /// </remarks>
        /// <param name="operationCount">The number of operations to get per page.</param>
        /// <param name="pageNumber">The page number to get.</param>
        /// <returns>A DataResponse containing a paginated list of OperationDto objects.</returns>
        [HttpGet("{operationCount}/{pageNumber}")]
        public DataResponse<PagedResponse<OperationDto>> Get(byte operationCount = 10, int pageNumber = 1)
        {
            var response = new DataResponse<PagedResponse<OperationDto>>();

            try
            {
                response.Data = _operationService.GetPagedOperations(operationCount, pageNumber);
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));

                _logger.Error("StackTrace", exception.StackTrace);

                if (exception.InnerException != null)
                {
                    _logger.Error("InnerException", exception.InnerException.Message);
                    _logger.Error("InnerException StackTrace", exception.InnerException.StackTrace);
                }
            }

            return response;
        }

        /// <summary>
        /// Get total count of operations.
        /// </summary>
        /// <returns>A DataResponse containing the total number of operations.</returns>
        [HttpGet("count")]
        public DataResponse<int> GetTotalCount()
        {
            var response = new DataResponse<int>();

            try
            {
                response.Data = _operationService.GetTotalOperationCount();
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));

                _logger.Error("StackTrace", exception.StackTrace);

                if (exception.InnerException != null)
                {
                    _logger.Error("InnerException", exception.InnerException.Message);
                    _logger.Error("InnerException StackTrace", exception.InnerException.StackTrace);
                }
            }

            return response;
        }

        /// <summary>
        /// Add a new operation.
        /// </summary>
        /// <param name="operationDto">The operation data transfer object containing details of the operation to add.</param>
        /// <returns>A DataResponse containing the ID of the newly added operation.</returns>
        [HttpPost]
        public DataResponse<int> Add(OperationCreateDto operationDto)
        {
            var response = new DataResponse<int>();

            try
            {
                var operation = operationDto.ToDomain();
                _operationService.AddOperation(operation);
                response.Data = operation.Id;
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));

                _logger.Error("StackTrace", exception.StackTrace);

                if (exception.InnerException != null)
                {
                    _logger.Error("InnerException", exception.InnerException.Message);
                    _logger.Error("InnerException StackTrace", exception.InnerException.StackTrace);
                }
            }

            return response;
        }

        /// <summary>
        /// Update an existing operation.
        /// </summary>
        /// <param name="operation">The operation data transfer object containing updated details of the operation.</param>
        /// <returns>A Response indicating the success or failure of the update operation.</returns>
        [HttpPut]
        public Response Update(OperationDto operation)
        {
            var response = new Response();

            try
            {
                _operationService.UpdateOperation(operation.ToDomain());
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));

                _logger.Error("StackTrace", exception.StackTrace);

                if (exception.InnerException != null)
                {
                    _logger.Error("InnerException", exception.InnerException.Message);
                    _logger.Error("InnerException StackTrace", exception.InnerException.StackTrace);
                }
            }

            return response;
        }

        /// <summary>
        /// Delete an operation by its ID.
        /// </summary>
        /// <param name="id">The ID of the operation to delete.</param>
        /// <returns>A Response indicating the success or failure of the delete operation.</returns>
        [HttpDelete("{id}")]
        public Response Delete(int id)
        {
            var response = new Response();

            try
            {
                _operationService.DeleteOperation(id);
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));

                _logger.Error("StackTrace", exception.StackTrace);

                if (exception.InnerException != null)
                {
                    _logger.Error("InnerException", exception.InnerException.Message);
                    _logger.Error("InnerException StackTrace", exception.InnerException.StackTrace);
                }
            }

            return response;
        }
    }
}
