using jyu.demo.rabbitmq.Utilities.Models;
using jyu.demo.rabbitmq.Utilities.RMQ.AccessFactory;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace jyu.demo.rabbitmq.Utilities.RMQ.PubSub
{
    public class PubSubProvider : IPubSubProvider
    {
        private RMQAccessFactory _accessFactory;

        public PubSubProvider()
        {
            _accessFactory = new RMQAccessFactory();
        }

        public void PubMessageForRMQ(
            ConnectionStringRMQ_Detail conn,
            string exchangeName,
            string exchangeType,
            string routingKey,
            string queueName,
            string msg,
            Logger _logger
        )
        {
            if (string.IsNullOrEmpty(queueName)) throw new ArgumentNullException(queueName);

            var channel = _accessFactory.GetRMQConnectionEntity(conn);
            using (channel)
            {
                // 宣告一個queue，後面在publish message會跟上面建立的Exchange
                // 如果未存在在的話，後面執行時會自動建立
                channel.QueueDeclare(
                    queue: queueName, // Queue Name
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                if (!string.IsNullOrEmpty(exchangeName))
                {
                    // 這裡宣告一個Exchange，如果未存在在的話，後面執行時會自動建立
                    // Type設定為Direct，我們有明確指定Key
                    channel.ExchangeDeclare(
                        exchange: exchangeName,
                        durable: true,
                        type: exchangeType ?? ExchangeType.Direct
                    );

                    // 在這裡把上面建立的Queue跟Exchange做一個綁定動作
                    // 設定routingKey可以將Queue做一個分類收集綁定用途
                    channel.QueueBind(
                        queue: queueName,
                        exchange: exchangeName,
                        routingKey: routingKey ?? string.Empty
                    );
                }

                //var queueName = channel.QueueDeclare().QueueName;

                //string message = "My first msg for publish.";
                //var body = Encoding.UTF8.GetBytes(message);

                #region Publish message

                int count = 0;

                while (count < 3)
                {
                    string message = $"{msg} - {count}.";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: exchangeName ?? string.Empty,
                        routingKey: routingKey ?? string.Empty,
                        basicProperties: null,
                        body: body
                    );

                    _logger.Info(" [x] Sent {0}", message);

                    count++;
                }

                //channel.BasicPublish(exchange: "MyExchange",
                //                     routingKey: "MyPubMsg",
                //                     basicProperties: null,
                //                     body: body);

                //_logger.Info(" [x] Sent {0}", message);

                #endregion
            }
        }

        public void SubMessageFromRMQ(
            ConnectionStringRMQ_Detail conn,
            string queueName,
            Logger _logger
        )
        {
            if (string.IsNullOrEmpty(queueName)) throw new ArgumentNullException(queueName);

            var channel = _accessFactory.GetRMQConnectionEntity(conn);
            using (channel)
            {
                #region Consumer綁定

                _logger.Info(" [*] Waiting for message.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.Info(" [x] Received message： {0}", message);
                };
                channel.BasicConsume(
                    queue: queueName, // 設定要綁定哪一個queue
                    autoAck: true, // 測定當收到Msg後回復收到狀態
                    consumer: consumer // 綁定指定Consumer
                );

                #endregion

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
