using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

string serviceUrl = configuration["QueueInfo:ServiceUrl"];
string queueUrl = configuration["QueueInfo:QueueUrl"];

var sqsClient = new AmazonSQSClient(new BasicAWSCredentials(configuration["AWSCredentials:AccessKey"], configuration["AWSCredentials:Password"]),
    new AmazonSQSConfig
    {
        ServiceURL = serviceUrl,
        UseHttp = true
    });

while (true)
{
    var receiveMessageRequest = new ReceiveMessageRequest
    {
        QueueUrl = queueUrl,
        MaxNumberOfMessages = 10,
        WaitTimeSeconds = 20
    };

    var receiveMessageResponse = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);

    foreach (var message in receiveMessageResponse.Messages)
    {
        Console.WriteLine($"Mensagem recebida: {message.Body}");

        var deleteMessageRequest = new DeleteMessageRequest
        {
            QueueUrl = queueUrl,
            ReceiptHandle = message.ReceiptHandle
        };
        await sqsClient.DeleteMessageAsync(deleteMessageRequest);
    }
}

