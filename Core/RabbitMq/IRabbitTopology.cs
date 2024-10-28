using RabbitMQ.Client;

namespace Core.RabbitMq
{
    public interface IRabbitTopology
    {
        void EstablishTopology(IModel channel);
        string Exchange { get; }
        string RoutingKey { get; }
    }
}
