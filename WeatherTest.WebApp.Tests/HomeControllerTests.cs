using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherTest.WebApp.Controllers;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;
using Xunit;

namespace WeatherTest.WebApp.Tests
{
	public class HomeControllerTests : WebAppTests
	{
		public HomeControllerTests()
		{
			var measuresMock = Mock.Get(unitsMock.Object.Value);
			measuresMock.Setup(mm => mm.Temperature).Returns(new List<Unit> { cel, degF });
			measuresMock.Setup(mm => mm.WindSpeed).Returns(new List<Unit> { kph, mph });

			unitsMock.Setup(um => um.Value).Returns(measuresMock.Object);
		}

		[Fact]
		public async Task Index_ReturnsAViewResult_WithTheLatestWeatherInTheCurrentLocation()
		{			// Arrange
			var weatherChecker = new Mock<IWeatherChecker>();
			weatherChecker
				.Setup(wc => wc.CheckAsync("bournemouth"))
				.Returns(
					Task.FromResult(
						new List<WeatherCheckResponse>
						{
							new WeatherCheckResponse(),
							new WeatherCheckResponse()
						}
						.AsEnumerable()));

			var sut = new HomeController(weatherChecker.Object, unitsMock.Object, unitConverterMock.Object);

			// Act
			var result = await sut.Index();

			// Assert
			mockRepo.VerifyAll();

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<WeatherViewModel>(viewResult.ViewData.Model);
			Assert.Equal(2, model.Responses.Count());
		}
	}
}