using Microsoft.Extensions.DependencyInjection;
using jyu.demo.rabbitmq.Init.InitializeSettings;
using jyu.demo.rabbitmq.Utilities.ServiceCollectionProvider;
using NLog;
using System;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using System.Text;
using RabbitMQ.Client.Events;
using jyu.demo.rabbitmq.Utilities.Models;
using jyu.demo.rabbitmq.Utilities.RMQ.PubSub;

namespace jyu.demo.rabbitmq.App
{
    class Program
    {
        private static Logger _logger;
        private static IServiceProvider _sp;
        private static IConfiguration _config;
        private static ConnectionStringRMQ _connectionStringRMQ;

        static void Main(string[] args)
        {
            #region App init

            ServiceCollectionProvider serviceCollectionProvider = new ServiceCollectionProvider();
            // 載入DI服務容器實體
            _sp = serviceCollectionProvider.GetServicesExtensionEntity();
            // 取得組態設定檔Provider
            var appSettings = _sp.GetService<ILoadSettings>();

            // 載入NLog初始化
            _logger = appSettings.NLogInitialize();
            _config = appSettings.GetConfiguration();

            // RMQ連線資訊強型別模型綁定
            _connectionStringRMQ = _config.GetSection(
                "ConnectionStringRMQ"
            ).Get<ConnectionStringRMQ>();

            #endregion

            #region Publish / Subscription Service Provider

            var rmqPubSubProvider = _sp.GetService<IPubSubProvider>();

            // Publish msg
            rmqPubSubProvider.PubMessageForRMQ(
                conn: _connectionStringRMQ.defaultRMQ,
                exchangeName: "MyExchange",
                exchangeType: ExchangeType.Direct,
                routingKey: "MyRouting",
                queueName: "MyPubMsg",
                msg: "My first msg for publish",
                _logger
            );

            // Received message
            rmqPubSubProvider.SubMessageFromRMQ(
                _connectionStringRMQ.defaultRMQ,
                queueName: "MyPubMsg",
                _logger
            );

            #endregion

            // Send / Get Queue Sample
            //SendHelloWordForRMQ(_connectionStringRMQ.defaultRMQ);
            //GetHelloWordForRMQ(_connectionStringRMQ.defaultRMQ);

            // Publish / Subscription for RMQ
            //PubSubMessageForRMQ(_connectionStringRMQ.defaultRMQ);

            _logger.Info("The app process was stopped.");
        }

        /// <summary>
        /// Publish / Subscription for RMQ
        /// </summary>
        public static void PubSubMessageForRMQ(
            ConnectionStringRMQ_Detail conn
        )
        {
            // 建立RMQ連線Factory
            var factory = new ConnectionFactory()
            {
                HostName = conn.HostName,
                UserName = conn.UserName,
                Password = conn.Password,
                Port = conn.Port,
                VirtualHost = conn.VirtualHost
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // 這裡宣告一個Exchange，如果未存在在的話，後面執行時會自動建立
                // Type設定為Direct，我們有明確指定Key
                channel.ExchangeDeclare(
                    exchange: "MyExchange",
                    durable: true,
                    type: ExchangeType.Direct
                );

                // 宣告一個queue，後面在publish message會跟上面建立的Exchange
                // 如果未存在在的話，後面執行時會自動建立
                channel.QueueDeclare(
                    queue: "MyPubMsg", // Queue Name
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                // 在這裡把上面建立的Queue跟Exchange做一個綁定動作
                // 設定routingKey可以將Queue做一個分類收集綁定用途
                channel.QueueBind(
                    queue: "MyPubMsg",
                    exchange: "MyExchange",
                    routingKey: "MyPubMsg"
                );

                //var queueName = channel.QueueDeclare().QueueName;

                string message = "My first msg for publish.";
                var body = Encoding.UTF8.GetBytes(message);

                #region Publish message

                channel.BasicPublish(exchange: "MyExchange",
                                     routingKey: "MyPubMsg",
                                     basicProperties: null,
                                     body: body);

                _logger.Info(" [x] Sent {0}", message);

                #endregion

                #region Consumer綁定

                _logger.Info(" [*] Waiting for message.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received message： {0}", message);
                };
                channel.BasicConsume(
                    queue: "MyPubMsg", // 設定要綁定哪一個queue
                    autoAck: true, // 測定當收到Msg後回復收到狀態
                    consumer: consumer // 綁定指定Consumer
                );

                #endregion

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        public static void SendHelloWordForRMQ(
             ConnectionStringRMQ_Detail conn
        )
        {
            var factory = new ConnectionFactory()
            {
                HostName = conn.HostName,
                UserName = conn.UserName,
                Password = conn.Password,
                Port = conn.Port,
                VirtualHost = conn.VirtualHost
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // 宣告一個queue
                // 如果未存在在的話，後面執行時會自動建立
                channel.QueueDeclare(queue: "hello",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                // 發送一個訊息至queue
                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);

                _logger.Info(" [x] Sent {0}", message);
            }
        }

        public static void GetHelloWordForRMQ(
             ConnectionStringRMQ_Detail conn
        )
        {
            var factory = new ConnectionFactory()
            {
                HostName = conn.HostName,
                UserName = conn.UserName,
                Password = conn.Password,
                Port = conn.Port,
                VirtualHost = conn.VirtualHost
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // 宣告一個queue
                // 如果未存在在的話，後面執行時會自動建立
                channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                #region Consumer綁定

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.Info(" [x] Received {0}", message);
                };
                channel.BasicConsume(
                    queue: "hello",
                    autoAck: true,
                    consumer: consumer
                );

                #endregion

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
