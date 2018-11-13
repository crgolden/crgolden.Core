namespace Cef.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface ISeedDataService
    {
        Task SeedDatabase();
    }
}