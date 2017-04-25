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
		readonly IUnitOfMeasurementsService measurements;

		readonly List<WeatherProvider> providers;

		public WeatherChecker(
			IUnitOfMeasurementsService measurements,
			IOptions<List<WeatherProvider>> providers)
		{
			this.measurements = measurements ?? throw new ArgumentNullException(nameof(measurements));
			this.providers = providers?.Value ?? throw new ArgumentNullException(nameof(providers));
		}

		public async Task<IEnumerable<WeatherCheckResponse>> CheckAsync(string location)
		{
			var responses = new List<WeatherCheckResponse>();
			if (string.IsNullOrWhiteSpace(location))
				return responses;

			var checkTasks = providers
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
				var weatherCheckResponse = new WeatherCheckResponse(provider, location, measurements);
				try
				{
					var response = await httpClient.GetAsync($"{provider.EndPoint}/{location}");
					response.EnsureSuccessStatusCode();

					var stringResponse = await response.Content.ReadAsStringAsync();
					JsonConvert.PopulateObject(stringResponse, weatherCheckResponse);

				}
				catch (HttpRequestException ex)
				{
					weatherCheckResponse.BadRequestMessage = $"Error getting weather from {provider.Name}: {ex.Message}";
				}
				catch (Exception ex)
				{
					weatherCheckResponse.BadRequestMessage = $"Error getting weather from {provider.Name}: {ex.Message}";
				}

				return weatherCheckResponse;
			}
		}
	}
}