using LibraryManagementSystem.Core.Entities.User;
using LibraryManagementSystem.Repository.Data.Contexts;
using LibraryManagementSystem.Repository.Data.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<LibraryDbContext>(o =>
            {
                o.UseSqlServer(
                    builder.Configuration.GetConnectionString("LibrarySystemConnection")
                );
            });
            builder
                .Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<LibraryDbContext>();
            var app = builder.Build();
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<LibraryDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            try
            {
                await _dbContext.Database.MigrateAsync();
                await RoleSeeding.SeedRoleAsync(roleManager);
                await AdminDbSeeding.SeedAdminAsync(userManager);
                await LibrarianDbSeeding.LibrarianDbSeedAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occured During Apply The Migration");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
