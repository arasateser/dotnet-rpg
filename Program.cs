global using dotnet_rpg.Services.CharacterService;
global using dotnet_rpg.Models;
global using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICharacterService, CharacterService>(); //System.InvalidOperationException: Unable to resolve service for type 'dotnet_rpg.Services.ICharacterService' while attempting to activate 'dotnet_rpg.Controllers.CharacterController'.

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
