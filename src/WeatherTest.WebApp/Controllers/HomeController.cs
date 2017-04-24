using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;

namespace WeatherTest.WebApp.Controllers
{
	public class HomeController : Controller
	{
		readonly IOptions<UnitOfMeasure> supportedUnitOfMeasure;

		readonly IWeatherChecker weatherChecker;

		readonly IUnitConversionService unitConversionService;

		public HomeController(
			IWeatherChecker weatherChecker,
			IOptions<UnitOfMeasure> supportedUnitOfMeasure,
			IUnitConversionService unitConversionService)
		{
			this.supportedUnitOfMeasure =
				supportedUnitOfMeasure ?? throw new ArgumentNullException(nameof(supportedUnitOfMeasure));
			this.weatherChecker = weatherChecker ?? throw new ArgumentNullException(nameof(weatherChecker));
			this.unitConversionService =
				unitConversionService ?? throw new ArgumentNullException(nameof(unitConversionService));
		}

		[HttpGet("")]
		public async Task<IActionResult> Index()
		{
			var vm = new WeatherViewModel
			{
				Location = "bournemouth",
				TemperatureUnit = new Unit { Code = "Cel" },
				WindSpeedUnit = new Unit { Code = "MPH" }
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
			vm.TemperatureUnit = supportedUnitOfMeasure.Value.Temperature
				.Single(t => t.Code.Equals(vm.TemperatureUnit.Code, StringComparison.CurrentCulture));
			vm.WindSpeedUnit = supportedUnitOfMeasure.Value.WindSpeed
				.Single(w => w.Code.Equals(vm.WindSpeedUnit.Code, StringComparison.CurrentCulture));
			vm.Responses = await weatherChecker.CheckAsync(vm.Location);

			ConvertViewModelAverageUnit(vm);
		}

		void ConvertViewModelAverageUnit(WeatherViewModel vm)
		{
			var temperatureBaseUnit = supportedUnitOfMeasure.Value.Temperature.Single(u => u.BaseUnit);
			var vmTemperatureUnit = vm.TemperatureUnit;
			if (temperatureBaseUnit.Code == vmTemperatureUnit.Code)
				vm.AverageTemperature = vm.AverageTemperatureBaseUnitValue;
			else
				vm.AverageTemperature = unitConversionService
					.Convert(temperatureBaseUnit, vmTemperatureUnit, vm.AverageTemperatureBaseUnitValue);

			var windSpeedBaseUnit = supportedUnitOfMeasure.Value.WindSpeed.Single(u => u.BaseUnit);
			var vmWindSpeedUnit = vm.WindSpeedUnit;
			if (windSpeedBaseUnit.Code == vmWindSpeedUnit.Code)
				vm.AverageWindSpeed = vm.AverageWindSpeedBaseUnitValue;
			else
				vm.AverageWindSpeed = unitConversionService
					.Convert(windSpeedBaseUnit, vmWindSpeedUnit, vm.AverageWindSpeedBaseUnitValue);
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
