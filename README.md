# netmf-ams-notifications
=======================

This is a Code Example for Azure Mobile Service (.NET Backend), a .NET Micro Framework Console and an Windows 8.1 application with Toasts


### Requirements
---

* A Microsoft Azure Account (You could start with a Trial Account and switch it later to a free one - http://azure.microsoft.com/en-us/pricing/free-trial/)
* A Windows Store Developer Account, which costs $19 for a  life time account (http://msdn.microsoft.com/en-us/library/windows/apps/hh868184.aspx)
* You do not need to have it, but I would suggest that you grab Visual Studio Community Editon which is availabe since last week for free (http://www.visualstudio.com/products/free-developer-offers-vs)


### Getting Started
---

##### The Microsoft Azure Mobile Service:

* Start a new Visual Studio Project in Visual Studio 2013 Update 4 and go to Cloud -> Azure Mobile Services. If you have already have an Microft Azure Account you also can create the Mobile Service direct from Visual Studio.
* After the creation of the Mobile Service I have deleted all the Variables, Classes which refer to the Default TodoItem which is created by default in the Mobile Service.
* For my purposes I have first added a new class called "SensorData" under the DataObjects folder which defines my Class for my sensors. The class have to inheret from "EntityData" which you get from the Mobile Services DLLs
* After the creation of the class we have to create a new "Microsoft Azure Mobile Services Table Controller" which has our Class as Model and the default MobileServiceContext as Data context class. (Screenshot: http://gadgeteer.blob.core.windows.net/netmf-blog/azure-tablecontroller.png)
* When the new Table controller is ready we are ready to implement the Notifications from the server side. So go to your Controller and in the POST request. As default it inserts the item in the Database and returns the JSON data for the created item including the id. I have changed that according to http://azure.microsoft.com/en-us/documentation/articles/mobile-services-dotnet-backend-windows-store-dotnet-get-started-push/ to the following code:
```c#
public async Task<IHttpActionResult> PostSensorData(SensorData item)
{
  SensorData current = await InsertAsync(item);

  var message = new WindowsPushMessage();
  message.XmlPayload = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                       @"<toast><visual><binding template=""ToastText01"">" +
                       @"<text id=""1"">" + "Temperature: " + item.Temperature + @"</text>" +
                       @"</binding></visual></toast>";
  try
  {
    NotificationOutcome result = await Services.Push.SendAsync(message);
    Services.Log.Info(result.State.ToString());
  }
  catch (Exception ex)
  {
    Services.Log.Error(ex.Message, null, "Push.SendAsync Error");
  }

  return CreatedAtRoute("Tables", new {id = current.Id}, current);
}
```
This inserts the item in the Database. After that we create a new Windows Push Message and add some XML to it with the data. Then we are sending the push message and then give back the HTTPResult from the Mobile Service to the client.
* As we have everything configured correctly we publish the Mobile Service to the cloud, which could take a while on the first publish.

-

##### The Windows 8.1 Store Project:

* Lets add another project to our solution which is a Blank Windows Store Application
* Install the "WindowsAzure.MobileServices" NuGet Package to get the required libraries
* Now we are creating an MobileServiceClient in the App.xaml.cs
```C#
public static readonly MobileServiceClient MobileService = new MobileServiceClient(
            "<Your Mobile Services URL>",
            "<Your Application Key>");
```
* In the MainPage there is a function called "InitNotificationsAsync()" which is called on the Button Press Event. This function requests a Push Notification Channel and then registers for notifications with our MobileServiceClient and the Push Notification Channel. You only have to do this once as long as the application is installed on your system.

-

##### The .NET Micro Framework Project:

* Now we are adding a new Project of Type "Micro Framework Console Application".
* Add a Reference to my Azure Mobile Services library from here: https://github.com/mobernberger/netmf-azure-mobile-services
* Create a new class with the same Variables as in the Mobile Services Project. You have to implement the IMobileServiceEntityData Interface from my library.
* And now lets create an Instance of the MobileServiceClient and insert an entry in our Table:
```C#
var mobileService = new MobileServiceClient(new Uri("<Your Mobile Services URL>"), "<Your Application Key>");
var sensorData = new SensorData
{
    Temperature = 24.23
};
string result = mobileService.Insert("SensorData", sensorData);
Debug.Print(result);
```

-

#### Test

* Publish your changed Azure Mobile Service with the Class and the corresponding controller and the Notification Configuration on Insert.
* To test the notifications you have to deploy the Windows 8 Application to your system. Start the application press the Registration Button and wait if the registration is successfull.
* After that start the .NET Micro Framework Console Application and wait for the Insert to Complete and you should get a notification in the moment of the Insert.


--
#### Resources:

Here are some resources for further research for you:
* Get started with Azure Mobile Services with .NET Backend: http://azure.microsoft.com/en-us/documentation/articles/mobile-services-dotnet-backend-windows-store-dotnet-get-started/
* Get started with Push Notifications: 
http://azure.microsoft.com/en-us/documentation/articles/mobile-services-dotnet-backend-windows-store-dotnet-get-started-push/
