using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using UserManagementSystem.Api;
using UserManagementSystem.Application.Features.Users.Commands;
using UserManagementSystem.Application.FluentValidator;
using UserManagementSystem.Infrastructure;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateBootstrapLogger();
try
{
    Log.Information("Application Starting......");


    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("NamedConnectionString");

    var migrationAssembly = Assembly.GetExecutingAssembly();

    builder.Services.AddDbContext<UserManagementSystemContext>(options =>
       options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationAssembly)));
    Log.Information("AddDbcontext......");
    //Serilog Configuration
    builder.Host.UseSerilog((context, lc) =>
    lc.MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
     );

    // Add services to the container.
    builder.Services.AddControllers();

    //Jwt token configuration
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
           ValidAudience= builder.Configuration["Jwt:Audience"],
           ValidIssuer = builder.Configuration["Jwt:Issuer"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

    #region MediatR Configuration
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(migrationAssembly);
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.RegisterServicesFromAssembly(typeof(UserAddCommand).Assembly);
       
    });
    #endregion
    #region Automapper Configuration
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    #endregion


    // Hangfire
    builder.Services.AddHangfire(config =>
        config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
    builder.Services.AddHangfireServer();

    // Register FluentValidation
    builder.Services.AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UserCreateValidator>());

    builder.Services.AddDependency(); //All dependency added this method
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    //Configure the HTTP request pipeline.
     
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Api");
        });
       

    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();
    app.UseHangfireDashboard("/hangfire");

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex,"Application creashed");
}
finally
{
    Log.CloseAndFlush();
}