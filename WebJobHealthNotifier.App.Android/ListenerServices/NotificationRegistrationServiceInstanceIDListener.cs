using Android.App;
using Android.Util;
using Firebase.Iid;
using WebJobHealthNotifier.Api;
using WebJobHealthNotifier.App.Services;

namespace WebJobHealthNotifier.AppListenerServices
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
	public class NotificationRegistrationServiceInstanceIDListener : FirebaseInstanceIdService
	{
		public override void OnTokenRefresh()
		{
			var refreshedToken = FirebaseInstanceId.Instance.Token;

			Log.Debug(nameof(NotificationRegistrationServiceInstanceIDListener), "Refreshed token: " + refreshedToken);

			SendRegistrationToServer(refreshedToken);
		}

		private void SendRegistrationToServer(string token)
		{
			try
			{
				using (var client = ApplicationService.GetApiClient())
				{
					client.Devices.Put(ApplicationService.GetDeviceUniqueId(), token);
				}
			}
			catch
			{
				Log.Error(nameof(NotificationRegistrationServiceInstanceIDListener), $"Error in: '{nameof(SendRegistrationToServer)}'");
			}
		}
	}
}