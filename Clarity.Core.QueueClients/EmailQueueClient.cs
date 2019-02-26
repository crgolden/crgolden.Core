namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Options;

    public class EmailQueueClient : QueueClient
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
            RegisterMessageHandler(ProcessMessagesAsync, MessageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            if (token.IsCancellationRequested) return;
            await _emailService.SendEmailAsync(message.UserProperties, message.Body, token);
            await CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(
                exception: exceptionReceivedEventArgs.Exception,
                message: "Error sending email. Context: {Context}",
                args: new object[] { exceptionReceivedEventArgs.ExceptionReceivedContext });
            return Task.CompletedTask;
        }
    }
}
