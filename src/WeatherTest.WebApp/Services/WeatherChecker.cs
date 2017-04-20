using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;

namespace WeatherTest.WebApp.Services
{
	public class WeatherChecker : IWeatherChecker
	{
		readonly IOptions<WeatherProviders> weatherProviders;

		public WeatherChecker(IOptions<WeatherProviders> weatherProviders) =>
			this.weatherProviders = weatherProviders ?? throw new ArgumentNullException(nameof(weatherProviders));

		public Task<IEnumerable<WeatherCheckResponse>> CheckAsync(string location)
		{
			if (string.IsNullOrWhiteSpace(location))
				return Task.FromResult(Enumerable.Empty<WeatherCheckResponse>());

			throw new NotImplementedException();
		}
	}
}