using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Pitstop.Infrastructure.Messaging;
using Serilog;

namespace CustomerEventHandler
{
    public class CustomerManager : IHostedService, IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;

        public CustomerManager(IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {
            JObject messageObject = MessageSerializer.Deserialize(message);

            Log.Information("{MessageType} - {Body}", messageType, message);

            try
            {

                switch (messageType)
                {
                    case "CustomerRegistered":
                        var messageObj = messageObject.ToObject<CustomerRegistered>();
                        Console.WriteLine($"New customer registered: {messageObj.Name}");
                        break;
                }
            }
            catch (Exception ex)
            {
                string messageId = messageObject.Property("MessageId") != null ? messageObject.Property("MessageId").Value<string>() : "[unknown]";
                Log.Error(ex, "Error while handling {MessageType} message with id {MessageId}.", messageType, messageId);
            }

            return true;
        }
    }
}