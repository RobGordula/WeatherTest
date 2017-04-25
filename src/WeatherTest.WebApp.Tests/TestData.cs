using System;
using System.Collections.Generic;
using System.Linq;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Models.UnitOfMeasure;
using WeatherTest.WebApp.Services;

namespace WeatherTest.WebApp.Tests
{
	internal class TestData
	{
		internal static readonly Guid TemperatureId = Guid.Parse("b2700d02-052c-48ba-afa3-3f01bb87a05a");

		internal static readonly Guid WindSpeedId = Guid.Parse("36d185d9-5784-4cad-bde0-917f0754cbc1");

		internal static readonly Guid Cel = Guid.Parse("5d364ba0-4045-4dd2-b11f-bbac3b73a526");

		internal static readonly Guid Far = Guid.Parse("63220210-491a-474b-8ea3-6113d17e6b98");

		internal static readonly Guid Mph = Guid.Parse("7a73bc9e-efc4-4f20-a117-3d676cbbfd26");

		internal static readonly Guid Kph = Guid.Parse("d26f20e8-f7f0-448f-935a-e029299e0f8b");

		internal static readonly IEnumerable<Measure> Measure = new List<Measure>
		{
			new Measure
			{
				Id = TemperatureId
			},
			new Measure
			{
				Id = WindSpeedId
			}
		};

		static IEnumerable<Unit> units;

		internal static IEnumerable<Unit> UnitsOfMeasure
		{
			get
			{
				if (units == null)
				{

					units = new List<Unit>
					{
						new Unit
						{
							Id = Cel,
							MeasurementId = TemperatureId,
							IsBaseUnit = true,
							Symbol = "&deg;C",
							Scale = 1.0,
							Shift = 0.0,

						},
						new Unit
						{
							Id = Far,
							MeasurementId = TemperatureId,
							IsBaseUnit = false,
							Symbol = "&deg;F",
							Scale = 1.8,
							Shift = 32.0
						},
						new Unit
						{
							Id = Mph,
							MeasurementId = WindSpeedId,
							IsBaseUnit = false,
							Symbol = "mph",
							Scale = 0.6
						},
						new Unit
						{
							Id = Kph,
							MeasurementId = WindSpeedId,
							IsBaseUnit = true,
							Symbol = "kph",
							Scale = 1
						},
					};

					foreach (var unit in TestData.UnitsOfMeasure)
					{
						unit.SetBaseUnit(TestData.UnitsOfMeasure.Single(u => unit.MeasurementId == u.MeasurementId && u.IsBaseUnit));
						unit.SetConvertTo(TestData.Measure.Single(m => m.Id == unit.MeasurementId).ConvertFunc);
					}

				}

				return units;
			}
		}

		internal static readonly IEnumerable<Unit> Temperature = UnitsOfMeasure.Where(m => m.MeasurementId == TemperatureId);

		internal static readonly IEnumerable<Unit> WindSpeed = UnitsOfMeasure.Where(m => m.MeasurementId == WindSpeedId);

		internal static readonly IEnumerable<WeatherProvider> Providers = new List<WeatherProvider>
		{
			new WeatherProvider
			{
				EndPoint = "http://localhost",
				TemperatureUnitId = Cel,
				WindSpeedUnitId = Kph
			},
			new WeatherProvider
			{
				EndPoint = "http://localhost",
				TemperatureUnitId = Far,
				WindSpeedUnitId = Mph
			}
		};

		internal static IEnumerable<WeatherCheckResponse> Responses(
			IUnitOfMeasurementsService measurements,
			double cel = 10.0,
			double far = 68.0,
			double kph = 8.0,
			double mph = 10.0) => new List<WeatherCheckResponse>
		{
			new WeatherCheckResponse(Providers.First(), "bournemouth", measurements)
			{
				Temperature = new Measurement(Temperature.Single(t => t.Id == Cel), cel),
				WindSpead = new Measurement(WindSpeed.Single(t => t.Id == Kph), kph)
			},
			new WeatherCheckResponse(Providers.Last(), "bournemouth", measurements)
			{
				Temperature = new Measurement(Temperature.Single(t => t.Id == Far), far),
				WindSpead = new Measurement(WindSpeed.Single(t => t.Id == Mph), mph)
			}
		};
	}
}
