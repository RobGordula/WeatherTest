namespace WeatherTest.SampleBbcApi.Models
{
	public class BbcWeatherResult
    {
		public string Location { get; internal set; }
		public double TemperatureCelsius { get; internal set; }
		public double WindSpeedKph { get; internal set; }
    }
}
