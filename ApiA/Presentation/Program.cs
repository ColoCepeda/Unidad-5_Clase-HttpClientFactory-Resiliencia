using Application.Interfaces;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1) Registramos un cliente con nombre ("named client") para la API de chistes.
//    Todo lo que configuremos acá aplica a cada HttpClient que se cree con CreateClient("jokes").
builder.Services.AddHttpClient("jokes", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApis:Jokes"]!);
    client.Timeout = TimeSpan.FromSeconds(10);
});

// 2) Registramos el servicio: cuando alguien pida IJokeService, le damos un JokeService.
builder.Services.AddScoped<IJokeService, JokeService>();

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
