using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace JonkoTrackerAPI;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = """
                              JWT Authorization header using the Bearer scheme.
                              Enter 'Bearer' [space] and then your token in the text input below.
                              Example: 'Bearer 12345abcdef'
                              """,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
        });
        
        builder.Services.AddDbContextPool<DatabaseContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(builder.Configuration["SelectedConnection"])));
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
        
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Default", policyBuilder =>
            {
                policyBuilder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((_) => true)
                    .AllowCredentials();
            });
        });
        
        WebApplication app = builder.Build();
        
        app.Use(async (context, next) =>
        {
            LogRequest(context.Request);
            
            await next();
        });
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseCors("Default");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
    
    private static void LogRequest(HttpRequest request)
    {
        Console.WriteLine($"Request Method: {request.Method}");
        Console.WriteLine($"Path: {request.Path}");
        Console.WriteLine($"Query String: {request.QueryString}");
        
        Console.WriteLine("Headers:");
        foreach (KeyValuePair<string, StringValues> header in request.Headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }
        
        if (!request.Body.CanRead)
        {
            return;
        }
        
        request.EnableBuffering();
        using StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
        string requestBody = reader.ReadToEnd();
        
        Console.WriteLine($"Request Body: {requestBody}");
        
        request.Body.Seek(0, SeekOrigin.Begin);
    }
}