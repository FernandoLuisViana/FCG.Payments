using FCG.Payments.Domain.DTOs.Requests;
using FCG.Payments.Domain.Enums;
using FCG.Payments.Domain.Interfaces.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FCG.Payments.API.Messaging
{
    public sealed class OrderCreatedConsumer : BackgroundService
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;
        private readonly IConfiguration _cfg;
        private readonly IServiceScopeFactory _scopeFactory;

        private IConnection? _connection;
        private IModel? _channel;

        public OrderCreatedConsumer(
            ILogger<OrderCreatedConsumer> logger,
            IConfiguration cfg,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _cfg = cfg;
            _scopeFactory = scopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var uri = _cfg["RabbitMq:Uri"]!;
            var queue = _cfg["RabbitMq:Queue"]!;

            var factory = new ConnectionFactory
            {
                Uri = new Uri(uri),
                Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = "fly.rmq.cloudamqp.com"
                },
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);
            _channel.BasicQos(0, 1, false);

            _logger.LogInformation("✅ RabbitMQ Consumer iniciado. Fila: {queue}", queue);

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel is null) return Task.CompletedTask;

            var queue = _cfg["RabbitMq:Queue"]!;

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (_, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("📩 OrderCreated recebido: {json}", json);

                    var payload = JsonSerializer.Deserialize<OrderCreatedEvent>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (payload is null || payload.OrderId == Guid.Empty)
                    {
                        _logger.LogWarning("⚠️ Payload inválido. Ack para não travar fila.");
                        _channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    }

                    // Converte string -> enum do seu domínio
                    var method = ParsePaymentMethod(payload.PaymentMethod);

                    using var scope = _scopeFactory.CreateScope();
                    var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

                    // ✅ Seu request real usa TotalAmount e PaymentMethod enum
                    var req = new ProcessPaymentRequest(payload.OrderId, payload.TotalAmount, method);

                    var result = await paymentService.ProcessPaymentAsync(req);

                    if (result.Success)
                    {
                        _logger.LogInformation("✅ Pagamento criado para OrderId={orderId}", payload.OrderId);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _logger.LogWarning("❌ Falha ao processar pagamento para OrderId={orderId}. Requeue.", payload.OrderId);
                        _channel.BasicNack(ea.DeliveryTag, false, requeue: true);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem");
                    _channel.BasicNack(ea.DeliveryTag, false, requeue: true);
                }

                await Task.Yield();
            };

            _channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            try { _channel?.Close(); } catch { }
            try { _connection?.Close(); } catch { }
            base.Dispose();
        }

        private static EPaymentMethod ParsePaymentMethod(string? method)
        {
            if (string.IsNullOrWhiteSpace(method))
                return EPaymentMethod.Credit; // default

            return Enum.TryParse<EPaymentMethod>(method, true, out var parsed)
                ? parsed
                : EPaymentMethod.Credit;
        }

        private sealed class OrderCreatedEvent
        {
            public Guid OrderId { get; set; }
            public decimal TotalAmount { get; set; }     // 👈 Bate com PaymentService (TotalAmount)
            public string? PaymentMethod { get; set; }   // 👈 ex: "Pix", "CreditCard"
        }
    }

}
