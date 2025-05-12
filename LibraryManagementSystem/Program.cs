using System.Text;
using LibraryManagementSystem.Core;
using LibraryManagementSystem.Core.Entities.User;
using LibraryManagementSystem.Core.Mapping;
using LibraryManagementSystem.Core.Service.Contract;
using LibraryManagementSystem.Repository;
using LibraryManagementSystem.Repository.Data.Contexts;
using LibraryManagementSystem.Repository.Data.Seeding;
using LibraryManagementSystem.Service.Admin;
using LibraryManagementSystem.Service.book;
using LibraryManagementSystem.Service.category;
using LibraryManagementSystem.Service.Librarian;
using LibraryManagementSystem.Service.Token;
using LibraryManagementSystem.Service.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            #region Database Config
            builder.Services.AddDbContext<LibraryDbContext>(o =>
            {
                o.UseSqlServer(
                    builder.Configuration.GetConnectionString("LibrarySystemConnection")
                );
            });
            builder
                .Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<LibraryDbContext>();
            #endregion

            #region Injection System Services
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<ILibrarianService, LibrarianService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            #endregion

            #region Injection security and Cors Policy
            builder
                .Services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = builder.Configuration["JWT:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = builder.Configuration["JWT:Audience"],
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])
                            ),
                        };
                });

            builder.Services.AddCors(o =>
            {
                o.AddPolicy(
                    "MyCors",
                    c =>
                    {
                        c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    }
                );
            });
            #endregion

            var app = builder.Build();

            #region AutoUpdate-Seed
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
            #endregion

            // Configure the HTTP request pipeline.
            #region Middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("MyCors");

            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
