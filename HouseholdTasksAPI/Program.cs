
using HouseholdTasksAPI.Database;
using HouseholdTasksAPI.Hubs;
using Microsoft.EntityFrameworkCore;
using System;

namespace HouseholdTasksAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSignalR();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient", builder =>
                {
                    builder.WithOrigins("https://0.0.0.0", "https://localhost") // HIER die URL deines Blazor-Frontends eintragen!
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials(); // Zwingend erforderlich für SignalR!
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowBlazorClient");
            app.UseAuthorization();
            app.MapHub<TaskHub>("/task");

            app.MapControllers();

            app.Run();
        }
    }
}
