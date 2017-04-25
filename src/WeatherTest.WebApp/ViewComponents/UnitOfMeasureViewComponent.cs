using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;
using WeatherTest.WebApp.Services;

namespace WeatherTest.WebApp.ViewComponents
{
	public class UnitOfMeasureViewComponent : ViewComponent
	{
		readonly IUnitOfMeasurementsService measurements;

		public UnitOfMeasureViewComponent(IUnitOfMeasurementsService measurements) =>
			this.measurements = measurements ?? throw new ArgumentNullException(nameof(measurements));

		public Task<IViewComponentResult> InvokeAsync(WeatherViewModel vm)
		{
			var tsc = new TaskCompletionSource<IViewComponentResult>();
			Task.Run(
				delegate
				{
					vm.TemperatureUnits = measurements.TemperatureUnits.Select(t =>
							new SelectListItem
							{
								Text = WebUtility.HtmlDecode(t.Symbol),
								Value = t.Id.ToString(),
								Selected = t.Id == vm.TemperatureUnit.Id
							});
						vm.WindSpeedUnits = measurements.WindSpeedUnits.Select(w =>
							new SelectListItem
							{
								Text = w.Symbol,
								Value = w.Id.ToString(),
								Selected = w.Id == vm.WindSpeedUnit.Id
							});

					tsc.SetResult(View(vm));
				});

			return tsc.Task;
		}
	}
}