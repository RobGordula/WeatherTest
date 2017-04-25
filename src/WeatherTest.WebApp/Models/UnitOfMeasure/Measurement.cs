using Microsoft.AspNetCore.Mvc;

namespace WeatherTest.WebApp.Models.UnitOfMeasure
{
	public class Measurement
	{
		public Unit Unit { get; set; }

		public double BaseValue { get; }

		[HiddenInput]
		public double Value { get; set; }

		public Measurement()
		{

		}

		public Measurement(Unit unit, double value)
		{
			Unit = unit;
			Value = value;

			if (!Unit.IsBaseUnit)
				BaseValue = Unit.ConvertTo?.Invoke(Unit.BaseUnit, Value) ?? 0f;
			else
				BaseValue = Value;
		}

		public Measurement ConvertTo(Unit to)
		{
			Value = Unit.ConvertTo(to, Value);
			Unit = to;
			return this;
		}

		public override string ToString()
		{
			if (Unit.MeasurementId == Measure.TemperatureId)
				return $"{Value.ToString("F0")}{Unit.Symbol}";
			else if (Unit.MeasurementId == Measure.WindSpeedId)
				return $"{Value.ToString("F1")} {Unit.Symbol}";
			else
				return "Unknown measurement";
		}
	}
}