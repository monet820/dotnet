using Customers.Api.Contracts.Requests;
using Bogus;

namespace template.integration.tests;

[Collection(nameof(CustomerApiFactoryTestCollection))]
public class TestBase : IAsyncLifetime
{
    protected readonly HttpClient Client;

    private readonly Func<Task> _resetDatabase;

    // Bogus
    protected readonly Faker<CustomerRequest> CustomerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    protected TestBase(CustomerApiFactory customerApiFactory)
    {
        Client = customerApiFactory.HttpClient;
        _resetDatabase = customerApiFactory.ResetDatabase;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}