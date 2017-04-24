using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;
using Xunit;

namespace WeatherTest.WebApp.Tests
{
	public class WeatherCheckerTests : WebAppTests
	{
		protected readonly Mock<IOptions<WeatherProviders>> providersMock;

		public WeatherCheckerTests() =>
			providersMock = mockRepo.Create<IOptions<WeatherProviders>>();

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public async Task CheckAsync_ReturnsAnEmptyCollectionOfResponses_WhenLocationIsNullOrEmpty(string location)
		{
			// Arrange
			var sut = new WeatherChecker(providersMock.Object, unitsMock.Object, unitConverterMock.Object);

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
			var measuresMock = Mock.Get(unitsMock.Object.Value);
			measuresMock.Setup(mm => mm.Temperature).Returns(new List<Unit> { cel, degF });
			measuresMock.Setup(mm => mm.WindSpeed).Returns(new List<Unit> { kph, mph });

			unitsMock.Setup(um => um.Value).Returns(measuresMock.Object);

			providersMock.Setup(pm => pm.Value).Returns(providers);

			unitConverterMock
				.Setup(ucs => ucs.Convert(It.IsAny<Unit>(), It.IsAny<Unit>(), It.IsAny<double>()))
				.Returns(It.IsAny<double>());

			var sut = new WeatherChecker(providersMock.Object, unitsMock.Object, unitConverterMock.Object);

			// Act
			var result = await sut.CheckAsync(location);

			// Assert
			mockRepo.VerifyAll();

			var responses = Assert.IsType<List<WeatherCheckResponse>>(result);
			Assert.NotEmpty(responses);
		}
	}
}