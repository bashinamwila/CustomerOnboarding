using Csla.Configuration;
using CustomerOnboarding.Ui.Blazor;
using CustomerOnboarding.Ui.Blazor.Components;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using CustomerOnboarding.DalMock;
using CustomerOnboarding.BusinessLibrary.Services.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Services;
using Csla;
using Microsoft.AspNetCore.Authentication.Cookies;
using CustomerOnboarding.BusinessLibrary.Multitenancy.Extensions;
using CustomerOnboarding.BusinessLibrary.Multitenancy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie();
builder.Services.AddCascadingAuthenticationState();

// Add render mode detection services
//builder.Services.AddRenderModeDetection();
builder.Services.AddMultitenancy<TenantInfo, TenantResolver>();


builder.Services.AddCascadingAuthenticationState();

// Add render mode detection services
builder.Services.AddTransient<RenderModeProvider>();
builder.Services.AddScoped<CustomerOnboarding.Ui.Blazor.ActiveCircuitState>();

builder.Services.AddScoped(typeof(CircuitHandler), typeof(CustomerOnboarding.Ui.Blazor.ActiveCircuitHandler));



// CSLA requires AddHttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();


// Add CSLA
builder.Services.AddCsla(o => o
  .AddAspNetCore()
  .DataPortal(o=>o
  .AddClientSideDataPortal(o=>o
  .DataPortalCacheType=typeof(MultitenancyDataPortalCache)))
  .AddServerSideBlazor(ssb => ssb.UseInMemoryApplicationContextManager = false));

// Required by CSLA data portal controller. If using Kestrel:
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// Required by CSLA data portal controller. If using IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.AddDalMock();
builder.Services.AddScoped<IEmailSender,EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
