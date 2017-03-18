using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using WebJobHealthNotifier.Api.Domain.Contracts;

namespace WebJobHealthNotifier.Api.Domain.Services
{
	public class NotificationHubService : INotificationHubService
	{
		private readonly NotificationHubClient notificationHubClient;

		private const string NotificationHubConnectionStringAppSettingsName = "wjhn:NotificationHubConnectionString";
		private const string NotificationHubNameAppSettingsName = "wjhn:NotificationHubName";

		public NotificationHubService()
		{
			var notificationHubConnectionString = CloudConfigurationManager.GetSetting(NotificationHubConnectionStringAppSettingsName);
			var notificationHubName = CloudConfigurationManager.GetSetting(NotificationHubNameAppSettingsName);

			if (string.IsNullOrEmpty(notificationHubConnectionString))
			{
				throw new System.Configuration.SettingsPropertyNotFoundException($"App setting named {NotificationHubConnectionStringAppSettingsName} not found.");
			}

			if (string.IsNullOrEmpty(notificationHubName))
			{
				throw new System.Configuration.SettingsPropertyNotFoundException($"App setting named {NotificationHubNameAppSettingsName} not found.");
			}

			this.notificationHubClient = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnectionString, notificationHubName);
		}

		public async Task<string[]> GetTags(string deviceId)
		{
			var installation = await this.TryGetInstallation(deviceId);

			if (installation == null)
			{
				throw new Exception("Device installation not found.");
			}

			return installation.Tags?.ToArray();
		}

		public async Task UpdateTags(string deviceId, string[] tags)
		{
			var installation = await this.TryGetInstallation(deviceId);

			if (installation == null)
			{
				throw new Exception("Device installation not found.");
			}

			installation.Tags = tags;

			await this.notificationHubClient.CreateOrUpdateInstallationAsync(installation);
		}

		public async Task UpdateToken(string deviceId, NotificationPlatform deviceType, string token)
		{
			var installation = await this.TryGetInstallation(deviceId);

			if (installation == null)
			{
				installation = new Installation()
				{
					InstallationId = deviceId,
					Platform = deviceType,
				};
			}

			installation.PushChannel = token;

			await this.notificationHubClient.CreateOrUpdateInstallationAsync(installation);
		}

		private async Task<Installation> TryGetInstallation(string deviceId)
		{
			try
			{
				return await this.notificationHubClient.GetInstallationAsync(deviceId);
			}
			catch (MessagingEntityNotFoundException)
			{
				//The device was not found on notification hub
				return null;
			}
			catch (AggregateException ex)
			{
				if (ex.InnerExceptions.Any(iex => iex is MessagingEntityNotFoundException))
				{
					return null;
				}

				throw;
			}
		}
	}
}
