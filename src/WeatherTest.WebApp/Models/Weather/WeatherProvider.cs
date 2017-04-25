using System;

namespace WeatherTest.WebApp.Models
{
	public class WeatherProvider
	{
		public string Name { get; set; }

		public string EndPoint { get; set; }

		public Guid TemperatureUnitId { get; set; }

		public Guid WindSpeedUnitId { get; set; }
	}
}