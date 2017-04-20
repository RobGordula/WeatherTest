namespace WeatherTest.WebApp.Models
{
	public class WeatherCheckResponse
	{
		public string Location { get; set; }

		public double Temperature { get; set; }

		public double WindSpeed { get; set; }

		public WeatherProvider Provider { get; set; }
	}
}