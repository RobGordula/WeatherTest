using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeatherTest.WebApp.Models
{
	public class UnitOfMeasureViewModel
	{
		[Display(Name = "Unit for Temperature")]
		public TemperatureUnit TemperatureUnit { get; set; }

		public List<SelectListItem> TemperatureUnits { get; set; }

		[Display(Name = "Unit for Wind Speed")]
		public string WindSpeedUnit { get; set; }

		public List<SelectListItem> WindSpeedUnits { get; set; }
	}
}