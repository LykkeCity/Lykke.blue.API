using System.Threading.Tasks;

namespace Lykke.blue.Api.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}
