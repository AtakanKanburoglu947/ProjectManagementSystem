using Auth;
using Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystemCore;
using ProjectManagementSystemRepository;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(DtoMapper).Assembly);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});
var secretKey = "C7CDA87C-89EF-4247-A2A6-0B9FBC49F2D0";
TokenService tokenService = new TokenService(secretKey);
builder.Services.AddSingleton(tokenService);
builder.Services.AddScoped<AuthService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = tokenService.GetTokenValidationParameters();
    }
    );
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStatusCodePages(async context => {
    HttpResponse response = context.HttpContext.Response;
    if (response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        response.ContentType = "application/json";
        var error = new { Message = "Eriþim izni yok" };
        await response.WriteAsync(JsonSerializer.Serialize(error));
    }
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
