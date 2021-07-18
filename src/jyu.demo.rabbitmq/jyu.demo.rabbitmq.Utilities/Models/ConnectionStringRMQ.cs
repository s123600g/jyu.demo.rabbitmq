using System;
using System.Collections.Generic;
using System.Text;

namespace jyu.demo.rabbitmq.Utilities.Models
{
    /// <summary>
    /// RMQ 連線資訊集合
    /// </summary>
    public class ConnectionStringRMQ
    {
        /// <summary>
        /// 預設RMQ連線資訊
        /// </summary>
        public ConnectionStringRMQ_Detail defaultRMQ { get; set; }
    }

    /// <summary>
    /// RMQ 連線資訊內容
    /// </summary>
    public class ConnectionStringRMQ_Detail
    {
        public string HostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int Port { get; set; } = 5672;

        public string VirtualHost { get; set; } = "/";
    }
}
