﻿namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class CreateRangeNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : CreateRangeNotification<TModel>
    {
        private readonly ILogger<CreateRangeNotificationHandler<TNotification, TModel>> _logger;

        protected CreateRangeNotificationHandler(ILogger<CreateRangeNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.EventId)
            {
                case EventIds.CreateRangeStart:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.CreateRangeStart, $"{EventIds.CreateRangeStart}"),
                        message: "Creating models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
                case EventIds.CreateRangeEnd:
                    _logger.LogInformation(
                        eventId: new EventId((int)EventIds.CreateRangeEnd, $"{EventIds.CreateRangeEnd}"),
                        message: "Created models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
                case EventIds.CreateRangeError:
                    _logger.LogError(
                        eventId: new EventId((int)EventIds.CreateRangeError, $"{EventIds.CreateRangeError}"),
                        exception: notification.Exception,
                        message: "Error creating models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
