using Customers.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace template.integration.tests.Controllers;

public class CreateCustomerControllerTests : TestBase
{
    public CreateCustomerControllerTests(CustomerApiFactory customerApiFactory) : base(customerApiFactory)
    {
        
    }

    [Fact]
    public async Task Create_Creates_User_WhenDataIsValid()
    {
        // ARRANGE
        var customer = CustomerGenerator.Generate();
        
        // ACT
        var response = await Client.PostAsJsonAsync("/customers", customer);
        
        // ASSERT
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be($"http://localhost/customers/{customerResponse!.Id}");
        customerResponse.Should().BeEquivalentTo(customer);
    }
    
    [Fact]
    public async Task Create_Returns_ValidationError_When_Email_Is_Invalid()
    {
        // ARRANGE
        const string invalidEmail = "clearlyNotValidEmail";
        var customer = CustomerGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail)
            .Generate();
        
        // ACT
        var response = await Client.PostAsJsonAsync("/customers", customer);
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Email"][0].Should().Be($"{invalidEmail} is not a valid email address");
    }
}