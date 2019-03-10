﻿namespace Clarity.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class UploadNotificationHandler<TModel> : INotificationHandler<UploadNotification<TModel>>
    {
        protected readonly ILogger<UploadNotificationHandler<TModel>> Logger;

        protected UploadNotificationHandler(ILogger<UploadNotificationHandler<TModel>> logger)
        {
            Logger = logger;
        }

        public virtual Task Handle(UploadNotification<TModel> notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.UploadStart:
                    Logger.LogInformation(
                        eventId: eventId,
                        message: "Uploading files {Files} at {Time}",
                        args: new object[] { notification.Files, DateTime.UtcNow });
                    break;
                case EventIds.UploadEnd:
                    Logger.LogInformation(
                        eventId: eventId,
                        message: "Uploaded models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
                case EventIds.UploadError:
                    Logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error uploading files {Files} at {Time}",
                        args: new object[] { notification.Files, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
