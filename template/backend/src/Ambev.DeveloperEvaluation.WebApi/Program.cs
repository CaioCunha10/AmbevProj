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

                // Configuração do DbContext com conexão ao PostgreSQL
                builder.Services.AddDbContext<DefaultContext>(options =>
                    options.UseNpgsql(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                    )
                );

                // Configuração de CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                });

                // Configuração de autenticação JWT
                builder.Services.AddJwtAuthentication(builder.Configuration);

                // Injeção de dependências (comentei a linha para o código mais limpo)
                Ambev.DeveloperEvaluation.IoC.ApplicationInjection.RegisterDependencies(builder);

                // Repositórios e serviços específicos
                builder.Services.AddScoped<ISaleRepository, SaleRepository>();
                builder.Services.AddScoped<ISaleItemRepository, SaleItemRepository>();
                builder.Services.AddScoped<IProductService, ProductService>();
                builder.Services.AddScoped<ISaleService, SaleService>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();

                // MongoDb

                builder.Services.AddSingleton<MongoDbService>();
                // Event logger
                builder.Services.AddSingleton<EventLogger>();


                // Configuração de validadores
                builder.Services.AddValidatorsFromAssemblyContaining<SalePostDTOValidator>();
                builder.Services.AddValidatorsFromAssembly(typeof(SalePostDTOValidator).Assembly);

                // Configuração de criptografia de senha
                builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
                builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

                // Configuração do AutoMapper
                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                // Configuração do MediatR e validação via pipeline
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

                // Configuração  ambiente de desenvolvimento
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                // Configurações padrão
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
