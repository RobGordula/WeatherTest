namespace WeatherTest.SampleAccuApi.Models
{
	public class AccuWeatherResult
    {
		public double TemperatureFahrenheit { get; internal set; }
		public string Where { get; internal set; }
		public double WindSpeedMph { get; internal set; }
    }
}
