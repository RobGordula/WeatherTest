using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models.UnitOfMeasure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace WeatherTest.WebApp.Models
{
	public class WeatherViewModel
	{
		public string AverageTemperatureDisplay =>
			AverageTemperature?.ToString();

		public string AverageWindSpeedDisplay =>
			AverageWindSpeed?.ToString();

		public Measurement AverageTemperature { get; set; }

		public Measurement AverageWindSpeed { get; set; }

		[HiddenInput]
		public string Location { get; set; }

		[Required]
		public string NewLocation { get; set; }
		
		public Unit TemperatureUnit { get; set; }
		
		public Unit WindSpeedUnit { get; set; }

		public IEnumerable<WeatherCheckResponse> Responses { get; set; }

		public IEnumerable<SelectListItem> TemperatureUnits { get; set; }

		public IEnumerable<SelectListItem> WindSpeedUnits { get; set; }

		public async Task RefreshValuesAsync()
		{
			var respSyncTasks = Responses.Select(r => r.SyncronizeValuesAsync()).ToList();
			while (respSyncTasks.Count() > 0)
			{
				var respSyncDone = await Task.WhenAny(respSyncTasks);
				respSyncTasks.Remove(respSyncDone);
			}

			AverageTemperature = new Measurement(TemperatureUnit, Responses.Average(r => r.Temperature.BaseValue));
			AverageWindSpeed = new Measurement(WindSpeedUnit, Responses.Average(r => r.WindSpead.BaseValue));

			Location = NewLocation;
		}
	}
}