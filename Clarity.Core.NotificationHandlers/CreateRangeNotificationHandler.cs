namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class CreateRangeNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : CreateRangeNotification<TModel>
        where TModel : Model
    {
        private readonly ILogger<CreateRangeNotificationHandler<TNotification, TModel>> _logger;

        protected CreateRangeNotificationHandler(ILogger<CreateRangeNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.CreateRangeStart:
                    token.ThrowIfCancellationRequested();
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Creating models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
                case EventIds.CreateRangeEnd:
                    token.ThrowIfCancellationRequested();
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Created models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
                case EventIds.CreateRangeError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error creating models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
