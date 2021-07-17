using jyu.demo.rabbitmq.Init.InitializeSettings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace jyu.demo.rabbitmq.Utilities.ServiceCollectionProvider
{
    /// <summary>
    /// DI容器提供者
    /// </summary>
    public class ServiceCollectionProvider
    {
        private ServiceCollection _services;

        public ServiceCollectionProvider()
        {
            _services = new ServiceCollection();
        }

        /// <summary>
        /// 取得載入擴充Services項目ServiceProvider
        /// </summary>
        /// <returns>
        /// 回傳一組型態為<see cref="ServiceProvider"/>服務提供者。
        /// </returns>
        public ServiceProvider GetServicesExtensionEntity()
        {

            _services.AddSingleton<ILoadSettings, LoadSettings>();

            var _sp = _services.BuildServiceProvider();

            return _sp;
        }
    }
}
