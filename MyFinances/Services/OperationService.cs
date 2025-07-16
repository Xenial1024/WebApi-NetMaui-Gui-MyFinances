using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using Newtonsoft.Json;
using System.Text;
using MyFinances.Models.Domains;

namespace MyFinances.Services
{
    public class OperationService : IOperationService
    {
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        readonly HttpClient _httpClient;
        public OperationService()
        {
            _httpClient = new()
            {
                BaseAddress = new Uri(Settings.ApiAddress)
            };
        }

        public async Task<DataResponse<int>> AddAsync(OperationDto operation)
        {
            try
            {
                var stringContent = new StringContent(
                    JsonConvert.SerializeObject(operation),
                    Encoding.UTF8,
                    "application/json");

                using var response =
                    await _httpClient.PostAsync("operation", stringContent);
                var responseContent =
                    await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<DataResponse<int>>(responseContent);
            }
            catch (Exception exception)
            {
                _logger.Error("Błąd podczas dodawania operacji do SQL Server: " + exception);
                return new DataResponse<int>
                {
                    Errors =
                    [
                        new Error(exception.Source, exception.Message)
                    ]
                };
            }
        }

        public async Task<Response> UpdateAsync(OperationDto operation)
        {
            try
            {
                var stringContent = new StringContent(
                JsonConvert.SerializeObject(operation),
                Encoding.UTF8,
                "application/json");

                using var response =
                    await _httpClient.PutAsync("operation", stringContent);
                var responseContent =
                    await response.Content.ReadAsStringAsync();

                return JsonConvert
                    .DeserializeObject<Response>(responseContent);
            }
            catch (Exception exception)
            {
                _logger.Error("Błąd podczas edycji operacji w SQL Server: " + exception);
                return new Response
                {
                    Errors =
                    [
                        new Error(exception.Source, exception.Message)
                    ]
                };
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                using var response =
                    await _httpClient.DeleteAsync($"operation/{id}");
                var responseContent =
                    await response.Content.ReadAsStringAsync();

                return JsonConvert
                    .DeserializeObject<Response>(responseContent);
            }
            catch (Exception exception)
            {
                _logger.Error("Błąd podczas usuwania operacji z SQL Server: " + exception);
                return new Response
                {
                    Errors =
                    [
                        new Error(exception.Source, exception.Message)
                    ]
                };
            }
        }

        public async Task<DataResponse<IEnumerable<OperationDto>>> GetAsync()
        {
            try
            {
                var json = await _httpClient.GetStringAsync($"operation");

                return JsonConvert.DeserializeObject
                    <DataResponse<IEnumerable<OperationDto>>>(json);
            }
            catch (Exception exception)
            {
                _logger.Error("Błąd podczas pobierania operacji z SQL Server: " + exception);
                return new DataResponse<IEnumerable<OperationDto>>
                {
                    Errors =
                    [
                        new Error(exception.Source, exception.Message)
                    ]
                };
            }
        }
        public async Task<DataResponse<OperationDto>> GetAsync(int id)
        {
            try
            {
                var json = await _httpClient.GetStringAsync($"operation/{id}");

                return JsonConvert
                    .DeserializeObject<DataResponse<OperationDto>>(json);
            }
            catch (Exception exception)
            {
                _logger.Error("Błąd podczas pobierania operacji z SQL Server: " + exception);
                return new DataResponse<OperationDto>
                {
                    Errors =
                    [
                        new Error(exception.Source, exception.Message)
                    ]
                };
            }
        }

        public async Task<DataResponse<PagedResponse<OperationDto>>> GetPagedAsync(byte operationCount, int pageNumber)
        {
            try
            {
                var json = await _httpClient.GetStringAsync($"operation/{operationCount}/{pageNumber}");
                var result = JsonConvert.DeserializeObject<DataResponse<PagedResponse<OperationDto>>>(json);

                if (result.IsSuccess)
                {
                    var countResponse = await GetTotalCountAsync();

                    if (countResponse.IsSuccess)
                        return result;
                    else
                        return new DataResponse<PagedResponse<OperationDto>>
                        {
                            Errors = countResponse.Errors
                        };
                }
                else
                    return new DataResponse<PagedResponse<OperationDto>>
                    {
                        Errors = result.Errors
                    };
            }
            catch (Exception exception)
            {
                _logger.Error("Błąd podczas pobierania operacji z SQL Server: " + exception);
                return new DataResponse<PagedResponse<OperationDto>>
                {
                    Errors =
                    [
                        new Error(exception.Source, exception.Message)
                    ]
                };
            }
        }

        public async Task<DataResponse<int>> GetTotalCountAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("operation/count");
                response.EnsureSuccessStatusCode(); 
                var json = await response.Content.ReadAsStringAsync();
                _logger.Debug($"Otrzymany JSON z API: {json}");

                return JsonConvert.DeserializeObject<DataResponse<int>>(json);

            }
            catch (Exception exception)
            {
                _logger.Error("Błąd podczas pobierania liczby operacji z SQL Server: " + exception);
                return new DataResponse<int>
                {
                    Errors = [new Error(exception.Source, exception.Message)]
                };
            }
        }
    }
}