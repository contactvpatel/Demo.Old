using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api
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

            services.AddControllers(setupAction =>
                {
                    //setupAction.ReturnHttpNotAcceptable = true;
                    setupAction.Filters.Add(typeof(TrackActionPerformanceFilter));
                })
                .AddXmlDataContractSerializerFormatters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApiExceptionHandler(options =>
            {
                options.AddResponseDetails = UpdateApiErrorResponse;
                options.DetermineLogLevel = DetermineLogLevel;
            });
            app.UseHsts();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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

            // Add AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

        private static LogLevel DetermineLogLevel(Exception ex)
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
