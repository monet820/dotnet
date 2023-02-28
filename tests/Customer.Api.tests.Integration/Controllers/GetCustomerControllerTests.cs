using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;

namespace template.integration.tests.Controllers;

// [Collection(nameof(CustomerApiFactoryTestCollection))]
public class GetCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _client;
    
    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);
    
    public GetCustomerControllerTests(CustomerApiFactory customerApiFactory)
    {
        _client = customerApiFactory.CreateClient();
    }

    [Fact]
    public async Task Get_Return()
    {
        // ARRANGE
        var customer = _customerGenerator.Generate();
        var createdResponse = await _client.PostAsJsonAsync("Customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        
        // ACT
        var response = await _client.GetAsync($"customers/{createdCustomer.Id}");

        // ASSERT
        var retrievedCustomer = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        retrievedCustomer.Should().BeEquivalentTo(createdCustomer);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_Returns_NotFound_WhenCustomerDoesNotExist()
    {
        // ACT
        var response = await _client.GetAsync($"customers/{Guid.NewGuid()}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}