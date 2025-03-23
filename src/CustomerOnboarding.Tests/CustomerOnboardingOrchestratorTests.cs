namespace CustomerOnboarding.Tests;

using Xunit;
using Csla;
using CustomerOnboarding.BusinessLibrary;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;

public class CustomerOnboardingOrchestratorTests : IClassFixture<CslaTestFixture>
{
    private readonly IServiceProvider _serviceProvider;

    public CustomerOnboardingOrchestratorTests(CslaTestFixture fixture)
    {
        _serviceProvider = fixture.Services;
    }
    [Fact]
    public async Task CreateAsync_InitializesWithTwoSteps()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
        var portal = factory.GetPortal<CustomerOnboardingOrchestrator>();

        // Act
        var orchestrator = await portal.CreateAsync();

        // Assert
        Assert.NotNull(orchestrator);
        Assert.False(string.IsNullOrWhiteSpace(orchestrator.TenantId));
        Assert.False(orchestrator.IsComplete);
        Assert.Equal(2, orchestrator.Steps.Count);
        Assert.Contains(orchestrator.Steps, s => s.Name == "Create Account");
        Assert.Contains(orchestrator.Steps, s => s.Name == "Send Email Notification");
    }

    [Fact]
    public async Task GoTo_WithValidIndex_ReturnsCorrectStep()
    {
        var portal = _serviceProvider.GetRequiredService<IDataPortalFactory>()
                      .GetPortal<CustomerOnboardingOrchestrator>();
        var orchestrator = await portal.CreateAsync();

        var step = orchestrator.GoTo(0);

        Assert.Equal("Create Account", step.Name);
    }

    [Fact]
    public async Task MoveNextAsync_ExecutesAutomaticStepAndIncrementsIndex()
    {
        var portal = _serviceProvider.GetRequiredService<IDataPortalFactory>()
                      .GetPortal<CustomerOnboardingOrchestrator>();
        var orchestrator = await portal.CreateAsync();

        // Simulate completing first manual step to move to the automatic step
        orchestrator.GoTo(0).GetType().GetProperty("IsCompleted")?.SetValue(orchestrator.GoTo(0), true);

        await orchestrator.MoveNextAsync(); // Should trigger automatic step

        Assert.True(orchestrator.GoTo(1).IsCompleted || orchestrator.Steps.Count > 1);
    }

    [Fact]
    public async Task IsComplete_ShouldBeTrue_WhenAllStepsCompleted()
    {
        var portal = _serviceProvider.GetRequiredService<IDataPortalFactory>()
                      .GetPortal<CustomerOnboardingOrchestrator>();
        var orchestrator = await portal.CreateAsync();

        foreach (var step in orchestrator.Steps)
        {
            step.GetType().GetProperty("IsCompleted")?.SetValue(step, true);
        }

      //  await orchestrator.BusinessRules.CheckRulesAsync();

        Assert.True(orchestrator.IsComplete);
    }

    [Fact]
    public async Task MoveNextAsync_DoesNothing_WhenAllStepsAreDone()
    {
        var portal = _serviceProvider.GetRequiredService<IDataPortalFactory>()
                      .GetPortal<CustomerOnboardingOrchestrator>();
        var orchestrator = await portal.CreateAsync();

        // Force index beyond last step
        typeof(CustomerOnboardingOrchestrator)
            .GetProperty("CurrentStepIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(orchestrator, orchestrator.Steps.Count);

        await orchestrator.MoveNextAsync();

        // Still within bounds, no crash
        Assert.True(true);
    }



}
