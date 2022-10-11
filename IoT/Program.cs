using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IoT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Manually call ConfigureServices()
            ConfigureServices(builder.Services);

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (true) //app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            string connString = PCDataDLL.SecureData.DataDictionary["IoTDB"];
            // Add DbContext using SQL Server Provider
            services.AddDbContext<IotDbContext>(options =>
            {
                options.UseMySql(connString, ServerVersion.AutoDetect(connString),
                    mysqlOptions =>
                    {
                        mysqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                    });
            },
                ServiceLifetime.Transient);

            services.AddSwaggerGen();

        }

    }
}
