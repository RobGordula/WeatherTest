using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models.UnitOfMeasure;
using WeatherTest.WebApp.Services;

namespace WeatherTest.WebApp.Models
{
	public class WeatherCheckResponse
	{
		Unit temperatureUnit;

		Unit windSpeedUnit;

		public string Location { get; }

		public Measurement Temperature { get; set; }

		public Measurement WindSpead { get; set; }

		public string BadRequestMessage { get; set; }

		public WeatherProvider Provider { get; }

		[JsonExtensionData]
		IDictionary<string, JToken> additionalData = new Dictionary<string, JToken>();

		public WeatherCheckResponse(WeatherProvider provider, string location, IUnitOfMeasurementsService measurements)
		{
			Provider = provider;
			Location = location;

			temperatureUnit = measurements.TemperatureUnits.Single(t => t.Id == Provider.TemperatureUnitId);
			windSpeedUnit = measurements.WindSpeedUnits.Single(w => w.Id == provider.WindSpeedUnitId);
		}

		[OnDeserialized]
		void OnDeserialized(StreamingContext context)
		{
			foreach (var key in additionalData.Keys)
			{
				if (key.StartsWith("temp"))
					Temperature = new Measurement(temperatureUnit, (double)additionalData[key]);
				else if (key.StartsWith("wind"))
					WindSpead = new Measurement(windSpeedUnit, (double)additionalData[key]);
			}
		}

		public Task SyncronizeValuesAsync()
		{
			var tsc = new TaskCompletionSource<object>();
			Task.Run(delegate
			{
				if (Temperature.Unit.Id != Provider.TemperatureUnitId)
					Temperature = Temperature.ConvertTo(temperatureUnit);

				if (WindSpead.Unit.Id != Provider.WindSpeedUnitId)
					WindSpead = WindSpead.ConvertTo(windSpeedUnit);

				tsc.SetResult(null);
			});

			return tsc.Task;
		}
	}
}