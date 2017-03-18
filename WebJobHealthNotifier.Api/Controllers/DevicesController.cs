using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.NotificationHubs;
using WebJobHealthNotifier.Api.Domain.Contracts;

namespace WebJobHealthNotifier.Api.Controllers
{
	public class DevicesController : ApiController
	{
		private readonly INotificationHubService notificationHubService;

		public DevicesController(INotificationHubService notificationHubService)
		{
			if (notificationHubService == null)
			{
				throw new ArgumentNullException(nameof(notificationHubService));
			}

			this.notificationHubService = notificationHubService;
		}

		// PUT api/<controller>/5
		public async Task Put(string id, [FromBody]string token)
		{
			await this.notificationHubService.UpdateToken(id, NotificationPlatform.Gcm, token);
		}
	}
}