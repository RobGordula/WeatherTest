using System;
using System.Collections.Generic;
using System.Linq;
using WeatherTest.WebApp.Models;

namespace WeatherTest.WebApp.Services
{
	public class UnitConversionService : IUnitConversionService
	{
		Dictionary<int, Func<Unit, Unit, double, double>> convert =
			new Dictionary<int, Func<Unit, Unit, double, double>>
			{
				[1] = ConvertTemperature,
				[2] = ConvertWindSpeed
			};

		public double Convert(Unit from, Unit to, double value)
		{
			if (from.UnitId != to.UnitId)
				throw new InvalidOperationException($"Conversion from incompatible units: {from.Code} to {to.Code}");
			if (!convert.Keys.Contains(from.UnitId))
				throw new InvalidOperationException($"Conversion not supported: {from.Code}");


			return convert[from.UnitId](from, to, value);
		}

		static double ConvertTemperature(Unit from, Unit to, double value)
		{
			var val = (value - from.Shift) / from.Scale;
			return val * to.Scale + to.Shift;
		}

		static double ConvertWindSpeed(Unit from, Unit to, double value)
		{
			if (to.BaseUnit)
				return value / from.Scale;
			else
				return value * to.Scale;
		}
	}
}
