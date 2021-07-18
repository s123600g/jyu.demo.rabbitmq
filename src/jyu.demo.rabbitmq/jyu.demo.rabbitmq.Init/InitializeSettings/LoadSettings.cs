using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;

namespace jyu.demo.rabbitmq.Init.InitializeSettings
{
    /// <summary>
    /// 載入JSON組態設定與Nlog初始化
    /// </summary>
    public class LoadSettings : ILoadSettings
    {
        private string _defaultConfigFilePath;
        private IConfiguration _config;
        public LoadSettings()
        {
            _defaultConfigFilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "ConfigFile"
            );

            _config = AppsetingsLoad();
        }
        /// <summary>
        /// 載入JSON組態設定檔
        /// </summary>
        private IConfiguration AppsetingsLoad()
        {
            var configBuild = new ConfigurationBuilder()
                .SetBasePath(_defaultConfigFilePath)
            ;

            string[] getJsonFile = Directory.GetFiles(_defaultConfigFilePath);

            foreach (string item in getJsonFile)
            {
                configBuild.AddJsonFile(
                    item,
                    optional: true,
                    reloadOnChange: false
                );
            }

            IConfiguration config = configBuild.Build();

            return config;
        }

        /// <summary>
        /// 取得IConfiguration 實體。
        /// </summary>
        /// <returns></returns>
        public IConfiguration GetConfiguration()
        {
            return _config;
        }

        /// <summary>
        /// NLog 載入初始化。
        /// </summary>
        /// <returns>回應NLog初始化後實體。</returns>
        public Logger NLogInitialize()
        {
            // NLog configuration with appsettings.json
            // https://github.com/NLog/NLog.Extensions.Logging/wiki/NLog-configuration-with-appsettings.json
            // 從組態設定檔載入NLog設定
            NLog.LogManager.Configuration = new NLogLoggingConfiguration(_config.GetSection("NLog"));
            Logger logger = LogManager.GetCurrentClassLogger();

            return logger;
        }
    }
}
