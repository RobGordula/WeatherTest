using System;
using System.Collections.Generic;
using WeatherTest.WebApp.Models.UnitOfMeasure;

namespace WeatherTest.WebApp.Services
{
	public interface IUnitOfMeasurementsService
	{
		IEnumerable<Unit> TemperatureUnits { get; }

		IEnumerable<Unit> WindSpeedUnits { get; }

		Unit Find(Guid unitId);
	}
}