using Serilog;
using Serilog.Exceptions;
using Serilog.Events;
using Microsoft.AspNetCore.Authentication.Negotiate;

Log.Logger = new LoggerConfiguration().MinimumLevel.Override("Microsoft", LogEventLevel.Debug).MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Debug).Enrich.FromLogContext().Enrich.WithExceptionDetails().MinimumLevel.Debug().WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development,
    WebRootPath = "customwwwroot"
});

builder.Host.UseSerilog(Log.Logger);
// Add services to the container.

builder.Services.AddLogging(loggingBuilder =>
          loggingBuilder.AddSerilog(dispose: true));
builder.Logging.AddJsonConsole();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

var app = builder.Build();


//Microsoft.Extensions.Logging.ILogger logger = Log.Logger.;
IHostApplicationLifetime lifetime = app.Lifetime;
lifetime.ApplicationStopping.Register(() => {
    Log.Information($"Closing at {DateTimeOffset.Now.ToString(System.Globalization.CultureInfo.CurrentCulture)}");
    Log.CloseAndFlush();
    //Wait while the data is flushed
    Thread.Sleep(1000);
});
IWebHostEnvironment env = app.Environment;

app.UseSerilogRequestLogging(options =>
{
    // Customize the message template
    options.MessageTemplate = "Handled {RequestPath}";

    // Emit debug-level events instead of the defaults
    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

    // Attach additional properties to the request completion event
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    };
});


lifetime.ApplicationStarted.Register(() =>
Log.Logger.Information(
$"The application {env.ApplicationName} started"));

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    var currentEndpoint = context.GetEndpoint();

    if (currentEndpoint is null)
    {
        await next(context);
        return;
    }

    Log.Logger.Debug($"Endpoint: {currentEndpoint.DisplayName}");

    if (currentEndpoint is RouteEndpoint routeEndpoint)
    {
        Log.Logger.Debug($"  - Route Pattern: {routeEndpoint.RoutePattern}");
    }

    foreach (var endpointMetadata in currentEndpoint.Metadata)
    {
        Log.Logger.Debug($"  - Metadata: {endpointMetadata}");
    }

    await next(context);
});

//app.MapFallbackToFile("index.html"); ;

app.Run();

Log.Information("Started at " + DateTimeOffset.Now.ToString(System.Globalization.CultureInfo.CurrentCulture));
