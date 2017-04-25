using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;

namespace WeatherTest.WebApp.Controllers
{
	public class HomeController : Controller
	{
		readonly IUnitOfMeasurementsService measurements;

		readonly IWeatherChecker weatherChecker;

		public HomeController(
			IUnitOfMeasurementsService measurements,
			IWeatherChecker weatherChecker)
		{
			this.measurements = measurements
				?? throw new ArgumentNullException(nameof(measurements));
			this.weatherChecker = weatherChecker
				?? throw new ArgumentNullException(nameof(weatherChecker));
		}

		[HttpGet("")]
		public async Task<IActionResult> Index()
		{
			var vm = new WeatherViewModel
			{
				NewLocation = "bournemouth",
				TemperatureUnit = measurements.TemperatureUnits.ElementAt(0),
				WindSpeedUnit = measurements.WindSpeedUnits.ElementAt(0)
			};
			await CheckWeather(vm);

			return View(vm);
		}

		[HttpPost("")]
		public async Task<IActionResult> Index(WeatherViewModel vm)
		{
			if (ModelState.IsValid)
				await CheckWeather(vm);

			return View(vm);
		}

		async Task CheckWeather(WeatherViewModel vm)
		{
			vm.TemperatureUnit = measurements.TemperatureUnits.First(t => t.Id == vm.TemperatureUnit.Id);
			vm.WindSpeedUnit = measurements.WindSpeedUnits.First(t => t.Id == vm.WindSpeedUnit.Id);
			vm.Responses = await weatherChecker.CheckAsync(vm.NewLocation);

			await vm.RefreshValuesAsync();
		}

		public IActionResult Error() =>
			View();
	}
}