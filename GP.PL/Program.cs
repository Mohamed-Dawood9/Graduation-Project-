using GP.BLL;
using GP.BLL.Interfaces;
using GP.DAL.Data;
using GP.DAL.Data.Models;
using GP.PL.Helper;
using GP_BLL.Interfaces;
using GP_BLL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GP.PL
{
	public class Program
	{
		public static async Task  Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;


            })
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(configure => {
                configure.LoginPath = "/Account/SignIn";
                configure.ExpireTimeSpan = TimeSpan.FromMinutes(1000);
            }
            );
            builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISpineProcessingService, SpineProcessingService>();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			var app = builder.Build();

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;
			var _dbContext = services.GetRequiredService<AppDbContext>();
			var logggerFactory = services.GetRequiredService<ILoggerFactory>();
			try
			{
				await _dbContext.Database.MigrateAsync();
			}
			catch (Exception ex)
			{

				var logger = logggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "an error happened during migrate the database");
			}

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Patient}/{action=Index}/{id?}");

			app.Run();
		}
	}
}