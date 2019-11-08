using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Webhook.Slack.Worker.Services;

namespace Webhook.Slack.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ISlackService _slackService;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, ISlackService slackService)
        {
            _logger = logger;
            _slackService = slackService;
            _cancellationTokenSource = new CancellationTokenSource();

            var conf = new ConsumerConfig
            {
                GroupId = "aoj69-consumer",
                BootstrapServers = configuration.GetSection("KafkaServer").Value,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
            _consumer.Subscribe(configuration.GetSection("KafkaTopic").Value);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando serviço...");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                _cancellationTokenSource.Cancel();
            };

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(_cancellationTokenSource.Token);
                    _logger.LogInformation(
                        $"Mensagem consumida: '{consumeResult.Value}'. Posição: '{consumeResult.TopicPartitionOffset}'.");

                    await _slackService.PostSlackAsync(consumeResult.Value);

                    _consumer.Commit();
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Erro ocorrido: {e.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    _consumer.Close();
                }
            }
        }
    }
}
