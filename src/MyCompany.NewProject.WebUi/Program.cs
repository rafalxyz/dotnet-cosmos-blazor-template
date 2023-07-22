using MyCompany.NewProject.Application;
using MyCompany.NewProject.Infrastructure;
using MyCompany.NewProject.Integration.MicrosoftGraph;
using MyCompany.NewProject.Persistence;
using MyCompany.NewProject.WebApi;
using MyCompany.NewProject.WebUi.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers()
    .AddApplicationPart(typeof(IWebApiMarker).Assembly)
    .AddApplicationJsonOptions();
builder.Services.AddPersistence();
builder.Services.AddInfrastructure();
builder.Services.AddMicrosoftGraphIntegration(builder.Configuration, builder.Environment);
builder.Services.AddApplication();
builder.Services.AddWebUi();
builder.Services.AddAppAuthentication(builder.Configuration);
builder.Services.AddAppSwagger();

if (!builder.Environment.IsDevelopment())
{
    builder.Logging.ClearProviders();
    builder.Services.AddApplicationInsights();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAppSwagger();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

try
{
    app.Run();
}
catch (Exception ex)
{
    builder.LogToApplicationInsightsOnError(ex);
    throw;
}
