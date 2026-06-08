using System;
using System.Collections.Generic;
using System.Text;
using Testcontainers.MsSql;

namespace ClinicApp.Tests.Integration.Fixture;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container;

    public string ConnectionString => _container.GetConnectionString() + ";Database=ClinicApp;TrustServerCertificate=True;";

    public DatabaseFixture()
    {
        _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-latest")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithPassword("Passw0rd!")
            .WithEnvironment("MSSQL_PID", "Express")
            .Build();
    }

    public Task DisposeAsync()
    {
        return _container.DisposeAsync().AsTask();
    }

    public Task InitializeAsync()
    {
        return _container.StartAsync();
    }
}
