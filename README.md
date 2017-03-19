# Demo-AzureNotificationHub.WebJobHealthNotifier
Source code for the presentation http://www.vivien-chevallier.com/Articles/take-control-of-multi-platform-push-notifications-with-azure-notification-hubs

## Installation steps

### Set up Firebase Cloud Messaging

You must acquire the necessary credentials to use Google's FCM servers. See: [Firebase Console New Project](https://developer.xamarin.com/guides/android/application_fundamentals/notifications/firebase-cloud-messaging/#Setting_Up_Firebase_Cloud_Messaging)

Once the project in Firebase created you will be able to retrieve:
* A google-services.json file
* A Firebase Server Key

### Deploy resources to Azure via ARM template

In the solution, you will find an Azure Resource Group deployment project: ***WebJobHealthNotifier.AzureResourceGroup***

Update the parameter named ***notificationHubGcmApiKey*** with your ***Firebase Server Key*** and deploy.

Once the deployment successful your resource group will contain the following resources:
* App Service plan
* App Service
* Notification Hub Namespace
* Notification Hub
* Storage account

### Publish the API

In the solution, you will find the API project named: ***WebJobHealthNotifier.Api***

From Visual Studio, publish it to the App Service previously created. It will also publish the WebJob ***WebJobHealthNotifier.WebJob*** at the same time.

Once published successfuly you'll see a message like the following in the Output:  
*Web App was published successfully http://wjhnwaapii2hlstulricu2.azurewebsites.net/*

### Android Application

**1. Set API Uri**

In ***ApplicationService.cs*** set ***ApiUri*** property using the Url of the Web App previously published.

**2. Update google-services.json file**

Update the ***google-services.json*** file using the one retrieved from your Firebase Console New Project.

**3. Deploy**

Deploy to an Emulator or a device.

### Storage account

In the storage, create an Azure Queue named ***queue***.

## How to test

**1. Android Application**

Launch the application and select ***Log Token*** to register the device in Notification Hub.

Then select ***Feeds Subscription*** and subscribe to JobsFailing and or JobsSuccessful feeds.

**2. Add messages to the storage queue**

Via the Cloud Explorer in Visual Studio or your favorite explorer add messages to the queue:
* Message **with text**: will simulate a successful job and send a notifcation to Notification Hub
* Message **with no text**: will simulate a failing job and send a notifcation to Notification Hub


# Useful Links

## Azure Notification Hubs

* [Azure Notification Hubs Overview](https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-push-notification-overview)
* [Azure Notification Hubs Frequently Asked Questions](https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-push-notification-faq)
* [Azure Notification Hubs Device registration management](https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-push-notification-registration-management)

## ﻿﻿﻿﻿Azure WebJobs SDK Extensions

* [Notification Hubs SDK Extension](https://github.com/Azure/azure-webjobs-sdk-extensions#azure-notification-hubs)
* [Notification Hubs SDK Extension Samples](https://github.com/Azure/azure-webjobs-sdk-extensions/blob/dev/src/ExtensionsSample/Samples/NotificationHubSamples.cs)
* [Notification Hubs SDK Extension Sources](https://github.com/Azure/azure-webjobs-sdk-extensions/tree/master/src/WebJobs.Extensions.NotificationHubs)

## Xamarin Android

* [Remote Notifications with Firebase Cloud Messaging](https://developer.xamarin.com/guides/android/application_fundamentals/notifications/remote-notifications-with-fcm/)
* [Setting Up Firebase Cloud Messaging](https://developer.xamarin.com/guides/android/application_fundamentals/notifications/firebase-cloud-messaging/#Setting_Up_Firebase_Cloud_Messaging)