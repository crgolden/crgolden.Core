namespace Clarity.Core
{
    using MediatR;

    public abstract class DetailsRequest<TResponse> : IRequest<TResponse> where TResponse : class
    {
        public readonly object[] KeyValues;

        protected DetailsRequest(params object[] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
