namespace Cef.Core.Interfaces
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    [PublicAPI]
    public interface ISeedService
    {
        Task SeedAsync();
    }
}