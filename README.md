# ChatApp

Prerequisites:
* Asp.Net Core 5
* RabbitMQ (https://www.rabbitmq.com/download.html)

Set RabbiMQ section in appsettings.json(if needed) for BotService and WebApp. This is the default config:
  "RabbitMQ": {
    "Username": "guest",
    "Password": "guest",
    "Hostname": "localhost",
    "Port": 5672,
    "VirtualHost":  "/"
  }
  
  For testing run BotService and WebApp projects.
