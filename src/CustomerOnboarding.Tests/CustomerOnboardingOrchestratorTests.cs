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
    public async Task CreateAsync_InitializesWithThreeSteps()
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
        Assert.Equal(3, orchestrator.Steps.Count);
        Assert.Contains(orchestrator.Steps, s => s.Name == "Create Account");
        Assert.Contains(orchestrator.Steps, s => s.Name == "Send Email Notification");
        Assert.Contains(orchestrator.Steps, s => s.Name == "Confirm Email");

    }

    [Fact]
    public async Task CreateAccountStep_IsFirstStepInOrchestrator()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
        var portal = factory.GetPortal<CustomerOnboardingOrchestrator>();

        // Act
        var orchestrator = await portal.CreateAsync();
        var createAccountStep = orchestrator.Steps.FirstOrDefault(s => s.Name == "Create Account");

        // Assert
        Assert.NotNull(createAccountStep);
        Assert.Equal(0, orchestrator.Steps.IndexOf(createAccountStep));
        Assert.False(createAccountStep.IsCompleted);
    }

    [Fact]
    public async Task Orchestrator_Initializes_CreateAccountStep_WithExpectedDefaults()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
        var portal = factory.GetPortal<CustomerOnboardingOrchestrator>();

        // Act
        var orchestrator = await portal.CreateAsync();
        var step = (CreateAccountStep)orchestrator.Steps[0];

        // Assert
        Assert.NotNull(step);
        Assert.Equal(0, step.StepIndex);
        Assert.False(step.IsCompleted);
        Assert.Equal("Create Account", step.Name);
        Assert.Equal(StepType.Manual, step.Type);
    }

    [Fact]
    public async Task CreateAccountStep_IsCompleted_ShouldBeTrue_WhenAllRequiredFieldsAreSet()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
        var portal = factory.GetPortal<CustomerOnboardingOrchestrator>();
        var orchestrator = await portal.CreateAsync();
        var step = (CreateAccountStep)orchestrator.Steps[0];

        step.FirstName = "John";
        step.LastName = "Doe";
        step.OrganisationName = "Acme Inc";
        step.WorkEmail = "john.doe@example.com";
        step.OrganisationPhone = "123456789";
        step.Password = "P@ssw0rd";
        step.Password2 = "P@ssw0rd";
        step.NumberOfEmployees = 10;

        // Act
        var isValid = step.IsCompleted;

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public async Task CreateAccountStep_ShouldHaveBrokenRule_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
        var portal = factory.GetPortal<CustomerOnboardingOrchestrator>();
        var orchestrator = await portal.CreateAsync();
        var step = (CreateAccountStep)orchestrator.Steps[0];

        // Act
        step.Password = "pass123";
        step.Password2 = "diffpass";

        // Assert
        var brokenRules = step.BrokenRulesCollection;
        Assert.Contains(brokenRules, r => r.Description.Contains("Passwords do not match"));
    }

    [Fact]
    public async Task MoveNextAsync_AutomaticStep_CompletesStep_AndAdvancesIndex()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
        var portal = factory.GetPortal<CustomerOnboardingOrchestrator>();
        var orchestrator = await portal.CreateAsync();

        // Populate CreateAccountStep (first step)
        var step = orchestrator.Steps[0] as CreateAccountStep;
        Assert.NotNull(step);

        step.FirstName = "John";
        step.LastName = "Doe";
        step.OrganisationName = "Acme Corp";
        step.WorkEmail = "john.doe@acme.com";
        step.OrganisationPhone = "123456789";
        step.Password = "Secure123!";
        step.Password2 = "Secure123!";
        step.NumberOfEmployees = 10;

        // Check rules to ensure IsCompleted is updated
        

        // Act
        await orchestrator.MoveNextAsync();

       // await Task.Delay(500);

        orchestrator =await portal.FetchAsync(orchestrator.TenantId);

        // Assert

        Assert.Equal(2,orchestrator.CurrentStepIndex);
    }


}
