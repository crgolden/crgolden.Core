namespace Cef.Core.Controllers.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;

    [ExcludeFromCodeCoverage]
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