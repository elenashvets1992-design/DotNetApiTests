using System.Net;

namespace ApiTests.Clients;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}