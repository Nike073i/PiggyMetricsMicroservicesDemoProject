using RabbitMQ.Client;

namespace Core.RabbitMq
{
    public class NotificationQueueTopology : IRabbitTopology
    {
        private string _queueName = "notitficationsQueue_DefaultQueue";

        public string RoutingKey => _queueName;

        public string Exchange => string.Empty;

        public void EstablishTopology(IModel channel)
        {
            channel.QueueDeclare(_queueName, true, false, false);
        }
    }
}
