namespace Clarity.Core.Files
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class FileRemoveNotificationHandler<TNotification, TKey> : INotificationHandler<TNotification>
        where TNotification : FileRemoveNotification<TKey>
    {
        protected readonly ILogger<FileRemoveNotificationHandler<TNotification, TKey>> Logger;

        protected FileRemoveNotificationHandler(ILogger<FileRemoveNotificationHandler<TNotification, TKey>> logger)
        {
            Logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.RemoveStart:
                    Logger.LogInformation(
                        eventId: eventId,
                        message: "Removing files {FileNames} with keyValues {KeyValues} at {Time}",
                        args: new object[] { notification.FileNames, notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.RemoveEnd:
                    Logger.LogInformation(
                        eventId: eventId,
                        message: "Removed files {FileNames} with keyValues {KeyValues} at {Time}",
                        args: new object[] { notification.FileNames, notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.RemoveError:
                    Logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error removing files {FileNames} with keyValues {KeyValues} at {Time}",
                        args: new object[] { notification.FileNames, notification.KeyValues, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
