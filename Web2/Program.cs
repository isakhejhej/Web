using System.Net;
using Web2.Data;

namespace Web2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<DB>();
            builder.Services.AddScoped<MainDB>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("_policy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Häftig API",
                    Description = "beskrivning"
                });
            });


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("_policy");

            app.UseDeveloperExceptionPage();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.Run("http://127.0.0.1:5001");
            }
            else
            {
                app.Run();
            }

		}
    }
}