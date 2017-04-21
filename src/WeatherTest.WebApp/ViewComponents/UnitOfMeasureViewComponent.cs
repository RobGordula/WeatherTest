using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;

namespace WeatherTest.WebApp.ViewComponents
{
	public class UnitOfMeasureViewComponent : ViewComponent
	{
		readonly IOptions<UnitOfMeasure> supportedUnitOfMeasure;

		public UnitOfMeasureViewComponent(IOptions<UnitOfMeasure> supportedUnitOfMeasure) =>
			this.supportedUnitOfMeasure = supportedUnitOfMeasure ?? throw new ArgumentNullException(nameof(supportedUnitOfMeasure));

		public Task<IViewComponentResult> InvokeAsync(string temperatureUnit, string windSpeedUnit)
		{
			var tsc = new TaskCompletionSource<IViewComponentResult>();
			Task.Run(
				delegate
				{
					var temperatureUnits = supportedUnitOfMeasure.Value.Temperature;
					var windSpeedUnits = supportedUnitOfMeasure.Value.WindSpeed;
					var vm = new UnitOfMeasureViewModel
					{
						TemperatureUnit = temperatureUnits
							.Single(u => u.Code.Equals(temperatureUnit, StringComparison.CurrentCulture)),
						TemperatureUnits = temperatureUnits
							.Select(t => new SelectListItem { Text = t.Symbol, Value = t.Code })
							.ToList(),
						WindSpeedUnit = windSpeedUnit,
						WindSpeedUnits = windSpeedUnits
							.Select(w => new SelectListItem { Text = w, Value = w })
							.ToList()
					};

					tsc.SetResult(View(vm));
				});

			return tsc.Task;
		}
	}
}