using System;
using Microsoft.Azure.WebJobs;

namespace WebJobHealthNotifier.WebJob
{
	// To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
	class Program
	{
		// Please set the following connection strings in app.config for this WebJob to run:
		// AzureWebJobsDashboard and AzureWebJobsStorage
		static void Main()
		{
			var config = new JobHostConfiguration();

			if (config.IsDevelopment)
			{
				config.UseDevelopmentSettings();
			}

			config.Queues.MaxPollingInterval = TimeSpan.FromSeconds(15);
			config.UseCore();
			config.UseNotificationHubs();

			var host = new JobHost(config);
			// The following code ensures that the WebJob will be running continuously
			host.RunAndBlock();
		}
	}
}
