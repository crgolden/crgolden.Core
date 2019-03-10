namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public abstract class ValidationRequestHandler<TModel> : IRequestHandler<ValidationRequest<TModel>, bool>
    {
        protected readonly IValidationService<TModel> ValidationService;

        protected ValidationRequestHandler(IValidationService<TModel> validationService)
        {
            ValidationService = validationService;
        }

        public virtual async Task<bool> Handle(ValidationRequest<TModel> request, CancellationToken token)
        {
            return await ValidationService.ValidateAsync(request.Model, token).ConfigureAwait(false);
        }
    }
}
