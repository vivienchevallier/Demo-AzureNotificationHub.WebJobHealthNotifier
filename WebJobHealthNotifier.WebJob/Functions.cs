using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions;
using Microsoft.Azure.WebJobs.Host;
using WebJobHealthNotifier.Entities.Notifications;
using WebJobHealthNotifier.WebJob.Extensions;

namespace WebJobHealthNotifier.WebJob
{
	public class Functions
	{
		// This function will get triggered/executed when a new message is written 
		// on an Azure Queue called queue.
		public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TraceWriter logger, [NotificationHub(TagExpression = "JobsSuccessful")] out Notification[] notifications)
		{
			notifications = default(Notification[]);

			try
			{
				if (string.IsNullOrEmpty(message))
				{
					throw new ArgumentNullException(nameof(message));
				}

				var notification = new WebJobHealthNotification()
				{
					Message = $"Job '{nameof(ProcessQueueMessage)}' has been completed without errors.",
					Status = WebJobHealthStatus.Success,
					Title = $"A Job has completed",
				};

				notifications = GetPlatformNotifications(notification).ToArray();
			}
			catch (Exception ex)
			{
				LogError(logger, nameof(ProcessQueueMessage), ex);
			}
		}

		/// <summary>
		/// Job triggered when an error is reported in other jobs.
		/// Called whenever 1 error occurs within a 1 minute sliding window (throttled at a maximum of 1 notification per 15 seconds).
		/// </summary>
		public static void GlobalErrorMonitorJob([ErrorTrigger("0:01:00", 1, Throttle = "0:00:15")] TraceFilter filter, TextWriter log, [NotificationHub(TagExpression = "JobsFailing")] out Notification[] notifications)
		{
			var notification = new WebJobHealthNotification()
			{
				Message = filter.GetDetailedMessage(1),
				Status = WebJobHealthStatus.Failure,
				Title = $"An error has been detected in a job",
			};

			notifications = GetPlatformNotifications(notification).ToArray();

			Console.Error.WriteLine("An error has been detected in a job.");

			log.WriteLine(filter.GetDetailedMessage(1));
		}

		private static void LogError(TraceWriter logger, string functionInError, Exception ex)
		{
			logger.Error($"An error occurred in: '{functionInError}'", ex, nameof(Functions));
		}

		private static IEnumerable<Notification> GetPlatformNotifications(WebJobHealthNotification webJobHealthNotification)
		{
			yield return new GcmNotification(webJobHealthNotification.ToGcmPayload());

			//You could add notifications for other platforms:

			//yield return new MpnsNotification(webJobHealthNotification.ToWpToastPayload());
			//yield return new MpnsNotification(webJobHealthNotification.ToWpTilePayload());
			//yield return new MpnsNotification(webJobHealthNotification.ToWpSecondaryTilePayload());
		}
	}
}
