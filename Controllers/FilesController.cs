namespace crgolden.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Controllers;
    using Files;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Shared;

    public abstract class FilesController<TEntity, TModel, TKey> : ClassController<TEntity, TModel, TKey>
        where TEntity : File, new()
        where TModel : FileModel
    {
        protected FilesController(IMediator mediator, IMemoryCache cache, IOptions<CacheOptions> cacheOptions)
            : base(mediator, cache, cacheOptions)
        {
        }

        public abstract Task<IActionResult> Upload(IFormFileCollection files);

        protected virtual async Task<IActionResult> Upload<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : FileUploadRequest<TEntity, TModel>
            where TNotification : FileUploadNotification<TModel>
        {
            if (request.Files.Count == 0) return BadRequest("No files received from the upload");
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Files = request.Files;
                    notification.EventId = EventIds.UploadStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.EventId = EventIds.UploadEnd;
                    notification.Models = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Content(JsonConvert.SerializeObject(
                        value: notification.Models,
                        settings: new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }));
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.UploadError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.Files);
                }
            }
        }

        public abstract Task<IActionResult> Remove(string[] fileNames, TKey[][] keys = null);

        protected virtual async Task<IActionResult> Remove<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : FileRemoveRequest<TKey>
            where TNotification : FileRemoveNotification<TKey>
        {
            if (request.FileNames.Length == 0) return BadRequest(request.FileNames);
            if (request.Keys != null && request.FileNames.Length != request.Keys.Length) return BadRequest(new
            {
                request.FileNames,
                request.Keys
            });
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.FileNames = request.FileNames;
                    notification.KeyValues = request.Keys;
                    notification.EventId = EventIds.RemoveStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.RemoveEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Content(JsonConvert.SerializeObject(
                        value: request.FileNames.Select((x, i) =>
                        {
                            TKey[] id = null;
                            if (request.Keys?.Length > i) id = request.Keys[i];
                            return new KeyValuePair<string, TKey[]>(x, id);
                        }),
                        settings: new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }));
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.EventId = EventIds.RemoveError;
                    notification.Exception = e;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.Keys);
                }
            }
        }
    }
}
