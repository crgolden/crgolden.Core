namespace Clarity.Core
{
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Hosting;

    public interface IEmailQueueClient : IQueueClient, IHostedService
    {
    }
}
