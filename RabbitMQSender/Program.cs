using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new ConnectionFactory();

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

byte[] messageBody = Encoding.UTF8.GetBytes("Hello World");

channel.BasicPublish(exchangeName , routingKey , null , messageBody);


channel.Close();
cnn.Close();