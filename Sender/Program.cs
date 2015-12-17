namespace Sender
{
    using System;
    using Microsoft.Azure;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    class Program
    {
        static void Main(string[] args)
        {
            // Configure queue settings.
            var queueDescription = new QueueDescription("TestQueue");
            queueDescription.MaxSizeInMegabytes = 5120;
            queueDescription.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

            // Create a new queue with custom settings.
            var connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists("TestQueue"))
            {
                namespaceManager.CreateQueue(description: queueDescription);
            }

            // Sending message to the queue
            var client = QueueClient.CreateFromConnectionString(connectionString, "TestQueue");
            for (var index = 0; index < 5; index++)
            {
                var message = new BrokeredMessage("Test Message " + index);
                message.Properties["TestProperty"] = "TestValue";
                message.Properties["Message Number"] = index;
                client.Send(message: message);
            }
        }
    }
}