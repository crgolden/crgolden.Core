namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class DeleteNotificationHandler : INotificationHandler<DeleteNotification>
    {
        private readonly ILogger<DeleteNotificationHandler> _logger;

        protected DeleteNotificationHandler(ILogger<DeleteNotificationHandler> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(DeleteNotification notification, CancellationToken cancellationToken)
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
