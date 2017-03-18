using System;
using Android.App;
using Android.Provider;
using WebJobHealthNotifier.Api;

namespace WebJobHealthNotifier.App.Services
{
	public static class ApplicationService
	{
		public const string ApiUri = "";

		public static IWebJobHealthNotifierApi GetApiClient()
		{
			return new WebJobHealthNotifierApi(new Uri(ApiUri));
		}

		public static string GetDeviceUniqueId()
		{
			return Settings.Secure.GetString(Application.Context.ContentResolver, Settings.Secure.AndroidId);
		}
	}
}