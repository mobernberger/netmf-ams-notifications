using Microsoft.WindowsAzure.Mobile.Service;

namespace MFAzureNotificationMobile.DataObjects
{
    public class SensorData : EntityData
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }
}