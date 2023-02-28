using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;

namespace template.integration.tests.Controllers;

// [Collection(nameof(CustomerApiFactoryTestCollection))]
public class GetAllCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _client;
    
    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);
    
    public GetAllCustomerControllerTests(CustomerApiFactory customerApiFactory)
    {
        _client = customerApiFactory.CreateClient();
    }
    
    [Fact]
    public async Task GetAll_Returns_All_Customers_When_Exists()
    {
        // ARRANGE
        var customer = _customerGenerator.Generate();
        var createdResponse = await _client.PostAsJsonAsync("Customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        
        // ACT
        var response = await _client.GetAsync($"customers/");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedCustomers = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        retrievedCustomers!.Customers.Single().Should().BeEquivalentTo(createdCustomer);
        
        // Tests share the data, we need to clear it between tests..
        // Alternative 1.
        // await _client.DeleteAsync($"customers/{createdCustomer!.Id}");
        // Alternative 2. -> new docker instance -> new class with IClassFixture<CustomerApiFactory>
        // Keep it separate from other databases.
    }
    
    [Fact]
    public async Task GetAll_Returns_Empty_List_When_No_Customers_Exist()
    {
        // ACT
        var response = await _client.GetAsync($"customers/");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedCustomers = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        retrievedCustomers!.Customers.Should().BeEmpty();
    }
}