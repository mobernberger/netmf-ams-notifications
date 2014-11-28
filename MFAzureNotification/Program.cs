using System;
using Microsoft.SPOT;
using NetMFAMS43;

namespace MFAzureNotification
{
    public class Program
    {
        public static void Main()
        {
            var mobileService = new MobileServiceClient(new Uri("<Your Mobile Services URL>"),
                "<Your Mobile Services Application Key>");

            var sensorData = new SensorData
            {
                Temperature = 24.23,
                Humidity = 41.2
            };

            string result = mobileService.Insert("SensorData", sensorData);
            Debug.Print(result);
        }
    }
}