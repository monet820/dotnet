using Customers.Api.Contracts.Responses;

namespace template.integration.tests.Controllers;

public class GetCustomerControllerTests : TestBase
{
    public GetCustomerControllerTests(CustomerApiFactory customerApiFactory) : base(customerApiFactory)
    {
        
    }

    [Fact]
    public async Task Get_Return()
    {
        // ARRANGE
        var customer = CustomerGenerator.Generate();
        var createdResponse = await Client.PostAsJsonAsync("Customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        
        // ACT
        var response = await Client.GetAsync($"customers/{createdCustomer.Id}");

        // ASSERT
        var retrievedCustomer = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        retrievedCustomer.Should().BeEquivalentTo(createdCustomer);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_Returns_NotFound_WhenCustomerDoesNotExist()
    {
        // ACT
        var response = await Client.GetAsync($"customers/{Guid.NewGuid()}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}