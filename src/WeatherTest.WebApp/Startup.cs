using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Models.UnitOfMeasure;
using WeatherTest.WebApp.Services;

namespace WeatherTest.WebApp
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddJsonFile($"weatherProviders.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"unitsOfMeasure.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<List<Measure>>(
				options => Configuration.GetSection("SupportedMeasurement").Bind(options));
			services.Configure<List<Unit>>(
				options => Configuration.GetSection("SupportedUnitOfMeasurement").Bind(options));
			services.Configure<List<WeatherProvider>>(
				options => Configuration.GetSection("WeatherProvider").Bind(options));

			// Add framework services.
			services.AddMvc();

			services.AddScoped<IUnitOfMeasurementsService, UnitOfMeasurementsService>();
			services.AddTransient<IWeatherChecker, WeatherChecker>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			//app.UseMvcWithDefaultRoute();
			app.UseMvc();
		}
	}
}
