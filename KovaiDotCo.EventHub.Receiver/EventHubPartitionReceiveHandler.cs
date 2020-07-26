using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using KovaiDotCo.Model;
using PubSub;

namespace KovaiDotCo.EventHub
{
    /// <summary>
    /// Partition Receiver
    /// </summary>
    public class EventHubPartitionReceiveHandler : IPartitionReceiveHandler
    {
        #region Fields
        /// <summary>
        /// PubSub Hub instane
        /// </summary>
        private Hub _hub = Hub.Default;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the MaxBatchSize
        /// </summary>
        public int MaxBatchSize { get; set; }
        #endregion

        #region Constructor
        public EventHubPartitionReceiveHandler()
        {
            MaxBatchSize = 5;
        }
        #endregion

        #region IPartitionReceiveHandler Implementation
        /// <summary>
        /// The method will be called if there is any error in communicating with the event hub.
        /// Creates a new instance of AppLogModel and publishes it.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public Task ProcessErrorAsync(Exception error)
        {
            _hub.Publish(new AppLogModel("Error Received \r\n " + error.Message + "\r\n" + error.StackTrace, true));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates a AzureDiagnosticGridModel from the received event data and publishes it in the HUb.
        /// </summary>
        /// <param name="eventDatas"></param>
        /// <returns></returns>
        public Task ProcessEventsAsync(IEnumerable<EventData> eventDatas)
        {
            _hub.Publish(new AppLogModel("Received event data"));
            if (eventDatas != null)
            {
                foreach (var eventData in eventDatas)
                {

                    var jsonData = Encoding.UTF8.GetString(eventData.Body.Array);
                    _hub.Publish(new AppLogModel(jsonData));

                    var jsonSerializerSettings = new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    };

                    var rootObject = JsonConvert.DeserializeObject<Model.AzureDiagnosticRootModel>(jsonData, jsonSerializerSettings);
                    if (rootObject != null && rootObject.Records != null)
                    {
                        var list = new List<AzureDiagnosticGridModel>();
                        foreach (var record in rootObject.Records)
                        {
                            var gridModel = new AzureDiagnosticGridModel();
                            gridModel.OperationName = record.OperationName;
                            gridModel.Status = record.ResultSignature;
                            gridModel.ReceivedTime = record.Time;
                            var timeDifference = DateTime.Now - record.Time;
                            if (timeDifference.TotalMinutes < 60)
                            {
                                var totalMinutes = Math.Truncate(timeDifference.TotalMinutes);
                                gridModel.Time = $"{totalMinutes} mins ago";
                            }
                            else if (timeDifference.TotalHours < 24)
                            {
                                var totalHours = Math.Truncate(timeDifference.TotalHours);
                                gridModel.Time = $"{totalHours} hours ago";
                            }
                            else
                            {
                                var totalDays = Math.Truncate(timeDifference.TotalDays);
                                gridModel.Time = $"{totalDays} days ago";
                            }
                            gridModel.TimeStamp = record.Time.ToString("ddd MMM dd yyyy HH:mm:ss");
                            gridModel.Subscription = record.Identity?.Authorization?.Scope?.Replace("/subscriptions/", "");
                            gridModel.EventInitiatedBy = record.Identity?.Claims?.HttpSchemasXmlsoapOrgWs200505IdentityClaimsEmailaddress;

                            list.Add(gridModel);
                        }

                        if (list.Count > 0)
                        {
                            _hub.Publish(list);
                        }
                    }
                    Console.WriteLine(jsonData);
                }
            }
            return Task.CompletedTask;
        }
        #endregion
    }
}
