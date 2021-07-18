using jyu.demo.rabbitmq.Utilities.Models;
using NLog;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace jyu.demo.rabbitmq.Utilities.RMQ.PubSub
{
    public interface IPubSubProvider
    {
        void PubMessageForRMQ(
            ConnectionStringRMQ_Detail conn,
            string exchangeName,
            string exchangeType,
            string routingKey,
            string queueName,
            string msg,
            Logger _logger
        );

        void SubMessageFromRMQ(
            ConnectionStringRMQ_Detail conn,
            string queueName,
            Logger _logger
        );
    }
}
