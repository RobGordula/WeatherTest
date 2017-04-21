using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;

namespace WeatherTest.WebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly IWeatherChecker weatherChecker;

		public HomeController(IWeatherChecker weatherChecker) =>
			this.weatherChecker = weatherChecker ?? throw new ArgumentNullException(nameof(weatherChecker));

		[Route("")]
		public async Task<IActionResult> Index()
		{
			var vm = new WeatherViewModel
			{
				Responses = await weatherChecker.CheckAsync("bournemouth")
			};

			return View(vm);
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
