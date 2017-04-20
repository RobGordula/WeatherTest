using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;

namespace WeatherTest.WebApp.Services
{
	public interface IWeatherChecker
	{
		Task<IEnumerable<WeatherCheckResponse>> CheckAsync(string location);
	}
}