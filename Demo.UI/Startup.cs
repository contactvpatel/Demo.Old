using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using AutoMapper;
using Demo.Application.Interfaces;
using Demo.Application.Services;
using Demo.Common;
using Demo.Common.Filters;
using Demo.Common.Middleware;
using Demo.Core.Configuration;
using Demo.Core.Interfaces;
using Demo.Core.Repositories;
using Demo.Core.Repositories.Base;
using Demo.Infrastructure.Data;
using Demo.Infrastructure.Logging;
using Demo.Infrastructure.Repositories;
using Demo.Infrastructure.Repositories.Base;
using Demo.UI.HealthChecks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demo.UI
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
            // service dependencies
            ConfigureDemoServices(services);

            services.AddSingleton<IScopeInformation, ScopeInformation>();

            services.AddControllersWithViews(config => { config.Filters.Add(typeof(TrackActionPerformanceFilter)); })
                .AddFluentValidation(s =>
                    {
                        s.RegisterValidatorsFromAssemblyContaining<Startup>();
                        s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    }
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureDemoServices(IServiceCollection services)
        {
            // Add Core Layer
            services.Configure<DemoSettings>(Configuration);

            // Add Infrastructure Layer
            ConfigureDatabases(services);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            // Add Application Layer
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            // Add Web Layer
            services.AddAutoMapper(typeof(Startup)); // Add AutoMapper
            services.AddScoped<Interfaces.IProductService, Services.ProductService>();
            services.AddScoped<Interfaces.ICategoryService, Services.CategoryService>();

            // Add Miscellaneous
            services.AddHttpContextAccessor();
            services.AddHealthChecks().AddCheck<HomeHealthCheck>("home_page_health_check");
        }

        public void ConfigureDatabases(IServiceCollection services)
        {
            // use in-memory database
            services.AddDbContext<DemoContext>(c =>
                c.UseInMemoryDatabase("DemoDbConnection"));

            //// use real database
            //services.AddDbContext<DemoContext>(c =>
            //    c.UseSqlServer(Configuration.GetConnectionString("DemoDbConnection")));
        }

        private LogLevel DetermineLogLevel(Exception ex)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
                ex.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
            {
                return LogLevel.Critical;
            }

            return LogLevel.Error;
        }

        private void UpdateApiErrorResponse(HttpContext context, Exception ex, ApiError apiError)
        {
            if (ex.GetType().Name == nameof(SqlException))
            {
                apiError.Detail = "Exception was a database exception!";
            }

            //apiError.Links = "https://gethelpformyerror.com/";
        }
    }
}
