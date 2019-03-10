namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public abstract class ValidationRequestHandler<TRequest, TModel> : IRequestHandler<TRequest, bool>
        where TRequest : ValidationRequest<TModel>
    {
        protected readonly IValidationService<TModel> ValidationService;

        protected ValidationRequestHandler(IValidationService<TModel> validationService)
        {
            ValidationService = validationService;
        }

        public virtual async Task<bool> Handle(TRequest request, CancellationToken token)
        {
            return await ValidationService.ValidateAsync(request.Model, token).ConfigureAwait(false);
        }
    }
}
