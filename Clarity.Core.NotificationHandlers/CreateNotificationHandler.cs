﻿namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class CreateNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : CreateNotification<TModel>
    {
        private readonly ILogger<CreateNotificationHandler<TNotification, TModel>> _logger;

        protected CreateNotificationHandler(ILogger<CreateNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.EventId)
            {
                case EventIds.CreateStart:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.CreateStart, $"{EventIds.CreateStart}"),
                        message: "Creating model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.CreateEnd:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.CreateEnd, $"{EventIds.CreateEnd}"),
                        message: "Created model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.CreateError:
                    _logger.LogError(
                        eventId: new EventId((int)EventIds.CreateError, $"{EventIds.CreateError}"),
                        exception: notification.Exception,
                        message: "Error creating model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
