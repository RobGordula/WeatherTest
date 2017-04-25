using Moq;
using System.Linq;

namespace WeatherTest.WebApp.Tests
{
	public abstract class WebAppTests
	{
		protected readonly MockRepository mockRepo;

		public WebAppTests()
		{
			mockRepo = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
		}
	}
}
