using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TaskerApi.Cross;
using TaskerApi.Model;

namespace TaskerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MongoContext>();
            services.AddTransient<ITaskItemRepository, TaskItemRepository>();

            services.Configure<Settings>(o =>
            {
                o.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                o.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.Formatting = Formatting.Indented);

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = true;

                    options.ApiName = "markettaskerapi";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}