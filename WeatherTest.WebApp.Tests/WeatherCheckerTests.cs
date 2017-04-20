using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;
using Xunit;

namespace WeatherTest.WebApp.Tests
{
	public class WeatherCheckerTests
	{
		[Fact]
		public async Task CheckAsync_ReturnsAnEmptyCollectionOfResponses_WhenLocationIsNullOrEmpty()
		{
			// Arrange
			var providersMock = new Mock<IOptions<WeatherProviders>>();
			var sut = new WeatherChecker(providersMock.Object);

			// Act
			var result = await sut.CheckAsync(string.Empty);

			// Assert
			var responses = Assert.IsType<WeatherCheckResponse[]>(result);
			Assert.Empty(responses);
		}

	}
}