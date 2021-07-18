---
title: jyu.demo.rabbitmq
tags: Github
description: RabbmitMQ C# Dotnet Core Console Pub/Sub練習
---

# jyu.demo.rabbitmq

實作`RabbmitMQ`透過`.NET/C# Client` Dotnet Core Console。

依據教學與相關參考一步一步慢慢實作和理解，如有寫錯地方歡迎糾正。

# 關於專案

### Nuget套件
| 套件                                                |  版本  |
|:--------------------------------------------------- |:------:|
| `RabbitMQ.Client`                                   | 6.2.2  |
| `NLog`                                              | 4.7.10 |
| `NLog.Extensions.Logging`                           | 1.7.3 |
| `Microsoft.Extensions.Configuration.FileExtensions` | 3.1.17 |
| `Microsoft.Extensions.Configuration.Json`           | 3.1.17 |
| `Microsoft.Extensions.Configuration`                | 3.1.17 |
| `Microsoft.Extensions.DependencyInjection`          | 3.1.17 |

相關Nuget套件是安裝在方案檔(`.sln`)，再透過方案檔去指派給哪一個專案。

### 專案結構

* `src/jyu.demo.rabbitmq/`
  - [jyu.demo.rabbitmq.App](https://github.com/s123600g/jyu.demo.rabbitmq/tree/main/src/jyu.demo.rabbitmq/jyu.demo.rabbitmq.App)
    - Dotnet Core Console 專案。
  - [jyu.demo.rabbitmq.Init](https://github.com/s123600g/jyu.demo.rabbitmq/tree/main/src/jyu.demo.rabbitmq/jyu.demo.rabbitmq.Init)
    - 負責載入相關初始化設定。
  - [jyu.demo.rabbitmq.Utilities](https://github.com/s123600g/jyu.demo.rabbitmq/tree/main/src/jyu.demo.rabbitmq/jyu.demo.rabbitmq.Utilities)
    - 放置共用Model與DI Services Handler。 
    - 內部`RMQ/`放置RabbitMQ連線實體產生與Publish/Subscribe Service介面實作。

* `docker_compose/`
  - 放置`RabbitMQ` Docker Composer配置檔案

# RabbitMQ 安裝

官網教學可以參考 [Downloading and Installing RabbitMQ](https://www.rabbitmq.com/download.html)

使用Docker Compose進行運作，專案`RabbitMQ`Docker Compose配置檔在`docker_compose/`。

https://github.com/s123600g/jyu.demo.rabbitmq/blob/main/docker_compose/docker-compose.yml

有關Docker Compose說明可以參考
[Overview of Docker Compose | Docker Documentation](https://docs.docker.com/compose/) 。


# 相關參考

* [RabbitMQ Tutorials](https://www.rabbitmq.com/getstarted.html)
* [Publish/Subscribe-(using the .NET Client)](https://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html)
* [.NET/C# Client API Guide](https://www.rabbitmq.com/dotnet-api-guide.html#consuming-async)
* [RabbitMQ Topic Exchange in C# to Publish or Consume Messages](https://www.tutlane.com/tutorial/rabbitmq/csharp-rabbitmq-topic-exchange)
* [RabbitMQ 基本介紹、安裝教學](https://kucw.github.io/blog/2020/11/rabbitmq/)
* [.NET Core串接RabbitMQ](https://blog.gofa.cloud/Article/post/3bd2DrL6)


