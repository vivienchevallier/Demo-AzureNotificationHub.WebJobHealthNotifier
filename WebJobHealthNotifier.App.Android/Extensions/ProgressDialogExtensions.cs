using Android.App;

namespace WebJobHealthNotifier.App
{
	public static class ProgressDialogExtensions
	{
		public static void DismissSafe(this ProgressDialog progressDialog, Activity activity)
		{
			//If dialog is dismissed when the activity is destroyed, an exception will throw.
			try
			{
				if (progressDialog != null && activity != null && progressDialog.IsShowing && !activity.IsDestroyed)
				{
					progressDialog.Dismiss();
				}
			}
			catch { }
		}
	}
}
