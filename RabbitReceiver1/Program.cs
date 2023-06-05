using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory();

factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

factory.ClientProvidedName = "Rabbit Receiver1 App";

IConnection cnn = factory.CreateConnection();

IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

channel.QueueDeclare(queueName, false, false, false, null);

channel.QueueBind(queueName, exchangeName, routingKey, null);

channel.BasicQos(0 , 1 , false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) =>
{
    var body = args.Body.ToArray();

    string message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message Received : {message}");

    channel.BasicAck(args.DeliveryTag , false);
};

string consumerTag = channel.BasicConsume(queueName , false , consumer);

//? To leave Console Running until then messages sent in
Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();

cnn.Close( );