using System;
using ApplicationCore.AssetAggregate.InterestAssetAggregate;
using ApplicationCore.AssetAggregate.RealEstateAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.PortfolioAggregate;
using ApplicationCore.UserAggregate;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PublicAPI.Configures;
using PublicAPI.Filters;

namespace PublicAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers(options =>
            {
                options.Filters.Add<SetUserIdFilter>();
            });
            services.AddDbContext<AppDbContext>(builder =>
                builder.UseNpgsql(Configuration.GetConnectionString("LocalDBConnection"))
                    .LogTo(Console.WriteLine, LogLevel.Critical));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PublicAPI", Version = "v1" });
                c.EnableAnnotations();
            });
            services.ConfigureJwtAuthenticationScheme(Configuration["JWTSigningKey"]);
            services.AddAuthorization();
            
            // configure DI services
            
            services.AddSingleton(Configuration);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(EfRepository<>));
            services.AddScoped<IInterestAssetService, InterestAssetService>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IRealEstateService, RealEstateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PublicAPI v1"));
            }



            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true) // allow any origin
                .AllowCredentials()); // allow credentials 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
