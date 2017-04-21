using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;
using Xunit;

namespace WeatherTest.WebApp.Tests
{
	public class WeatherCheckerTests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public async Task CheckAsync_ReturnsAnEmptyCollectionOfResponses_WhenLocationIsNullOrEmpty(string location)
		{
			// Arrange
			var providersMock = new Mock<IOptions<WeatherProviders>>();
			var sut = new WeatherChecker(providersMock.Object);

			// Act
			var result = await sut.CheckAsync(location);

			// Assert
			var responses = Assert.IsType<List<WeatherCheckResponse>>(result);
			Assert.Empty(responses);
		}

		[Theory]
		[InlineData("bournemouth")]
		public async Task CheckAsync_ReturnsACollectionOfWeatherCheckResponses(string location)
		{
			// Arrange
			var providersMock = new Mock<IOptions<WeatherProviders>>();
			var weatherProviders = new WeatherProviders();
			weatherProviders.Providers.AddRange(
				new List<WeatherProvider>
				{
					new WeatherProvider { EndPoint = "http://localhost" },
					new WeatherProvider { EndPoint = "http://localhost" }
				});
			providersMock.Setup(pm => pm.Value).Returns(weatherProviders);

			var sut = new WeatherChecker(providersMock.Object);

			// Act
			var result = await sut.CheckAsync(location);

			// Assert
			var responses = Assert.IsType<List<WeatherCheckResponse>>(result);
			Assert.NotEmpty(responses);
		}
	}
}