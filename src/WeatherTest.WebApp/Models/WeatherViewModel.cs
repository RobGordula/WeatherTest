using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WeatherTest.WebApp.Models
{
	public class WeatherViewModel
	{
		public double AverageTemperature { get; set; }

		public double AverageTemperatureBaseUnitValue =>
			Responses?.Average(t => t.TemperatureBaseUnitValue) ?? 0f;

		public string AverageTemperatureDisplay =>
			$"{AverageTemperature.ToString("F0")}{TemperatureUnit?.Symbol}";

		public double AverageWindSpeed { get; set; }

		public double AverageWindSpeedBaseUnitValue =>
			Responses?.Average(s => s.WindSpeedBaseUnitValue) ?? 0f;

		public string AverageWindSpeedDisplay =>
			$"{AverageWindSpeed.ToString("F1")} {WindSpeedUnit?.Symbol}";

		[Required]
		public string Location { get; set; }

		public Unit TemperatureUnit { get; set; }

		public Unit WindSpeedUnit { get; set; }

		public IEnumerable<WeatherCheckResponse> Responses { get; set; }

		public UnitOfMeasureViewModel UnitOfMeasureViewModel { get; set; }
	}
}