using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;

namespace WeatherTest.WebApp.Tests
{
	public abstract class WebAppTests
	{
		protected readonly Unit cel;

		protected readonly Unit degF;

		protected readonly Unit kph;

		protected readonly Unit mph;

		protected readonly WeatherProviders providers;

		protected readonly MockRepository mockRepo;

		protected readonly Mock<IOptions<UnitOfMeasure>> unitsMock;

		protected readonly Mock<IUnitConversionService> unitConverterMock;

		public WebAppTests()
		{
			mockRepo = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

			cel = new Unit { BaseUnit = true, Code = "Cel", Id = 1 };
			degF = new Unit { BaseUnit = false, Code = "degF", Id = 1 };
			kph = new Unit { BaseUnit = true, Code = "KPH", Id = 2 };
			mph = new Unit { BaseUnit = false, Code = "MPH", Id = 2 };

			providers = new WeatherProviders();
			providers.Providers.AddRange(
				new List<WeatherProvider>
				{
					new WeatherProvider
					{
						EndPoint = "http://localhost",
						TemperatureUnit = cel,
						WindSpeedUnit = kph
					},
					new WeatherProvider
					{
						EndPoint = "http://localhost",
						TemperatureUnit = degF,
						WindSpeedUnit = mph
					}
				});

			unitsMock = mockRepo.Create<IOptions<UnitOfMeasure>>();

			unitConverterMock = mockRepo.Create<IUnitConversionService>();
		}
	}
}
