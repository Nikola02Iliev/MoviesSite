using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviesSite.Context;
using MoviesSite.Models;
using MoviesSite.Repositories.Implementations;
using MoviesSite.Repositories.Interfaces;
using MoviesSite.Services.Implementations;
using MoviesSite.Services.Interfaces;

namespace MoviesSite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireRole("Admin"));
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
            builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
            builder.Services.AddScoped<IRatingsRepository, RatingsRepository>();

            builder.Services.AddScoped<IMoviesService, MoviesService>();
            builder.Services.AddScoped<IReviewsService, ReviewsService>();
            builder.Services.AddScoped<IRatingsService, RatingsService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/ErrorHandler/GenericError");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Movies}/{action=Index}/{id?}");

            app.MapRazorPages();


            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Manager" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                string email = "admin@gmail.com";
                string password = "Admin1!";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new AppUser();
                    user.Email = email;
                    user.UserName = email;

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");

                }
            }

            await app.RunAsync();
        }
    }
}