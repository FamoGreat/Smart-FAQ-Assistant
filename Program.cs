using Microsoft.Extensions.Options;
using OpenAI;
using SmartFAQAssistantApi.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAISettings"));
builder.Services.Configure<Qdrantsettings>(builder.Configuration.GetSection("Qdrantsettings"));

builder.Services.AddSingleton<OpenAIClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<OpenAISettings>>().Value;
    return new OpenAIClient(settings.ApiKey);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
