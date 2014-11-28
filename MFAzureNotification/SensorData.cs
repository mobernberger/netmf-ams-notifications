using NetMFAMS43;

namespace MFAzureNotification
{
    internal class SensorData : IMobileServiceEntityData
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public string id { get; set; }
    }
}