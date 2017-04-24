using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherTest.WebApp.Models;

namespace WeatherTest.WebApp.ViewComponents
{
	public class UnitOfMeasureViewComponent : ViewComponent
	{
		readonly IOptions<UnitOfMeasure> supportedUnitOfMeasure;

		public UnitOfMeasureViewComponent(IOptions<UnitOfMeasure> supportedUnitOfMeasure) =>
			this.supportedUnitOfMeasure = supportedUnitOfMeasure ?? throw new ArgumentNullException(nameof(supportedUnitOfMeasure));

		public Task<IViewComponentResult> InvokeAsync(WeatherViewModel vm)
		{
			var tsc = new TaskCompletionSource<IViewComponentResult>();
			Task.Run(
				delegate
				{
					var temperatureUnits = supportedUnitOfMeasure.Value.Temperature;
					var windSpeedUnits = supportedUnitOfMeasure.Value.WindSpeed;
					vm.UnitOfMeasureViewModel = new UnitOfMeasureViewModel
					{
						TemperatureUnits = temperatureUnits
							.Select(t =>
								new SelectListItem
								{
									Text = WebUtility.HtmlDecode(t.Symbol),
									Value = t.Code,
									Selected = t.Code.Equals(vm.TemperatureUnit.Code, StringComparison.CurrentCulture)
								})
							.ToList(),
						WindSpeedUnits = windSpeedUnits
							.Select(w =>
								new SelectListItem
								{
									Text = w.Symbol,
									Value = w.Code,
									Selected = w.Code.Equals(vm.WindSpeedUnit.Code, StringComparison.CurrentCulture)
								})
							.ToList()
					};

					tsc.SetResult(View(vm));
				});

			return tsc.Task;
		}
	}
}