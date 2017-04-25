using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;
using Xunit;

namespace WeatherTest.WebApp.Tests
{
	public class WeatherCheckerTests : WebAppTests
	{
		Mock<IUnitOfMeasurementsService> mockMeasurements;

		Mock<IOptions<List<WeatherProvider>>> mockProviders;

		public WeatherCheckerTests()
		{
			mockMeasurements = mockRepo.Create<IUnitOfMeasurementsService>();
			mockProviders = mockRepo.Create<IOptions<List<WeatherProvider>>>();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public async Task CheckAsync_ReturnsAnEmptyCollectionOfResponses_WhenLocationIsNullOrEmpty(string location)
		{
			// Arrange
			var sut = new WeatherChecker(mockMeasurements.Object, mockProviders.Object);

			// Act
			var result = await sut.CheckAsync(location);

			// Assert
			mockRepo.VerifyAll();

			var responses = Assert.IsType<List<WeatherCheckResponse>>(result);
			Assert.Empty(responses);
		}

		[Theory]
		[InlineData("bournemouth")]
		public async Task CheckAsync_ReturnsACollectionOfWeatherCheckResponses(string location)
		{
			// Arrange
			mockMeasurements.Setup(mm => mm.TemperatureUnits).Returns(TestData.Temperature);
			mockMeasurements.Setup(mm => mm.WindSpeedUnits).Returns(TestData.WindSpeed);

			mockProviders.Setup(p => p.Value).Returns(TestData.Providers.Take(1).ToList());

			var sut = new WeatherChecker(mockMeasurements.Object, mockProviders.Object);

			// Act
			var result = await sut.CheckAsync(location);

			// Assert
			mockRepo.VerifyAll();

			var responses = Assert.IsType<List<WeatherCheckResponse>>(result);
			Assert.NotEmpty(responses);
		}
	}
}