using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WeatherTest.WebApp.Models
{
	public class UnitOfMeasureViewModel
	{
		public List<SelectListItem> TemperatureUnits { get; set; }

		public List<SelectListItem> WindSpeedUnits { get; set; }
	}
}