namespace Receiver
{
    using System;
    using Microsoft.Azure;
    using Microsoft.ServiceBus.Messaging;

    class Program
    {
        static void Main(string[] args)
        {
            // Connect to a service
            var connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var client = QueueClient.CreateFromConnectionString(connectionString: connectionString, path: "TestQueue");

            // Configure the callback options
            var options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            // Callback to handle received messages
            client.OnMessage(message =>
                {
                    try
                    {
                        // Process message from the queue
                        Console.WriteLine("Body: " + message.GetBody<string>());
                        Console.WriteLine("MessageID: " + message.MessageId);
                        Console.WriteLine("Test Property: " + message.Properties["TestProperty"]);
                        message.Complete();
                    }
                    catch(Exception)
                    {
                        message.Abandon();
                    }
                }, options);
        }
    }
}
