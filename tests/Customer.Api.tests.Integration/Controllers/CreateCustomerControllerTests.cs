using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace template.integration.tests.Controllers;

// [Collection(nameof(CustomerApiFactoryTestCollection))]
public class CreateCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly CustomerApiFactory _customerApiFactory;
    private readonly HttpClient _client;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    public CreateCustomerControllerTests(CustomerApiFactory customerApiFactory)
    {
        _customerApiFactory = customerApiFactory;
        _client = customerApiFactory.CreateClient();
    }

    [Fact]
    public async Task Create_Creates_User_WhenDataIsValid()
    {
        // ARRANGE
        var customer = _customerGenerator.Generate();
        
        // ACT
        var response = await _client.PostAsJsonAsync("/customers", customer);
        
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
        var customer = _customerGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail)
            .Generate();
        
        // ACT
        var response = await _client.PostAsJsonAsync("/customers", customer);
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Email"][0].Should().Be($"{invalidEmail} is not a valid email address");
    }
}