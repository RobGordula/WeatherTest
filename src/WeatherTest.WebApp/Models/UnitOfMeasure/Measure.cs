using System;
using System.Collections.Generic;

namespace WeatherTest.WebApp.Models.UnitOfMeasure
{
	public class Measure
	{
		public static Guid TemperatureId =>
			Guid.Parse("b2700d02-052c-48ba-afa3-3f01bb87a05a");

		public static Guid WindSpeedId =>
			Guid.Parse("36d185d9-5784-4cad-bde0-917f0754cbc1");

		public Guid Id { get; set; }

		public string Name { get; set; }

		public Func<Unit, Unit, double, double> ConvertFunc =>
			new Dictionary<Guid, Func<Unit, Unit, double, double>>
			{
				[TemperatureId] = (f, t, v) =>
				{
					// conversion for temperature
					var val = (v - f.Shift) / f.Scale;
					return val * t.Scale + t.Shift;
				},
				[WindSpeedId] = (f, t, v) =>
				{
					// conversion for speed
					if (t.IsBaseUnit)
						return v / f.Scale;
					else
						return v * t.Scale;
				}
			}[Id];

		public override string ToString() =>
			$"{Name} - {Id}";
	}
}
