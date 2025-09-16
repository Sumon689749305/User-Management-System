using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using UserManagementSystem.Api;
using UserManagementSystem.Application.Features.Users.Commands;
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
    builder.Services.AddDependency(); //All dependency added this method
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Api");
        });
    }

    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

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