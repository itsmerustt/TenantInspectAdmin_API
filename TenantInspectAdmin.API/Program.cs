using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Text;
using TenantInspectAdmin.API.ConsoleLogger;
using TenantInspectAdmin.DAL.Apple;
using TenantInspectAdmin.DAL.Notification;
using TenantInspectAdmin.DAL.Repository;
using TenantInspectAdmin.Service.Apple;
using TenantInspectAdmin.Service.Notification;
using TenantInspectAdmin.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS Policy - Allow All
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Allow all origins
              .AllowAnyMethod()   // Allow all HTTP methods (GET, POST, etc.)
              .AllowAnyHeader();  // Allow all headers
    });
});

builder.Services.AddAuthentication(f =>
{
    f.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    f.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(k =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    k.SaveToken = true;
    k.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddScoped<ITenantInspectAdminRepository, TenantInspectAdminService>();
builder.Services.AddScoped<IAppleService, AppleService>();

builder.Services.AddSingleton<INotificationService>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    var googleCredential = Path.Combine(env.ContentRootPath, "Firebase", "firebase.json");

    // Log the path for debugging
    var logger = sp.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Looking for Firebase credential at: {googleCredential}");

    if (!System.IO.File.Exists(googleCredential))
    {
        logger.LogError("Firebase credential file not found.");
        throw new FileNotFoundException("Firebase credential file not found.", googleCredential);
    }

    var credential = GoogleCredential.FromFile(googleCredential);
    var firebaseApp = FirebaseApp.Create(new AppOptions()
    {
        Credential = credential
    });

    logger.LogInformation("Firebase initialized successfully.");
    return new NotificationService(firebaseApp);
});

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1",
                       new OpenApiInfo
                       {
                           Title = "Tenant Inspect Admin API",
                           Version = "V1",
                           Description = "This API provides administrative functionalities for managing tenants, inspections, properties, and related resources in the Tenant Inspect platform."
                       });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Authorization header using the Bearer scheme. Example \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    swagger.AddSecurityDefinition(securitySchema.Reference.Id, securitySchema);
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securitySchema,Array.Empty<string>() }
    });
});

builder.Services.TryAddEnumerable(
    ServiceDescriptor.Singleton<ILoggerProvider, ColorConsoleLoggerProvider>());
LoggerProviderOptions.RegisterProviderOptions
    <ColorConsoleLoggerConfiguration, ColorConsoleLoggerProvider>(builder.Services);

var app = builder.Build();
app.UseCors("AllowAll");
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();