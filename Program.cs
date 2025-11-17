
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SurfLib.Business;
using SurfLib.Data;
using SurfLib.Data.Models;
using SurfLib.Data.Profiles;
using SurfLib.Data.Repositories;
using SurfLib.Data.Services;
using SurfLib.Utils;

namespace MediumLib
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            // Attention ne pas oublier d'installer la dépendance d'injection de dépendance 
            // pour l'autoMapper 
            //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(MareeProfile));

            // Quand quelqu'un demande ISpot, Utilise Spot
            // Mais SpotRepository n'est pas déclaré en tant que service DI
            builder.Services.AddTransient<ISpotRepository, SpotRepository>();
            builder.Services.AddTransient<IMareeRepository, MareeRepository>();
            builder.Services.AddTransient<IMareeService, MareeService>();
            builder.Services.AddHttpClient<ICityRepository, CityRepository>();
            builder.Services.AddHttpClient<IMareeScrapper, MareeScrapper>();

            builder.Services.AddDbContext<SurfDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));
            //options.UseSqlite(builder.Configuration.GetConnectionString("SqlLite")));
            //.UseLazyLoadingProxies()

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapSwaggerUI();

                //app.UseSwagger();
                //app.UseSwaggerUI();

            }


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
