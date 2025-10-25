using System.Net;
using FluentAssertions;

namespace Api.IntegrationTests;

public class UnitTest1: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UnitTest1(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Test1()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/v1/Auth");

        string responseString = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

    }
}
