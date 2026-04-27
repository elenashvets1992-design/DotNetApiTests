using System.Net.Http;
using System.Text.Json;
using ApiTests.Utils;
using System.Text;
using System.Net;

namespace ApiTests.Clients;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

//Get positive
    public async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        var json = await response.Content.ReadAsStringAsync();

        ApiLogger.LogRequest("GET", endpoint);
        ApiLogger.LogResponse(json, (int)response.StatusCode);

        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<T>(json)!;
    }
  
// GET negative
    public async Task<HttpResponseMessage> GetRawAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);

        var json = await response.Content.ReadAsStringAsync();

        ApiLogger.LogRequest("GET", endpoint);
        ApiLogger.LogResponse(json, (int)response.StatusCode);

        return response;
    }

// POST
    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object body)
    {
        var jsonBody = JsonSerializer.Serialize(body);

        var response = await _httpClient.PostAsync(
            endpoint,
            new StringContent(jsonBody, Encoding.UTF8, "application/json")
        );

        var json = await response.Content.ReadAsStringAsync();
        ApiLogger.LogRequest("POST", endpoint);
        ApiLogger.LogResponse(json, (int)response.StatusCode);
        var data = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return new ApiResponse<T>
        {
            Data = data!,
            StatusCode = response.StatusCode
        };
    }

// PUT
    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object body)
    {
        var jsonBody = JsonSerializer.Serialize(body);

        var response = await _httpClient.PutAsync(
            endpoint,
            new StringContent(jsonBody, Encoding.UTF8, "application/json")
        );

        var json = await response.Content.ReadAsStringAsync();

        ApiLogger.LogRequest("PUT", endpoint, body);
        ApiLogger.LogResponse(json, (int)response.StatusCode);
        var data = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return new ApiResponse<T>
        {
            Data = data!,
            StatusCode = response.StatusCode
        };
    }

// DELETE
    public async Task<ApiResponse<object>> DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);

        var json = await response.Content.ReadAsStringAsync();
        ApiLogger.LogRequest("DELETE", endpoint);
        ApiLogger.LogResponse(json, (int)response.StatusCode);
        return new ApiResponse<object>
        {
            Data = json,
            StatusCode = response.StatusCode
        };
    }

    
}