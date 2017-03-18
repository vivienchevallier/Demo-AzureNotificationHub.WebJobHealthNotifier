using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using WebJobHealthNotifier.Api;
using WebJobHealthNotifier.App.Services;

namespace WebJobHealthNotifier.App
{
	[Activity(Label = "Feeds Subscription")]
	public class FeedsSubscriptionActivity : ListActivity
	{
		private bool subscriptionsChanged;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
		}

		protected override void OnResume()
		{
			base.OnResume();

			this.LoadData();
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			this.subscriptionsChanged = true;

			this.InvalidateOptionsMenu();

			base.OnListItemClick(l, v, position, id);
		}

		private async void LoadData()
		{
			using (var progressDialog = ProgressDialog.Show(this, null, "Loading Feeds Subscription....", true, true))
			{
				using (var cts = new CancellationTokenSource(12000))
				{
					progressDialog.CancelEvent += (s, ev) => { cts.Cancel(true); };

					try
					{
						var feeds = default(IList<string>);
						var subscriptions = default(IList<string>);

						await Task.Run(async () =>
						{
							using (var client = ApplicationService.GetApiClient())
							{
								feeds = await client.Feeds.GetAsync(cts.Token);
								subscriptions = await client.Subscriptions.GetAsync(ApplicationService.GetDeviceUniqueId(), cts.Token);
							}
						}, cts.Token);

						base.ListView.ChoiceMode = ChoiceMode.Multiple;

						base.ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemMultipleChoice, feeds);

						for (int i = 0; i < base.ListAdapter.Count; i++)
						{
							var feed = feeds.ElementAt(i);

							base.ListView.SetItemChecked(i, subscriptions != null && subscriptions.Contains(feed));
						}
					}
					catch (Exception ex)
					{
						if (!cts.IsCancellationRequested)
						{
							Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
						}

						Finish();
					}
					finally
					{
						progressDialog.DismissSafe(this);
					}
				}
			}
		}

		private async void Save()
		{
			using (var progressDialog = ProgressDialog.Show(this, null, "Updating Feeds Subscription....", true, false))
			{
				using (var cts = new CancellationTokenSource(8000))
				{
					try
					{
						var selectedFeeds = new List<string>();

						for (int i = 0; i < base.ListView.CheckedItemPositions.Size(); i++)
						{
							if (base.ListView.CheckedItemPositions.ValueAt(i))
							{
								selectedFeeds.Add(base.ListAdapter.GetItem(i).ToString());
							}
						}

						await Task.Run(async () =>
						{
							using (var client = ApplicationService.GetApiClient())
							{
								await client.Subscriptions.PutAsync(ApplicationService.GetDeviceUniqueId(), selectedFeeds, cts.Token);
							}
						}, cts.Token);

						this.subscriptionsChanged = false;

						this.InvalidateOptionsMenu();

						progressDialog.DismissSafe(this);

						Toast.MakeText(this, "Subscriptions saved!", ToastLength.Long).Show();
					}
					catch (Exception ex)
					{
						Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
					}
					finally
					{
						progressDialog.DismissSafe(this);
					}
				}
			}
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			menu.Add("Save").SetShowAsActionFlags(ShowAsAction.Always).SetEnabled(false);

			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnPrepareOptionsMenu(IMenu menu)
		{
			menu.FindItem(0).SetEnabled(this.subscriptionsChanged);

			return base.OnPrepareOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case 0:
					this.Save();
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
	}
}