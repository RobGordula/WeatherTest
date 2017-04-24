namespace WeatherTest.WebApp.Models
{
	public class Unit
	{
		public bool BaseUnit { get; set; }

		public int UnitId { get; set; }

		public string Code { get; set; }

		public string Symbol { get; set; }

		public double Scale { get; set; }

		public double Shift { get; set; }
	}
}