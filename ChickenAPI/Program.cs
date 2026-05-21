using Microsoft.EntityFrameworkCore;
using ChickenAPI.Model;

namespace ChickenAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<FarmDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")));
            
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (true)
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "Chicken API v1");
                    options.RoutePrefix = "swagger";
                });
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<FarmDbContext>();
                db.Database.Migrate(); // Applies any pending migrations automatically
            }
            
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
