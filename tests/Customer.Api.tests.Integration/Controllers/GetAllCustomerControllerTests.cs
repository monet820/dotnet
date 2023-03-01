using Customers.Api.Contracts.Responses;

namespace template.integration.tests.Controllers;

public class GetAllCustomerControllerTests : TestBase
{
    public GetAllCustomerControllerTests(CustomerApiFactory customerApiFactory) : base(customerApiFactory)
    {
        
    }
    
    [Fact]
    public async Task GetAll_Returns_All_Customers_When_Exists()
    {
        // ARRANGE
        var customer = CustomerGenerator.Generate();
        var createdResponse = await Client.PostAsJsonAsync("Customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        
        // ACT
        var response = await Client.GetAsync($"customers/");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedCustomers = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        retrievedCustomers!.Customers.Single().Should().BeEquivalentTo(createdCustomer);
        
        await Client.DeleteAsync($"customers/{createdCustomer!.Id}");
    }
    
    [Fact]
    public async Task GetAll_Returns_Empty_List_When_No_Customers_Exist()
    {
        // ACT
        var response = await Client.GetAsync($"customers/");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedCustomers = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        retrievedCustomers!.Customers.Should().BeEmpty();
    }
}