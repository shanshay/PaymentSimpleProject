using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PaymentSimple.Core.Abstractions.Repositories;
using PaymentSimple.DataAccess;
using PaymentSimple.DataAccess.Repositories;

namespace PaymentSimple.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            //services.AddDbContext<DataContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<DataContext>(options =>
            //    options
            //        .UseLazyLoadingProxies()
            //        .UseSqlServer(
            //            Configuration.GetConnectionString("ALRConnection"), option => option.UseRelationalNulls())
            //                );
            services.AddDbContext<DataContext>(options =>
                options
                    .UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"), option => option.UseRelationalNulls())
                    .UseLazyLoadingProxies());

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers().AddMvcOptions(x =>
                            x.SuppressAsyncSuffixInActionNames = false);
            


            // In production, the React files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/build";
            //});

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            //if (env.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller}/{action=Index}/{id?}");
            //});
        }
    }
}
