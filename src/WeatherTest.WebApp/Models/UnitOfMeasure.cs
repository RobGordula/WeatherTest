using System.Collections.Generic;

namespace WeatherTest.WebApp.Models
{
	public class UnitOfMeasure
	{
		public virtual List<Unit> Temperature { get; set; }

		public virtual List<Unit> WindSpeed { get; set; }
	}
}