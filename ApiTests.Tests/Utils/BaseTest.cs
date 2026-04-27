using ApiTests.Clients;
using ApiTests.Config;

namespace ApiTests.Utils;

public class BaseTest
{
    protected ApiClient client;

    [SetUp]
    public void Setup()
    {
        client = new ApiClient(new HttpClient
        {
            BaseAddress = new Uri(AppSettings.BaseUrl)
        });
    }
}