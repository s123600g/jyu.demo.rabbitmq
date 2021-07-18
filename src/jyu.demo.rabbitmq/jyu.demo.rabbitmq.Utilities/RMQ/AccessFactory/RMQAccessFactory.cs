using jyu.demo.rabbitmq.Utilities.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace jyu.demo.rabbitmq.Utilities.RMQ.AccessFactory
{
    public class RMQAccessFactory
    {
        /// <summary>
        /// 建立RMQ連線實體
        /// </summary>
        /// <param name="connectionStringRMQ_Detail">
        /// RMQ連線資訊
        /// </param>
        /// <returns></returns>
        public IModel GetRMQConnectionEntity(
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

            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            return channel;
        }
    }
}
