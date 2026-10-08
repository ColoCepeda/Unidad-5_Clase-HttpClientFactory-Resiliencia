using Application.Interfaces;
using Infrastructure.Resilience;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Parámetros de resiliencia. ¡Cambialos y mirá qué pasa en las consolas!
var resilienceConfig = new ApiClientConfiguration
{
    RetryCount = 3,
    RetryDelayInSeconds = 2,
    UseExponentialBackoff = false,

    FailureRatio = 0.5,
    MinimumThroughput = 4,
    SamplingDurationInSeconds = 30,
    BreakDurationInSeconds = 15
};

// 1) Cliente para la API de chistes (sin resiliencia).
builder.Services.AddHttpClient("jokes", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApis:Jokes"]!);
    client.Timeout = TimeSpan.FromSeconds(10);
});

// 2) Cliente para la API B, con Retry + Circuit Breaker.
builder.Services.AddHttpClient("apiB", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApis:ApiB"]!);
})
.AddResilienceHandler("apiB-resiliencia", pipeline =>
    HttpResiliencePolicies.Configure(pipeline, resilienceConfig));

// 3) Servicios.
builder.Services.AddScoped<IJokeService, JokeService>();
builder.Services.AddScoped<IApiBService, ApiBService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
