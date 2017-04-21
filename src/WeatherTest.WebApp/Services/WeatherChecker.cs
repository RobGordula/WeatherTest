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

		public WeatherChecker(IOptions<WeatherProviders> weatherProviders) =>
			this.weatherProviders = weatherProviders ?? throw new ArgumentNullException(nameof(weatherProviders));

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

				return weatherCheckResponse;
			}
		}
	}
}