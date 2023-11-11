global using Microsoft.AspNetCore.Mvc;
global using MTGCardsAPI.Data;
global using Microsoft.EntityFrameworkCore;
global using MTGCardsAPI.Models;
global using MTGCardsAPI.DTO;
using MTGCardsAPI.Services.CardTypeService;
using MTGCardsAPI.Services.AbilityService;
using MTGCardsAPI.Services.ColourService;
using MTGCardsAPI.Services.SetService;
using MTGCardsAPI.Services.CardService;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<ICardTypeService, CardTypeService>();
builder.Services.AddScoped<IAbilityService, AbilityService>();
builder.Services.AddScoped<IColourService, ColourService>();
builder.Services.AddScoped<ISetService, SetService>();
builder.Services.AddScoped<ICardService, CardService>();


builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));
builder.Services.AddDbContext<AuthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalAuthConnection")));

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<AuthContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
