namespace WeatherTest.WebApp.Models
{
	public class WeatherProvider
	{
		public string Name { get; set; }

		public string EndPoint { get; set; }

		public Unit TemperatureUnit { get; set; }

		public Unit WindSpeedUnit { get; set; }
	}
}