using System.Collections.Generic;

namespace WeatherTest.WebApp.Models
{
	public class WeatherProviders
	{
		public IEnumerable<WeatherProvider> Providers { get; set; }
	}
}