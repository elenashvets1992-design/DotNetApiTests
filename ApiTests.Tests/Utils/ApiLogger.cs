using System.Text.Json;

namespace ApiTests.Utils;

public static class ApiLogger
{
    // Logs HTTP request body
    public static void LogRequest(string method, string endpoint, object? body = null)
    {
        TestContext.Out.WriteLine($"REQUEST: {method} {endpoint}");

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            TestContext.Out.WriteLine($"REQUEST BODY:\n{json}");
        }
    }

    // Logs HTTP response status code and response body
    public static void LogResponse(string json, int statusCode)
    {
        TestContext.Out.WriteLine($"STATUS CODE: {statusCode}");
        TestContext.Out.WriteLine($"RESPONSE BODY:\n{json}");
    }
}