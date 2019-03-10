namespace Clarity.Core
{
    using MediatR;

    public abstract class DetailsRequest<TEntity, TModel> : IRequest<TModel>
        where TEntity : Entity
        where TModel : Model
    {
        public readonly object[] KeyValues;

        protected DetailsRequest(object[] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
