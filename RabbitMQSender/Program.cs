using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQSender;

ConnectionFactory factory = new ConnectionFactory(/*! Here Goes Settings {}*/);

factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

factory.ClientProvidedName = "Rabbit Sender App";

IConnection cnn = factory.CreateConnection();

IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";

string routingKey = "demo-routing-key";

string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName , ExchangeType.Direct);

channel.QueueDeclare(queueName , false , false , false  , null);

channel.QueueBind(queueName , exchangeName , routingKey , null);

//for (int i = 0; i < 60; i++)
//{
//    Console.WriteLine($"Sending Message : {i}");

//    byte[] messageBody = Encoding.UTF8.GetBytes($"Message #{i}");

//    channel.BasicPublish(exchangeName, routingKey, null, messageBody);

//    Thread.Sleep(1000);
//}

//! Example to send a object as class and serialize it

Console.WriteLine("Sending Message");

var jsonString = JsonSerializer.Serialize(new Booking());

var messageBody = Encoding.UTF8.GetBytes(jsonString);

channel.BasicPublish(exchangeName , routingKey ,   null , messageBody);

channel.Close();
cnn.Close();