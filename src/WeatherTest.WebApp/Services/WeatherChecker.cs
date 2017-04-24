using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;

namespace WeatherTest.WebApp.Services
{
	public class WeatherChecker : IWeatherChecker
	{
		readonly IOptions<WeatherProviders> weatherProviders;

		readonly IOptions<UnitOfMeasure> supportedUnitOfMeasure;

		readonly IUnitConversionService unitConversionService;

		public WeatherChecker(
			IOptions<WeatherProviders> weatherProviders,
			IOptions<UnitOfMeasure> supportedUnitOfMeasure,
			IUnitConversionService unitConversionService)
		{
			this.weatherProviders = weatherProviders ?? throw new ArgumentNullException(nameof(weatherProviders));
			this.supportedUnitOfMeasure =
				supportedUnitOfMeasure ?? throw new ArgumentNullException(nameof(supportedUnitOfMeasure));
			this.unitConversionService =
				unitConversionService ?? throw new ArgumentNullException(nameof(unitConversionService));
		}

		public async Task<IEnumerable<WeatherCheckResponse>> CheckAsync(string location)
		{
			var responses = new List<WeatherCheckResponse>();
			if (string.IsNullOrWhiteSpace(location))
				return responses;

			var checkTasks = weatherProviders.Value.Providers
				.Select(p => CheckFromProvider(p, location)).ToList();
			while (checkTasks.Count > 0)
			{
				var finishedCheckTask = await Task.WhenAny(checkTasks);
				checkTasks.Remove(finishedCheckTask);

				responses.Add(await finishedCheckTask);
			}

			return responses;
		}

		async Task<WeatherCheckResponse> CheckFromProvider(WeatherProvider provider, string location)
		{
			using (var httpClient = new HttpClient())
			{
				WeatherCheckResponse weatherCheckResponse = null;
				try
				{
					var response = await httpClient.GetAsync($"{provider.EndPoint}/{location}");
					response.EnsureSuccessStatusCode();

					var stringResponse = await response.Content.ReadAsStringAsync();
					weatherCheckResponse = JsonConvert.DeserializeObject<WeatherCheckResponse>(stringResponse);

				}
				catch (HttpRequestException ex)
				{
					weatherCheckResponse = new WeatherCheckResponse
					{
						BadRequestMessage = $"Error getting weather from {provider.Name}: {ex.Message}"
					};
				}

				weatherCheckResponse.Provider = provider;
				weatherCheckResponse.Location = location;
				SetResponseBaseUnitValues(weatherCheckResponse);

				return weatherCheckResponse;
			}
		}

		void SetResponseBaseUnitValues(WeatherCheckResponse response)
		{
			var temperatureBaseUnit = supportedUnitOfMeasure.Value.Temperature.Single(u => u.BaseUnit);
			var responseTemperatureUnit = response.Provider.TemperatureUnit;
			if (temperatureBaseUnit.Code == responseTemperatureUnit.Code)
				response.TemperatureBaseUnitValue = response.Temperature;
			else
				response.TemperatureBaseUnitValue = unitConversionService
					.Convert(responseTemperatureUnit, temperatureBaseUnit, response.Temperature);

			var windSpeedBaseUnit = supportedUnitOfMeasure.Value.WindSpeed.Single(u => u.BaseUnit);
			var responseWindSpeedUnit = response.Provider.WindSpeedUnit;
			if (windSpeedBaseUnit.Code == responseWindSpeedUnit.Code)
				response.WindSpeedBaseUnitValue = response.WindSpeed;
			else
				response.WindSpeedBaseUnitValue = unitConversionService
					.Convert(responseWindSpeedUnit, windSpeedBaseUnit, response.WindSpeed);
		}
	}
}