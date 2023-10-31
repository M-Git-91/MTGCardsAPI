global using Microsoft.AspNetCore.Mvc;
global using MTGCardsAPI.Data;
global using Microsoft.EntityFrameworkCore;
global using MTGCardsAPI.Models;
global using MTGCardsAPI.DTO;
using MTGCardsAPI.Services.CardTypeService;
using MTGCardsAPI.Services.AbilityService;
using MTGCardsAPI.Services.ColourService;
using MTGCardsAPI.Services.SetService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICardTypeService, CardTypeService>();
builder.Services.AddScoped<IAbilityService, AbilityService>();
builder.Services.AddScoped<IColourService, ColourService>();
builder.Services.AddScoped<ISetService, SetService>();


builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));
builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
