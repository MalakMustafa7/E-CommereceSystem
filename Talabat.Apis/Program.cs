
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Talabat.Apis.Errors;
using Talabat.Apis.Extentions;
using Talabat.Apis.Helper;
using Talabat.Apis.MiddleWares;
using Talabat.Core.Models.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            #region  Configure Services Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });
             
            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.WithOrigins(builder.Configuration["FrontBaseURL"]);

                });
            });
            #endregion

            var app = builder.Build();

            #region  Updata-Database
            // StoreContext DbContext = new StoreContext(); InValid
            //await DbContext.Database.MigrateAsync();
            using var Scope= app.Services.CreateScope();
            //Group of Services Lifetime Scoped
            var Services = Scope.ServiceProvider;
            // Services itis self
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var DbContext = Services.GetRequiredService<StoreContext>();
                //Ask CLR For Creating Object From DbContext Explicity
                await DbContext.Database.MigrateAsync();

                var IdentityDbContext = Services.GetRequiredService<AppIdentityContext>();
                await IdentityDbContext.Database.MigrateAsync();
                var usermanager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityContextSeed.SeedUserAsync(usermanager);
                await StoreContextSeed.SeedAsync(DbContext);
            }
            catch (Exception ex) {
                //log erron on console
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured During Applying Migration");
            }
            #endregion



            #region ConfigureMiddeleWare Configure the HTTP request pipeline. 
            
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.UseSwaggerMiddleWares();
            }
            
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
