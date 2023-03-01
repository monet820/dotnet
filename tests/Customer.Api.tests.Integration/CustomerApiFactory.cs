using Customers.Api;
using Customers.Api.Database;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace template.integration.tests;

public class CustomerApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    // can be any image! 
    
    // Collection!
    // private int port = Random.Shared.next(10000,60000); kinda hacky..
    
    // private readonly DockerContainer _postgresDatabaseContainer = new ContainerBuilder<DockerContainer>()
    //         .WithImage("postgres:11-alpine")
    //         .WithEnvironment("POSTGRES_USER", "course")
    //         .WithEnvironment("POSTGRES_PASSWORD", "changeme")
    //         .WithEnvironment("POSTGRES_DB", "mydb")
    //         .WithPortBinding(5432, 5432)
    //         .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
    //         .Build();
    
    // Multiple.
    private readonly TestcontainerDatabase _dbContainer = new ContainerBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "mydb",
            Username = "course",
            Password = "changeme"
        })
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders(); // Remove all logging
        });

        builder.ConfigureTestServices(collection =>
        {
            // Remove the one added by Program.
            collection.RemoveAll(typeof(IDbConnectionFactory));
            collection.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(_dbContainer.ConnectionString));
           
            collection.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(
                "Server=localhost;Port=5432;Database=mydb;User ID=course;Password=changeme;"
                ));
        });
    }

    public async Task InitializeAsync()
    {
        // await _postgresDatabaseContainer.StartAsync();
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        // await _postgresDatabaseContainer.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}