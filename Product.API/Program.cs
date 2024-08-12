using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Product.API.Extensions;
using Product.API.Middleware;
using Product.Infrastructure;
using StackExchange.Redis;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiReguestration();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Jwt Auth Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    option.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequiment = new OpenApiSecurityRequirement { { securitySchema, new[] { "Beater" } } };
    option.AddSecurityRequirement(securityRequiment);
});
 



builder.Services.InfrastructureConfiguration(builder.Configuration);
builder.Services.AddSingleton<IConnectionMultiplexer>(i =>
{
    var configure = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(configure);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

InfrastructureRequistration.infrastructureConfigMiddleware(app);

app.Run();
