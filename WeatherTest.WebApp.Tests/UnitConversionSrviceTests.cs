using System;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;
using Xunit;

namespace WeatherTest.WebApp.Tests
{
	public class UnitConversionSrviceTests
	{
		[Theory]
		[InlineData("b2700d02-052c-48ba-afa3-3f01bb87a05a", "36d185d9-5784-4cad-bde0-917f0754cbc1")]
		public void Convert_ThrowsInvalidOperationException(Guid fromUnitId, Guid toUnitId)
		{
			// Arrange
			var from = new Unit { Id = fromUnitId };
			var to = new Unit { Id = toUnitId };

			var sut = new UnitConversionService();

			// Act

		}
	}
}
