using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace jyu.demo.rabbitmq.Init.InitializeSettings
{
    /// <summary>
    /// JSON設定檔載入介面
    /// </summary>
    public interface ILoadSettings
    {
        /// <summary>
        /// 取得Configuration
        /// </summary>
        /// <returns></returns>
        IConfiguration GetConfiguration();

        /// <summary>
        /// NLog載入
        /// </summary>
        Logger NLogInitialize();
    }
}
