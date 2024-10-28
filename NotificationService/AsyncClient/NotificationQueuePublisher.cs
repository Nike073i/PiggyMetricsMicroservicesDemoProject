using Core.RabbitMq;

namespace NotificationService.AsyncClient
{
    public class NotificationQueuePublisher : BaseRabbitMqTransmitter
    {
        public NotificationQueuePublisher(
            RabbitMqChannelFactory channelFactory,
            IRabbitTopology topology
        )
            : base(channelFactory, topology) { }
    }
}
