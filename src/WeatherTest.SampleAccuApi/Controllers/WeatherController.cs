using Microsoft.AspNetCore.Mvc;
using WeatherTest.SampleAccuApi.Models;

namespace WeatherTest.SampleAccuApi.Controllers
{
	[Route("api/[controller]")]
	public class WeatherController : Controller
	{
		private System.Random _randomNumberGenerator = new System.Random();

		[HttpGet("{where}")]
		public IActionResult Get(string where)
		{
			if (string.IsNullOrWhiteSpace(where)) return new BadRequestResult();

			return new ObjectResult(
				new AccuWeatherResult
				{
					TemperatureFahrenheit = _randomNumberGenerator.Next(32, 100),
					Where = where,
					WindSpeedMph = _randomNumberGenerator.Next(0, 32)
				}
			);
		}
	}
}
