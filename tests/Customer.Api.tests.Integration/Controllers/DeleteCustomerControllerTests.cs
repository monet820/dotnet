using Customers.Api.Contracts.Responses;

namespace template.integration.tests.Controllers;

public class DeleteCustomerControllerTests  : TestBase
{
    public DeleteCustomerControllerTests(CustomerApiFactory apiFactory) : base(apiFactory)
    {
        
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenCustomerExists()
    {
        // Arrange
        var customer = CustomerGenerator.Generate();
        var createdResponse = await Client.PostAsJsonAsync("customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Act
        var response = await Client.DeleteAsync($"customers/{createdCustomer!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Act
        var response = await Client.DeleteAsync($"customers/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}