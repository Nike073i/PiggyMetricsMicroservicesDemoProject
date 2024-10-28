using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Core.RabbitMq
{
    public abstract class BaseRabbitMqTransmitter
    {
        private readonly string _channelGuid = Guid.NewGuid().ToString();

        private readonly RabbitMqChannelFactory _channelFactory;

        protected readonly IRabbitTopology Topology;

        private bool _topologyEstablished;

        protected BaseRabbitMqTransmitter(
            RabbitMqChannelFactory channelFactory,
            IRabbitTopology topology
        )
        {
            _channelFactory = channelFactory;
            Topology = topology;
        }

        private void EstablishTopology(IModel channel)
        {
            if (_topologyEstablished)
                return;

            Topology.EstablishTopology(channel);
            _topologyEstablished = true;
        }

        protected IModel GetChannel() => _channelFactory.GetOrCreateChannel(_channelGuid);

        public void Publish<T>(T message)
        {
            var channel = GetChannel();

            EstablishTopology(channel);

            IBasicProperties props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = 2;

            var json = JsonConvert.SerializeObject(message);
            var bytes = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(Topology.Exchange, Topology.RoutingKey, props, bytes);
        }
    }
}
