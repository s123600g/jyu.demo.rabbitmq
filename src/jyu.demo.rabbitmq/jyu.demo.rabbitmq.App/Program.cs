using Microsoft.Extensions.DependencyInjection;
using jyu.demo.rabbitmq.Init.InitializeSettings;
using jyu.demo.rabbitmq.Utilities.ServiceCollectionProvider;
using NLog;
using System;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using System.Text;
using RabbitMQ.Client.Events;

namespace jyu.demo.rabbitmq.App
{
    class Program
    {
        private static Logger _logger;
        private static IServiceProvider _sp;
        private static IConfiguration _config;

        static void Main(string[] args)
        {
            ServiceCollectionProvider serviceCollectionProvider = new ServiceCollectionProvider();
            // 載入DI服務容器實體
            _sp = serviceCollectionProvider.GetServicesExtensionEntity();
            // 取得組態設定檔Provider
            var appSettings = _sp.GetService<ILoadSettings>();

            // 載入NLog初始化
            _logger = appSettings.NLogInitialize();
            _config = appSettings.GetConfiguration();

            SendHelloWordForRMQ();
            GetHelloWordForRMQ();

            _logger.Info("The app process was stopped.");
        }

        public static void SendHelloWordForRMQ()
        {
            //string getConnectionString = _config.GetSection(
            //    "ConnectionStringRMQ:defaultRMQ"
            //).Value;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "MyRMQ",
                Password = "1234",
                Port = 5672,
                VirtualHost = "/"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);

                _logger.Info(" [x] Sent {0}", message);
            }
        }

        public static void GetHelloWordForRMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "MyRMQ",
                Password = "1234",
                Port = 5672,
                VirtualHost = "/"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.Info(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);


                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
