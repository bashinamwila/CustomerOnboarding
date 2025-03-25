// TestFixture.cs
using Csla.Configuration;
using CustomerOnboarding.DalMock;
using Microsoft.Extensions.DependencyInjection;
using System;

public class CslaTestFixture
{
    public IServiceProvider Services { get; }

    public CslaTestFixture()
    {
        var services = new ServiceCollection();
        services.AddCsla();
        services.AddDalMock();
        Services = services.BuildServiceProvider();
    }
}

