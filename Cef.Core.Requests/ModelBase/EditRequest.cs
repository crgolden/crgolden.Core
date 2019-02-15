namespace Cef.Core.Requests.ModelBase
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class EditRequest<T> : IRequest
        where T : ModelBase
    {
        public readonly Guid Id;

        public readonly T Model;

        protected EditRequest(Guid id, T model)
        {
            Id = id;
            Model = model;
        }
    }
}
