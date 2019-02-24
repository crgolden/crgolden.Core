namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class DetailsNotificationHandler<TModel> : INotificationHandler<DetailsNotification<TModel>>
    {
        private readonly ILogger<DetailsNotificationHandler<TModel>> _logger;

        protected DetailsNotificationHandler(ILogger<DetailsNotificationHandler<TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(DetailsNotification<TModel> notification, CancellationToken cancellationToken)
        {
            switch (notification.EventId)
            {
                case EventIds.DetailsStart:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.DetailsStart, $"{EventIds.DetailsStart}"),
                        message: "Details requested for key values {KeyValues} at {Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DetailsNotFound:
                    _logger.LogWarning(
                        eventId: new EventId((int)EventIds.DetailsNotFound, $"{EventIds.DetailsNotFound}"),
                        message: "Details not found key values {KeyValues} at {Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DetailsEnd:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.DetailsEnd, $"{EventIds.DetailsEnd}"),
                        message: "Details found for model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.DetailsError:
                    _logger.LogError(
                        eventId: new EventId((int)EventIds.DetailsError, $"{EventIds.DetailsError}"),
                        exception: notification.Exception,
                        message: "Error finding details for key values {KeyValues} at {Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
