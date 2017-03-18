using Newtonsoft.Json;
using WebJobHealthNotifier.Entities.Notifications;
using WebJobHealthNotifier.WebJob.Models;

namespace WebJobHealthNotifier.WebJob.Extensions
{
	public static class WebJobHealthNotificationExtensions
	{
		public static string ToGcmPayload(this WebJobHealthNotification webJobHealthNotification)
		{
			var gcmPayloadModel = new GcmPayloadModel()
			{
				Data = new
				{
					Content = webJobHealthNotification
				}
			};

			return JsonConvert.SerializeObject(gcmPayloadModel);
		}
	}
}
