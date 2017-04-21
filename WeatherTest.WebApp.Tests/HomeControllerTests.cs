using Microsoft.AspNetCore.Mvc;
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
	public class HomeControllerTests
	{
		[Fact]
		public async Task Index_ReturnsAViewResult_WithTheLatestWeatherInTheCurrentLocation()
		{
			// Arrange
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
			var sut = new HomeController(weatherChecker.Object);

			// Act
			var result = await sut.Index();

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<WeatherViewModel>(viewResult.ViewData.Model);
			Assert.Equal(2, model.Responses.Count());
		}
	}
}