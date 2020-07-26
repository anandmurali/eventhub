using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KovaiDotCo.Model;

namespace KovaiDotCo.EventHub.Contracts
{
    /// <summary>
    /// Defines the contract for communicating with the event hub
    /// </summary>
    public interface IEventHubCommunicator
    {
        /// <summary>
        /// Converts the AzureDiagnosticGridModel into AzureDiagnosticRootModel instance and sends it to the EventHub.
        /// Note: ReceiveSync method should be called first before calling SendEventAsync
        /// </summary>
        /// <param name="gridModel"></param>
        /// <returns></returns>
        Task CloseReceiver();

        /// <summary>
        /// Creates an EventHubClient instance and creates partition receivers for all partitions
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        Task ReceiveAsync(string connectionString);

        /// <summary>
        /// Safely closes the communication with the EventHub
        /// </summary>
        /// <returns></returns>
        Task SendEventAsync(AzureDiagnosticGridModel gridModel);
    }
}
