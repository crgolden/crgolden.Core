﻿namespace Clarity.Core
{
    using MediatR;

    public abstract class DeleteRequest : IRequest
    {
        public readonly object[] KeyValues;

        protected DeleteRequest(object[] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
