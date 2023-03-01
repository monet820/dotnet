using Customers.Api.Contracts.Requests;
using Bogus;

namespace template.integration.tests;

[Collection(nameof(CustomerApiFactoryTestCollection))]
public class TestBase
{
    protected readonly HttpClient Client;
    
    // Bogus
    protected readonly Faker<CustomerRequest> CustomerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    protected TestBase(CustomerApiFactory customerApiFactory)
    {
        Client = customerApiFactory.CreateClient();
        
        // This constructor is called before any test.
        // Database cleanup
    }
}