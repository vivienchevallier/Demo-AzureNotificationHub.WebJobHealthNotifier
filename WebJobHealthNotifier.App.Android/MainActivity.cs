using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Common;
using Android.OS;
using Android.Util;
using Android.Widget;
using Firebase.Iid;
using WebJobHealthNotifier.Api;
using WebJobHealthNotifier.App.Services;

namespace WebJobHealthNotifier.App
{
	[Activity(MainLauncher = true)]
	public class MainActivity : Activity
	{
		private Button feedsSubscriptionButton;
		private Button logTokenButton;
		private TextView msgText;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			this.msgText = FindViewById<TextView>(Resource.Id.msgText);
			this.feedsSubscriptionButton = FindViewById<Button>(Resource.Id.feedsSubscriptionButton);
			this.logTokenButton = FindViewById<Button>(Resource.Id.logTokenButton);

			this.feedsSubscriptionButton.Click += FeedsSubscriptionButton_Click;
			this.logTokenButton.Click += LogTokenButton_Click;

			IsPlayServicesAvailable();
			IsApiUriSet();
		}

		private void FeedsSubscriptionButton_Click(object sender, EventArgs e)
		{
			this.StartActivity(typeof(FeedsSubscriptionActivity));
		}

		private async void LogTokenButton_Click(object sender, EventArgs e)
		{
			Log.Debug(nameof(MainActivity), "InstanceID token: " + FirebaseInstanceId.Instance.Token);

			using (var progressDialog = ProgressDialog.Show(this, null, "Sending device Firebase token....", true, false))
			{
				this.feedsSubscriptionButton.Enabled = this.logTokenButton.Enabled = false;

				using (var cts = new CancellationTokenSource(8000))
				{
					try
					{
						await Task.Run(async () =>
						{
							using (var client = ApplicationService.GetApiClient())
							{
								await client.Devices.PutAsync(ApplicationService.GetDeviceUniqueId(), FirebaseInstanceId.Instance.Token, cts.Token);
							}
						}, cts.Token);

						progressDialog.DismissSafe(this);

						Toast.MakeText(this, "Token logged!", ToastLength.Long).Show();
					}
					catch (Exception ex)
					{
						Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
					}
					finally
					{
						this.feedsSubscriptionButton.Enabled = this.logTokenButton.Enabled = true;

						progressDialog.DismissSafe(this);
					}
				}
			}
		}

		private bool IsApiUriSet()
		{
			if (string.IsNullOrEmpty(ApplicationService.ApiUri))
			{
				msgText.Text = "Warning Api Uri not set. Please set ApiUri property in ApplicationService.";

				this.feedsSubscriptionButton.Enabled = this.logTokenButton.Enabled = false;

				return false;
			}
			else
			{
				return true;
			}
		}

		private bool IsPlayServicesAvailable()
		{
			var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);

			if (resultCode != ConnectionResult.Success)
			{
				if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
				{
					msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
				}
				else
				{
					msgText.Text = "This device is not supported.";
					Finish();
				}

				return false;
			}
			else
			{
				msgText.Text = "Google Play Services is available.";

				return true;
			}
		}
	}
}

