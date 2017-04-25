using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherTest.WebApp.Models.UnitOfMeasure;
using WeatherTest.WebApp.Services;
using Xunit;

namespace WeatherTest.WebApp.Tests
{
	public class UnitOfMeasurementsServiceTests : WebAppTests
	{
		[Theory]
		[InlineData("5d364ba0-4045-4dd2-b11f-bbac3b73a526")] // in the list
		public void Find_ReturnsAnInstanceOfTheUnit(string unitId)
		{
			// Arange
			var measures = mockRepo.Create<IOptions<List<Measure>>>();
			measures.Setup(m => m.Value).Returns(TestData.Measure.ToList());

			var units = mockRepo.Create<IOptions<List<Unit>>>();
			units.Setup(u => u.Value).Returns(TestData.UnitsOfMeasure.ToList());

			var sut = new UnitOfMeasurementsService(measures.Object, units.Object);

			var guid = Guid.Parse(unitId);

			// Act
			var result = sut.Find(guid);

			// Assert
			mockRepo.VerifyAll();

			var unit = Assert.IsType<Unit>(result);
			Assert.Equal(unit.Id, guid);
		}

		[Theory]
		[InlineData("00000000-0000-0000-0000-000000000000")] // default Guid
		[InlineData("aecf2462-7e8f-4baf-a7b2-7115686bea28")] // not in the list
		public void Find_ReturnsNullIfNotInTheList(string unitId)
		{
			// Arange
			var measures = mockRepo.Create<IOptions<List<Measure>>>();
			measures.Setup(m => m.Value).Returns(TestData.Measure.ToList());

			var units = mockRepo.Create<IOptions<List<Unit>>>();
			units.Setup(u => u.Value).Returns(TestData.UnitsOfMeasure.ToList());

			var sut = new UnitOfMeasurementsService(measures.Object, units.Object);

			// Act
			var result = sut.Find(Guid.Parse(unitId));

			// Assert
			mockRepo.VerifyAll();

			Assert.Null(result);
		}
	}
}
