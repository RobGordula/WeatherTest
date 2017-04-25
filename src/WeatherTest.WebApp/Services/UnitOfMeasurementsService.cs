using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherTest.WebApp.Models.UnitOfMeasure;

namespace WeatherTest.WebApp.Services
{
	public class UnitOfMeasurementsService : IUnitOfMeasurementsService
	{
		readonly IEnumerable<Measure> measures;

		readonly IEnumerable<Unit> units;

		public IEnumerable<Unit> TemperatureUnits =>
			units.Where(u => u.MeasurementId == Measure.TemperatureId);

		public IEnumerable<Unit> WindSpeedUnits =>
			units.Where(u => u.MeasurementId == Measure.WindSpeedId);

		public UnitOfMeasurementsService(
			IOptions<List<Measure>> supportedMeasures,
			IOptions<List<Unit>> supportedUnits)
		{
			measures = supportedMeasures?.Value
				?? throw new ArgumentNullException(nameof(supportedMeasures));
			units = supportedUnits?.Value
				?? throw new ArgumentNullException(nameof(supportedUnits));

			foreach (var unit in units)
			{
				var baseUnit = units.Single(u => u.MeasurementId == unit.MeasurementId && u.IsBaseUnit);
				unit.SetBaseUnit(baseUnit);
				unit.SetConvertTo(measures.Single(m => m.Id == unit.MeasurementId).ConvertFunc);
			}
		}

		public Unit Find(Guid unitId)
		{
			if (unitId == null)
				return null;

			return units.SingleOrDefault(u => u.Id == unitId);
		}
	}
}
