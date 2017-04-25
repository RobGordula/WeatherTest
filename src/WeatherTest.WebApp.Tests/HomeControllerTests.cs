using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
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

		Mock<IUnitOfMeasurementsService> mockMeasurements;

		Mock<IWeatherChecker> mockWeatherChecker;

		public HomeControllerTests()
		{
			mockMeasurements = mockRepo.Create<IUnitOfMeasurementsService>();
			mockWeatherChecker = mockRepo.Create<IWeatherChecker>();
		}

		[Fact]
		public async Task Index_ReturnsAViewResult_WithTheLatestWeatherInTheCurrentLocation()
		{
			// Arrange
			mockMeasurements.Setup(m => m.TemperatureUnits).Returns(TestData.Temperature);
			mockMeasurements.Setup(m => m.WindSpeedUnits).Returns(TestData.WindSpeed);

			mockWeatherChecker
				.Setup(w => w.CheckAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(TestData.Responses(mockMeasurements.Object)));

			var sut = new HomeController(mockMeasurements.Object, mockWeatherChecker.Object);

			// Act
			var result = await sut.Index();

			// Assert
			mockRepo.VerifyAll();

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<WeatherViewModel>(viewResult.ViewData.Model);
			Assert.Equal(2, model.Responses.Count());
		}


		// Given temperatures of 10c from bbc and 68f from accuweather when searching then display
		// either 15c or 59f depending on what the user has chosen
		[Theory]
		[InlineData(10.0, 68.0, "5d364ba0-4045-4dd2-b11f-bbac3b73a526", 15.0)] // C
		[InlineData(10.0, 68.0, "63220210-491a-474b-8ea3-6113d17e6b98", 59.0)] // F
		public async Task Index_ShouldDisplayAnAverageOfTemperature(
			double val1Cel,
			double val2Far,
			string unit,
			double expected)
		{
			// Arrange
			mockMeasurements.Setup(m => m.TemperatureUnits).Returns(TestData.Temperature);
			mockMeasurements.Setup(m => m.WindSpeedUnits).Returns(TestData.WindSpeed);

			var responsesTask = Task
				.FromResult(TestData.Responses(mockMeasurements.Object, val1Cel, val2Far));
			mockWeatherChecker.Setup(w => w.CheckAsync(It.IsAny<string>())).Returns(responsesTask);

			var sut = new HomeController(mockMeasurements.Object, mockWeatherChecker.Object);
			var vm = new WeatherViewModel
			{
				NewLocation = "bournemouth",
				TemperatureUnit = TestData.Temperature.Single(t => t.Id == Guid.Parse(unit)),
				WindSpeedUnit = TestData.WindSpeed.First()
			};

			// Act
			var result = await sut.Index(vm);

			// Assert
			mockRepo.VerifyAll();

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<WeatherViewModel>(viewResult.ViewData.Model);
			Assert.Equal(2, model.Responses.Count());
			Assert.Equal(expected, model.AverageTemperature.Value);
		}


		// Given wind speeds of 8kph from bbc and 10mph from accuweather when searching then display
		// either 12kph or 7.5mph depending on what the user has chosen
		[Theory]
		[InlineData(8.0, 10.0, "7a73bc9e-efc4-4f20-a117-3d676cbbfd26", 7.5)] // MPH
		[InlineData(8.0, 10.0, "d26f20e8-f7f0-448f-935a-e029299e0f8b", 12.0)] // KPH
		public async Task Index_ShouldDisplayAnAverageOfWindSpeed(
			double val1Kph,
			double val2Mph,
			string unit,
			double expected)
		{
			// Arrange
			mockMeasurements.Setup(m => m.TemperatureUnits).Returns(TestData.Temperature);
			mockMeasurements.Setup(m => m.WindSpeedUnits).Returns(TestData.WindSpeed);

			var responsesTask = Task
				.FromResult(TestData.Responses(mockMeasurements.Object, kph: val1Kph, mph: val2Mph));
			mockWeatherChecker.Setup(w => w.CheckAsync(It.IsAny<string>())).Returns(responsesTask);

			var sut = new HomeController(mockMeasurements.Object, mockWeatherChecker.Object);
			var vm = new WeatherViewModel
			{
				NewLocation = "bournemouth",
				TemperatureUnit = TestData.Temperature.First(),
				WindSpeedUnit = TestData.WindSpeed.Single(t => t.Id == Guid.Parse(unit))
			};

			// Act
			var result = await sut.Index(vm);

			// Assert
			mockRepo.VerifyAll();

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<WeatherViewModel>(viewResult.ViewData.Model);
			Assert.Equal(2, model.Responses.Count());
			Assert.InRange(model.AverageWindSpeed.Value, expected - .5, expected + .5);
		}
	}
}