using Pitstop.Infrastructure.Messaging;

namespace CustomerEventHandler
{
    public class CustomerRegistered : Event
    {
        public string CustomerId { get; set; }

        public string Name { get; set; }
    }
}