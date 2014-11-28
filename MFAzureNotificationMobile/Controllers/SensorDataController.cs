using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using MFAzureNotificationMobile.DataObjects;
using MFAzureNotificationMobile.Models;
using Microsoft.ServiceBus.Notifications;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MFAzureNotificationMobile.Controllers
{
    public class SensorDataController : TableController<SensorData>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<SensorData>(context, Request, Services);
        }

        // GET tables/SensorData
        public IQueryable<SensorData> GetAllSensorData()
        {
            return Query();
        }

        // GET tables/SensorData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<SensorData> GetSensorData(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/SensorData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<SensorData> PatchSensorData(string id, Delta<SensorData> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/SensorData
        public async Task<IHttpActionResult> PostSensorData(SensorData item)
        {
            SensorData current = await InsertAsync(item);

            // Create a WNS native toast.
            var message = new WindowsPushMessage();
            // Define the XML paylod for a WNS native toast notification 
            // that contains the text of the inserted item.
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

        // DELETE tables/SensorData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSensorData(string id)
        {
            return DeleteAsync(id);
        }
    }
}