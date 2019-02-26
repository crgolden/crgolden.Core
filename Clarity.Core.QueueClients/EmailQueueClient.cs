namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    public class EmailQueueClient : QueueClient, IHostedService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailQueueClient> _logger;
        private MessageHandlerOptions MessageHandlerOptions =>
            new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false
            };

        public EmailQueueClient(
            IOptions<ServiceBusOptions> options,
            IEmailService emailService,
            ILogger<EmailQueueClient> logger)
            : base(new ServiceBusConnectionStringBuilder(
                options.Value.Endpoint,
                options.Value.EmailQueueName,
                options.Value.SharedAccessKeyName,
                options.Value.PrimaryKey,
                TransportType.Amqp))
        {
            _emailService = emailService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation(
                eventId: new EventId((int)EventIds.QueueClientStart, $"{EventIds.QueueClientStart}"),
                message: "Queue client {Client} starting at {Time}",
                args: new object[] { typeof(EmailQueueClient), DateTime.UtcNow });
            RegisterMessageHandler(ProcessMessagesAsync, MessageHandlerOptions);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                eventId: new EventId((int)EventIds.QueueClientStop, $"{EventIds.QueueClientStop}"),
                message: "Queue client {Client} stopping at {Time}",
                args: new object[] { typeof(EmailQueueClient), DateTime.UtcNow });
            await CloseAsync();
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            _logger.LogInformation(
                eventId: new EventId((int)EventIds.QueueClientProcessing, $"{EventIds.QueueClientProcessing}"),
                message: "Queue client {Client} processing at {Time}",
                args: new object[] { typeof(EmailQueueClient), DateTime.UtcNow });
            await _emailService.SendEmailAsync(message.UserProperties, message.Body, token);
            await CompleteAsync(message.SystemProperties.LockToken);
            _logger.LogInformation(
                eventId: new EventId((int)EventIds.QueueClientCompleted, $"{EventIds.QueueClientCompleted}"),
                message: "Queue client {Client} completed at {Time}",
                args: new object[] { typeof(EmailQueueClient), DateTime.UtcNow });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(
                eventId: new EventId((int)EventIds.QueueClientError, $"{EventIds.QueueClientError}"), 
                exception: exceptionReceivedEventArgs.Exception,
                message: "Queue client {Client} received exception {Context} at {Time}",
                args: new object[] { exceptionReceivedEventArgs.ExceptionReceivedContext, DateTime.UtcNow });
            return Task.CompletedTask;
        }
    }
}
