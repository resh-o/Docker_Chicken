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

            // Replace your current using(var scope...) block with this:
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<FarmDbContext>();

                int retries = 5;
                while (retries > 0)
                {
                    try
                    {
                        Console.WriteLine("Attempting to run database migrations...");
                        db.Database.Migrate();
                        Console.WriteLine("Database migrations applied successfully!");
                        break;
                    }
                    catch (System.Exception ex)
                    {
                        retries--;
                        Console.WriteLine($"Database not ready yet ({ex.Message}). Retrying in 5 seconds... ({retries} retries left)");
                        System.Threading.Thread.Sleep(5000);
                        if (retries == 0) throw;
                    }
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                // Only enforce HTTPS if you aren't testing locally/in CI meshes
                app.UseHttpsRedirection();
            }
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
