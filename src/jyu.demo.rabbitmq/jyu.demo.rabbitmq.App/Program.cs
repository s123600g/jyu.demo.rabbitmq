using Microsoft.Extensions.DependencyInjection;
using jyu.demo.rabbitmq.Init.InitializeSettings;
using jyu.demo.rabbitmq.Utilities.ServiceCollectionProvider;
using NLog;
using System;

namespace jyu.demo.rabbitmq.App
{
    class Program
    {
        private static Logger _logger;
        private static IServiceProvider _sp;

        static void Main(string[] args)
        {
            ServiceCollectionProvider serviceCollectionProvider = new ServiceCollectionProvider();
            // 載入DI服務容器實體
            _sp = serviceCollectionProvider.GetServicesExtensionEntity();
            // 取得組態設定檔Provider
            var appSettings = _sp.GetService<ILoadSettings>();

            // 載入NLog初始化
            _logger = appSettings.NLogInitialize();

            _logger.Info("The app process was stopped.");
        }
    }
}
