using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.AutoMapperProfiles;
using Ambev.DeveloperEvaluation.Application.Interfaces.Service;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Infrastructure.Repositories;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using Ambev.DeveloperEvaluation.WebApi.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi
{
    public class Program
    {
 
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting web application");

                var builder = WebApplication.CreateBuilder(args);

                builder.AddDefaultLogging();
                builder.Logging.ClearProviders();
                builder.Logging.AddConsole();

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.AddBasicHealthChecks();

                // Configura��o do DbContext com conex�o ao PostgreSQL
                builder.Services.AddDbContext<DefaultContext>(options =>
                    options.UseNpgsql(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                    )
                );

                // Configura��o de CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                });

                // Configura��o de autentica��o JWT
                builder.Services.AddJwtAuthentication(builder.Configuration);

                // Inje��o de depend�ncias (comentei a linha para o c�digo mais limpo)
                Ambev.DeveloperEvaluation.IoC.ApplicationInjection.RegisterDependencies(builder);

                // Reposit�rios e servi�os espec�ficos
                builder.Services.AddScoped<ISaleRepository, SaleRepository>();
                builder.Services.AddScoped<ISaleItemRepository, SaleItemRepository>();
                builder.Services.AddScoped<IProductService, ProductService>();
                builder.Services.AddScoped<ISaleService, SaleService>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();

                // MongoDb

                builder.Services.AddSingleton<MongoDbService>();
                // Event logger
                builder.Services.AddSingleton<EventLogger>();


                // Configura��o de validadores
                builder.Services.AddValidatorsFromAssemblyContaining<SalePostDTOValidator>();
                builder.Services.AddValidatorsFromAssembly(typeof(SalePostDTOValidator).Assembly);

                // Configura��o de criptografia de senha
                builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
                builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

                // Configura��o do AutoMapper
                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                // Configura��o do MediatR e valida��o via pipeline
                builder.Services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(
                        typeof(ApplicationLayer).Assembly,
                        typeof(Program).Assembly
                    );
                });
                builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                var app = builder.Build();

                // Middlewares
                app.UseMiddleware<ValidationExceptionMiddleware>();

                // Configura��o  ambiente de desenvolvimento
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                // Configura��es padr�o
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseBasicHealthChecks();
                app.MapControllers();

                 app.Run();
            }
            catch (Exception ex)
            {
                 Log.Fatal(ex, "Application terminated unexpectedly");

                if (ex is AggregateException aggregateException)
                {
                    foreach (var inner in aggregateException.InnerExceptions)
                    {
                        Console.WriteLine($"[ERROR] {inner.GetType().Name}: {inner.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"[ERROR] {ex.GetType().Name}: {ex.Message}");
                }

                 throw;
            }
            finally
            {
                 Log.CloseAndFlush();
            }
        }
    }
}
