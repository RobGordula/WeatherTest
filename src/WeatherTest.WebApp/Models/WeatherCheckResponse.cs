using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WeatherTest.WebApp.Models
{
	public class WeatherCheckResponse
	{
		public string Location { get; set; }

		public double Temperature { get; set; }

		public double WindSpeed { get; set; }

		public string BadRequestMessage { get; set; }

		public WeatherProvider Provider { get; set; }

		[JsonExtensionData]
		private IDictionary<string, JToken> additionalData = new Dictionary<string, JToken>();

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			foreach (var key in additionalData.Keys)
			{
				if (key.StartsWith("temp"))
					Temperature = (double)additionalData[key];
				else if (key.StartsWith("wind"))
					WindSpeed = (double)additionalData[key];
			}
		}
	}
}