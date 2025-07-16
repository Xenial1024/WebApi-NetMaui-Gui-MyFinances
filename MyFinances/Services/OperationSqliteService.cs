using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using MyFinances.Models;
using MyFinances.Models.Converters;
using MyFinances.Models.Domains;

namespace MyFinances.Services
{
    public class OperationSqliteService : IOperationService
    {
        static UnitOfWork _unitOfWork;
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static UnitOfWork UnitOfWork
        {
            get
            {
                _unitOfWork ??= new UnitOfWork(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyFinancesSQLite.db3"));

                return _unitOfWork;
            }
        }

        public async Task<DataResponse<int>> AddAsync(OperationDto operation)
        {
            var response = new DataResponse<int>();
            try
            {
                response.Data = await UnitOfWork
                    .Operation.AddAsync(operation.ToDomain());
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
                _logger.Error("Błąd podczas dodawania operacji do SQLite: " + exception);
            }
            return response;
        }

        public async Task<Response> UpdateAsync(OperationDto operation)
        {
            var response = new Response();
            try
            {
                await UnitOfWork
                    .Operation.UpdateAsync(operation.ToDomain());
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
                _logger.Error("Błąd podczas aktualizacji operacji w SQLite: " + exception);
            }
            return response;
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var response = new Response();
            try
            {
                await UnitOfWork
                    .Operation.DeleteAsync(new Operations { Id = id });
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
                _logger.Error("Błąd podczas usuwania operacji z SQLite: " + exception);
            }
            return response;
        }

        public async Task<DataResponse<IEnumerable<OperationDto>>> GetAsync()
        {
            var response = new DataResponse<IEnumerable<OperationDto>>();
            try
            {
                response.Data = (await UnitOfWork
                    .Operation.GetAsync()).ToDtos();
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
                _logger.Error("Błąd podczas pobierania operacji z SQLite: " + exception);
            }
            return response;
        }

        public async Task<DataResponse<OperationDto>> GetAsync(int id)
        {
            var response = new DataResponse<OperationDto>();
            try
            {
                response.Data = (await UnitOfWork
                    .Operation.GetAsync(id)).ToDto();
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
                _logger.Error("Błąd podczas pobierania operacji z SQLite: " + exception);
            }
            return response;
        }

        public async Task<DataResponse<PagedResponse<OperationDto>>> GetPagedAsync(byte operationCount, int pageNumber)
        {
            try
            {
                var items = await UnitOfWork.Operation.GetAsync(operationCount, pageNumber);
                var totalCount = await UnitOfWork.Operation.GetTotalCountAsync();

                return new DataResponse<PagedResponse<OperationDto>>
                {
                    Data = new PagedResponse<OperationDto>
                    {
                        Items = [.. items.ToDtos()],
                        TotalCount = totalCount
                    }
                };
            }
            catch (Exception exception)
            {
                _logger.Error("Błąd podczas ładowania danych z SQLite", exception);
                return new DataResponse<PagedResponse<OperationDto>>
                {
                    Errors = [new Error(exception.Source, exception.Message)]
                };
            }
        }

        public async Task<DataResponse<int>> GetTotalCountAsync()
        {
            var response = new DataResponse<int>();
            try
            {
                response.Data = await UnitOfWork.Operation.GetTotalCountAsync();
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
                _logger.Error("Błąd podczas pobierania liczby operacji z SQLite: " + exception);
            }
            return response;
        }
    }
}
