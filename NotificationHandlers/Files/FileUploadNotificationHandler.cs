namespace Clarity.Core.Files
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Shared;

    public abstract class FileUploadNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : FileUploadNotification<TModel>
    {
        protected readonly ILogger<FileUploadNotificationHandler<TNotification, TModel>> Logger;

        protected FileUploadNotificationHandler(ILogger<FileUploadNotificationHandler<TNotification, TModel>> logger)
        {
            Logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.UploadStart:
                    Logger.LogInformation(
                        eventId: eventId,
                        message: "Uploading files {@Files} at {@Time}",
                        args: new object[] { notification.Files, DateTime.UtcNow });
                    break;
                case EventIds.UploadEnd:
                    Logger.LogInformation(
                        eventId: eventId,
                        message: "Uploaded files {@Files} at {@Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
                case EventIds.UploadError:
                    Logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error uploading files {@Files} at {@Time}",
                        args: new object[] { notification.Files, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
