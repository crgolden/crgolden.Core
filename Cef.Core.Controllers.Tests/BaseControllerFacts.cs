namespace Cef.Core.Controllers.Tests
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;

    public abstract class BaseControllerFacts<T>
    {
        protected readonly Mock<IMediator> Mediator;
        protected static ILogger<T> Logger => Mock.Of<ILogger<T>>();

        protected BaseControllerFacts()
        {
            Mediator = new Mock<IMediator>();
        }
    }
}