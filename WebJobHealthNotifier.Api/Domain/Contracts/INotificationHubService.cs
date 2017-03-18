using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;

namespace WebJobHealthNotifier.Api.Domain.Contracts
{
	public interface INotificationHubService
	{
		Task<string[]> GetTags(string deviceId);

		Task UpdateTags(string deviceId, string[] tags);

		Task UpdateToken(string deviceId, NotificationPlatform deviceType, string token);
	}
}