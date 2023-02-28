using Bogus;
using Customers.Api.Contracts.Data;
using Customers.Api.Repositories;

namespace template.integration.tests.Controllers;

public class CustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly CustomerApiFactory _customerApiFactory;
    private readonly ICustomerRepository _customerRepository;

    private Faker<CustomerDto> _customerFaker = new Faker<CustomerDto>()
        .RuleFor(x => x.Id, faker => faker.Random.Guid())
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth);

    public CustomerControllerTests(CustomerApiFactory customerApiFactory, ICustomerRepository customerRepository)
    {
        _customerApiFactory = customerApiFactory;
        _customerRepository = customerRepository;
    }

    [Fact]
    public async Task Create_User_Should_Be_Ok()
    {
        var customer = _customerFaker.Generate();
        
        await _customerRepository.CreateAsync(customer);
    }
}