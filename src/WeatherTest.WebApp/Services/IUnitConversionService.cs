using WeatherTest.WebApp.Models;

namespace WeatherTest.WebApp.Services
{
	public interface IUnitConversionService
	{
		double Convert(Unit from, Unit to, double value);
	}
}