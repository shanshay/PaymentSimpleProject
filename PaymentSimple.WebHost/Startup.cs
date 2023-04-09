using Microsoft.EntityFrameworkCore;
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
        }
    }
}
