namespace SampleModule
{
    using System;
    using System.Runtime.Loader;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    using System.Text;
    using Newtonsoft.Json;

    class Program
    {
        static int counter;
        static Random rand = new Random();

        static void Main(string[] args)
        {
            Init().Wait();

            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Initializes the ModuleClient and sets up the callback to receive
        /// messages containing temperature information
        /// </summary>
        static async Task Init()
        {
            MqttTransportSettings mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            ITransportSettings[] settings = { mqttSetting };

            // Open a connection to the Edge runtime
            ModuleClient ioTHubModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
            await ioTHubModuleClient.OpenAsync();
            Console.WriteLine("IoT Hub module client initialized.");

            // Register a callback for messages that are received by the module.
            await ioTHubModuleClient.SetInputMessageHandlerAsync("input1", LogMessage, ioTHubModuleClient);

            IoT_Device deviceData = new IoT_Device();
            string serializedData = "";
            for (int i = 0; i < 100; i++)
            {
                serializedData = SendRandomTelemetry(ioTHubModuleClient, deviceData, serializedData);
            }
        }

        private static string SendRandomTelemetry(ModuleClient ioTHubModuleClient, IoT_Device deviceData, string serializedData)
        {
            System.Threading.Thread.Sleep(1000);
            var sendMessageTask = new Task(() => SendDeviceToCloudMessagesAsync(ioTHubModuleClient, serializedData));
            sendMessageTask.Start();
            sendMessageTask.Wait();
            serializedData = JsonConvert.SerializeObject(deviceData.GetDeviceData());
            return serializedData;
        }

        private static async void SendDeviceToCloudMessagesAsync(ModuleClient ioTHubModuleClient, string messagePayload)
        {
            try
            {
                var message = new Message(Encoding.ASCII.GetBytes(messagePayload));

                // Send the telemetry message  
                await ioTHubModuleClient.SendEventAsync("output1", message);
                Console.WriteLine("\n{0} > Sending message: {1}\n", DateTime.Now, messagePayload);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is called whenever the module is sent a message from the EdgeHub. 
        /// It prints all the incoming messages.
        /// </summary>
        static async Task<MessageResponse> LogMessage(Message message, object userContext)
        {
            int counterValue = Interlocked.Increment(ref counter);

            var moduleClient = userContext as ModuleClient;
            if (moduleClient == null)
            {
                throw new InvalidOperationException("UserContext doesn't contain " + "expected values");
            }

            byte[] messageBytes = message.GetBytes();
            string messageString = Encoding.UTF8.GetString(messageBytes);
            Console.WriteLine($"Received message: {counterValue}, Body: [{messageString}]");

            // Async message simulation
            await Task.CompletedTask;

            return MessageResponse.Completed;
        }
    }
}
