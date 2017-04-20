using Microsoft.AspNetCore.Mvc;
using WeatherTest.SampleBbcApi.Models;

namespace WeatherTest.SampleBbcApi.Controllers
{
	[Route("api/[controller]")]
	public class WeatherController : Controller
	{
		private System.Random _randomNumberGenerator = new System.Random();

		[HttpGet("{location}")]
		public IActionResult Get(string location)
		{
			if (string.IsNullOrWhiteSpace(location)) return new BadRequestResult();

			return new ObjectResult(
				new BbcWeatherResult
				{
					Location = location,
					TemperatureCelsius = _randomNumberGenerator.Next(0, 38),
					WindSpeedKph = _randomNumberGenerator.Next(0, 32)
				}
			);
		}
	}
}
