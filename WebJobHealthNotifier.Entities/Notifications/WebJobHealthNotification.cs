namespace WebJobHealthNotifier.Entities.Notifications
{
	public class WebJobHealthNotification
	{
		public string Message { get; set; }

		public WebJobHealthStatus Status { get; set; }

		public string Title { get; set; }
	}
}
