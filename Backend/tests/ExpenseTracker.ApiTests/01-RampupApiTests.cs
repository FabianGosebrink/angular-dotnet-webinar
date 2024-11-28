using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace ExpenseTracker.ApiTests;

public class RampupApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public RampupApiTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
    }
    
    /* Task 1: Implement Greeting
       Create an endpoint that accepts a 'name' query parameter
       Expected response: A Json with the following structure when "Steven" is passed in:
       {
         "message": "Hello, Steven!"
        }
       Endpoint: GET /api/greeting?name=Steven
    */
    [Theory]
    [InlineData("Steven", "Hello, Steven!")]
    [InlineData("Fabian", "Hello, Fabian!")]
    public async Task ShouldGreetUser(string name, string expectedGreeting)
    {
        using var client = _webApplicationFactory.CreateClient();
        
        using var response = await client.GetAsync($"/api/greeting?name={name}");
        
        response.EnsureSuccessStatusCode();
        var greeting = await response.Content.ReadFromJsonAsync<GreetingResponse>();
        greeting.ShouldNotBeNull();
        greeting.Message.ShouldBe(expectedGreeting);
    }
    
    /* Task 2: Implement Message Update
       Create an endpoint to update existing messages
       Expected behavior: Allow updating message text
       Endpoint: PUT /api/messages/{id}
    */
    [Fact]
    public async Task ShouldStoreAndRetrieveMessages()
    {
        using var client = _webApplicationFactory.CreateClient();

        using var postResponse = await client.PostAsJsonAsync("/api/messages", 
            new CreateMessageRequest { Text = "Hello World!" });
        postResponse.EnsureSuccessStatusCode();
        
        await client.PostAsJsonAsync("/api/messages", 
            new CreateMessageRequest { Text = "Second message" });
        
        using var getResponse = await client.GetAsync("/api/messages");
        getResponse.EnsureSuccessStatusCode();
    
        var messages = await getResponse.Content
            .ReadFromJsonAsync<List<MessageResponse>>();
    
        messages.ShouldNotBeNull();
        messages.Count.ShouldBe(2);
        messages[0].Text.ShouldBe("Hello World!");
        messages[1].Text.ShouldBe("Second message");
    }
    
    /* Task 3: Middleware I
       Create a middleware that adds a "Correlation-Id" header to the response
       More information here: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0#create-a-middleware-pipeline-with-webapplication
    */
    [Fact]
    public async Task ShouldAddCorrelationId()
    {
        using var client = _webApplicationFactory.CreateClient();
        var url = Guid.NewGuid().ToString();
        
        // Purposefully don't use an existing URL
        using var response = await client.GetAsync(url);

        var header = response.Headers.FirstOrDefault(h => h.Key == "Correlation-Id");
        header.Value.FirstOrDefault().ShouldNotBeNull().ShouldNotBeEmpty();
        
        // Use a pre-existing URL to check if next was called
        using var responseExistingUrl = await client.GetAsync($"/api/greeting?name=Fabian");
        
        responseExistingUrl.EnsureSuccessStatusCode();
        header = response.Headers.FirstOrDefault(h => h.Key == "Correlation-Id");
        header.Value.FirstOrDefault().ShouldNotBeNull().ShouldNotBeEmpty();
        var greeting = await responseExistingUrl.Content.ReadFromJsonAsync<GreetingResponse>();
        greeting.ShouldNotBeNull();
        greeting.Message.ShouldBe("Hello, Fabian!");
    }

    /* Task 4: Middleware II
       If there is already a "Correlation-Id" present in the request, use this!
    */
    [Fact]
    public async Task GivenCorrelationIdPresentInRequestHeader_ThenNoNewCorrelationIdIsReturned()
    {
        using var client = _webApplicationFactory.CreateClient();
        var correlationId = Guid.NewGuid().ToString();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/greeting?name=Fabian");
        request.Headers.Add("Correlation-Id", correlationId);
        
        using var response = await client.SendAsync(request);
        
        response.EnsureSuccessStatusCode();
        var header = response.Headers.FirstOrDefault(h => h.Key == "Correlation-Id");
        header.Value.FirstOrDefault().ShouldBe(correlationId);
    }

    private sealed class GreetingResponse
    {
        public required string Message { get; init; }
    }
    
    private sealed class CreateMessageRequest
    {
        public required string Text { get; init; }
    }

    private sealed class MessageResponse
    {
        public required string Text { get; init; }
    }
}