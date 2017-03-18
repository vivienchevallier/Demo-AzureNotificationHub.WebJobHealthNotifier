using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebJobHealthNotifier.Api.Domain.Contracts;

namespace WebJobHealthNotifier.Api.Controllers
{
	public class SubscriptionsController : ApiController
	{
		private readonly INotificationHubService notificationHubService;

		public SubscriptionsController(INotificationHubService notificationHubService)
		{
			if (notificationHubService == null)
			{
				throw new ArgumentNullException(nameof(notificationHubService));
			}

			this.notificationHubService = notificationHubService;
		}

		// GET api/<controller>
		public async Task<IEnumerable<string>> Get(string id)
		{
			return await this.notificationHubService.GetTags(id);
		}

		// PUT api/<controller>/5
		public async Task Put(string id, [FromBody]string[] values)
		{
			await this.notificationHubService.UpdateTags(id, values);
		}
	}
}