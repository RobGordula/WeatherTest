using System.Collections.Generic;

namespace WeatherTest.WebApp.Models
{
	public class WeatherViewModel
	{
		public string TemperatureUnit { get; set; } = "Cel";

		public string WindSpeedUnit { get; set; } = "mph";

		public IEnumerable<WeatherCheckResponse> Responses { get; set; }
	}
}
