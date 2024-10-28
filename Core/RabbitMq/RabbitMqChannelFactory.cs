using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Core.RabbitMq
{
    public class RabbitMqChannelFactory : IDisposable
    {
        private RabbitMqConfiguration _rabbitMqConfiguration;

        private IConnection _connection;

        private Dictionary<string, IModel> _channels = new Dictionary<string, IModel>();
        private bool _disposed;

        public RabbitMqChannelFactory(IOptions<RabbitMqConfiguration> options)
        {
            _rabbitMqConfiguration = options.Value;
        }

        public IModel GetOrCreateChannel(string name)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMqChannelFactory));

            if (_channels.TryGetValue(name, out var channel))
                return channel;

            var connection = GetConnection();
            var newChannel = connection.CreateModel();
            _channels.Add(name, newChannel);

            return newChannel;
        }

        private IConnection GetConnection()
        {
            if (_connection == null)
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(_rabbitMqConfiguration.ConnectionString)
                };

                _connection = factory.CreateConnection();
            }

            return _connection;
        }

        public void Dispose()
        {
            _disposed = true;

            var channels = _channels.Values.ToList();
            _channels.Clear();

            foreach (var channel in channels)
            {
                channel.Close();
                channel.Dispose();
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
