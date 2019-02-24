namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class DeleteNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : DeleteNotification
    {
        private readonly ILogger<DeleteNotificationHandler<TNotification>> _logger;

        protected DeleteNotificationHandler(ILogger<DeleteNotificationHandler<TNotification>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.EventId)
            {
                case EventIds.DeleteStart:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.DeleteStart, $"{EventIds.DeleteStart}"),
                        message: "Deleting entity with key values {KeyValues} at {Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DeleteEnd:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.DeleteEnd, $"{EventIds.DeleteEnd}"),
                        message: "Deleted entity with key values {KeyValues} at {Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DeleteError:
                    _logger.LogError(
                        eventId: new EventId((int)EventIds.DeleteError, $"{EventIds.DeleteError}"),
                        exception: notification.Exception,
                        message: "Error deleting entity with key values {KeyValues} at {Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
