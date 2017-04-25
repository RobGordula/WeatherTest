using Microsoft.AspNetCore.Mvc;
using System;

namespace WeatherTest.WebApp.Models.UnitOfMeasure
{
	public class Unit
	{
		public Unit BaseUnit { get; private set; }

		public bool IsBaseUnit { get; set; }

		public Guid Id { get; set; }

		[HiddenInput]
		public Guid MeasurementId { get; set; }

		public string Code { get; set; }

		[HiddenInput]
		public string Symbol { get; set; }

		public double Scale { get; set; }

		public double Shift { get; set; }

		public Func<Unit, double, double> ConvertTo { get; private set; }

		public void SetConvertTo(Func<Unit, Unit, double, double> func) =>
			ConvertTo = (to, value) => func(this, to, value);

		public void SetBaseUnit(Unit unit) =>
			BaseUnit = unit;
	}
}