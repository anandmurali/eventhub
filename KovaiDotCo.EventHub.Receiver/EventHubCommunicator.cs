using Microsoft.Azure.EventHubs;
using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Linq;
using KovaiDotCo.Model;
using System.Text;
using PubSub;
using KovaiDotCo.EventHub.Contracts;

namespace KovaiDotCo.EventHub
{
    /// <summary>
    /// The EventHub Communicator Class is responsible for receiving and sending events to the EventHub
    /// </summary>
    public class EventHubCommunicator : IEventHubCommunicator
    {
        #region Private Fields
        /// <summary>
        /// EventHubClient instance
        /// </summary>
        private EventHubClient _eventHubClient;

        /// <summary>
        /// PubSub Hub default instance
        /// </summary>
        private Hub _hub = Hub.Default;
        #endregion


        #region Public Methods
        /// <summary>
        /// Converts the AzureDiagnosticGridModel into AzureDiagnosticRootModel instance and sends it to the EventHub.
        /// Note: ReceiveSync method should be called first before calling SendEventAsync
        /// </summary>
        /// <param name="gridModel"></param>
        /// <returns></returns>
        public async Task SendEventAsync(AzureDiagnosticGridModel gridModel)
        {
            var data = new AzureDiagnosticRootModel();
            data.Records = new System.Collections.Generic.List<Record>();
            Record record = new Record();
            record.Time = DateTime.Now;
            record.OperationName = gridModel.OperationName;
            record.ResultSignature = gridModel.Status;
            record.Identity = new Identity();
            record.Identity.Authorization = new Authorization();
            record.Identity.Authorization.Scope = gridModel.Subscription;
            record.Identity.Claims = new Claims();
            record.Identity.Claims.HttpSchemasXmlsoapOrgWs200505IdentityClaimsEmailaddress = gridModel.EventInitiatedBy;

            data.Records.Add(record);
            await _eventHubClient.SendAsync(CreateEventData(data));
        }

        /// <summary>
        /// Creates an EventHubClient instance and creates partition receivers for all partitions
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(string connectionString)
        {
            _hub.Publish(new AppLogModel("Creating EventHubClient"));
            // Create instance of EventHubClient by passing connection string
            _eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);


            _hub.Publish(new AppLogModel("Reading partitions"));
            // Get all the partitions 
            var runtimeInformation = await _eventHubClient.GetRuntimeInformationAsync();


            _hub.Publish(new AppLogModel("Creating partition receivers"));
            // Create Partition Receivers for each PartitionIds
            var partitionReceivers = runtimeInformation.PartitionIds.Select(pId => _eventHubClient.CreateReceiver("$Default", pId, EventPosition.FromEnqueuedTime(DateTime.Today))).ToList();

            // Add Handler for each partition
            foreach (var partitionReceiver in partitionReceivers)
            {
                partitionReceiver.SetReceiveHandler(new EventHubPartitionReceiveHandler());
            }

            _hub.Publish(new AppLogModel("EventHubClient setup successfully"));
        }

        /// <summary>
        /// Safely closes the communication with the EventHub
        /// </summary>
        /// <returns></returns>
        public async Task CloseReceiver()
        {
            if (_eventHubClient != null)
            {
                await _eventHubClient.CloseAsync();
                _eventHubClient = null;
            }
        }

        /// <summary>
        /// Helper method to create instance of EventData from AzureDiagnosticRootModel instance
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private EventData CreateEventData(AzureDiagnosticRootModel data)
        {
            var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var eventData = new EventData(Encoding.UTF8.GetBytes(dataAsJson));
            return eventData;
        }
        #endregion


    }
}
