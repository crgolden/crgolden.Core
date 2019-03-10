namespace Clarity.Core
{
    using System;
    using MediatR;

    public abstract class CreateNotification<TModel> : INotification
        where TModel : Model
    {
        public EventIds EventId { get; set; }

        public TModel Model { get; set; }

        public Exception Exception { get; set; }
    }
}
