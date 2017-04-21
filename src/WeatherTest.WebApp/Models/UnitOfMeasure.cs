using System.Collections.Generic;

namespace WeatherTest.WebApp.Models
{
	public class UnitOfMeasure
	{
		public List<TemperatureUnit> Temperature { get; set; }

		public List<string> WindSpeed { get; set; }
	}
}
