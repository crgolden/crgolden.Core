namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class IndexNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : IndexNotification
    {
        private readonly ILogger<IndexNotificationHandler<TNotification>> _logger;

        protected IndexNotificationHandler(ILogger<IndexNotificationHandler<TNotification>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.EventId)
            {
                case EventIds.IndexStart:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.IndexStart, $"{EventIds.IndexStart}"),
                        message: "Searching request {Request} at {Time}",
                        args: new object[] { notification.Request, DateTime.UtcNow });
                    break;
                case EventIds.IndexEnd:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.IndexEnd, $"{EventIds.IndexEnd}"),
                        message: "Found result {Result} at {Time}",
                        args: new object[] { notification.Result, DateTime.UtcNow });
                    break;
                case EventIds.IndexError:
                    _logger.LogError(
                        eventId: new EventId((int)EventIds.IndexError, $"{EventIds.IndexError}"),
                        exception: notification.Exception,
                        message: "Error searching request {Request} at {Time}",
                        args: new object[] { notification.Request, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
