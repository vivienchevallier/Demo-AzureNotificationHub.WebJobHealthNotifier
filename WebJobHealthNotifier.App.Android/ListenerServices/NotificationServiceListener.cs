using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using Newtonsoft.Json;
using WebJobHealthNotifier.Entities.Notifications;

namespace WebJobHealthNotifier.App.ListenerServices
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
	public class NotificationServiceListener : FirebaseMessagingService
	{
		public override void OnMessageReceived(RemoteMessage message)
		{
			Log.Debug(nameof(NotificationServiceListener), "From: " + message.From);

			if (message.Data.ContainsKey("Content"))
			{
				var webJobHealthNotification = JsonConvert.DeserializeObject<WebJobHealthNotification>(message.Data["Content"]);

				SendNotification(webJobHealthNotification);
			}
		}

		private void SendNotification(WebJobHealthNotification webJobHealthNotification)
		{
			var intent = new Intent(this, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.ClearTop);

			var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

			var notificationBuilder = new Notification.Builder(this)
				.SetSmallIcon(Resource.Drawable.Icon)
				.SetContentTitle(webJobHealthNotification.Title)
				.SetContentText(webJobHealthNotification.Message)
				.SetAutoCancel(true)
				.SetContentIntent(pendingIntent)
				.SetStyle(new Notification.BigTextStyle().BigText(webJobHealthNotification.Message));

			var notificationManager = NotificationManager.FromContext(this);
			notificationManager.Notify(0, notificationBuilder.Build());
		}
	}
}