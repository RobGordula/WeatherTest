using System.Collections.Generic;

namespace WeatherTest.WebApp.Models
{
	public class WeatherViewModel
	{
		public IEnumerable<WeatherCheckResponse> Responses { get; set; }
	}
}
