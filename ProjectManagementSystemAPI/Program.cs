using Auth;
using Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystemAPI;
using ProjectManagementSystemCore;
using ProjectManagementSystemRepository;
using ProjectManagementSystemService;
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
TokenService tokenService = new TokenService(builder.Configuration["JWT:SecretKey"]!);
builder.Services.AddSingleton(tokenService);
builder.Services.AddMemoryCache();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped(typeof(IService<,,>),typeof(Service<,,>));
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<CacheService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication("CustomScheme")
                        .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", options => { });

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
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var configuration = services.GetRequiredService<IConfiguration>();
    await DatabaseConfiguration.Seed(context,configuration);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
