using MyFinances.Core.Dtos;
using MyFinances.Core.Response;

namespace MyFinances.Services
{
    public interface IOperationService
    {
        Task<DataResponse<int>> AddAsync(OperationDto operation);
        Task<Response> UpdateAsync(OperationDto itoperationem);
        Task<Response> DeleteAsync(int id);
        Task<DataResponse<IEnumerable<OperationDto>>> GetAsync();
        Task<DataResponse<OperationDto>> GetAsync(int id);
        Task<DataResponse<PagedResponse<OperationDto>>> GetPagedAsync(byte operationCount, int pageNumber);
        Task<DataResponse<int>> GetTotalCountAsync();
    }
}
