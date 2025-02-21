using WindowsManager.Models;

namespace WindowsManager.Abstracts
{
    public interface ISoftwareFinder
    {
        Task<HashSet<RegistryModel>> Find(CancellationToken token);
    }
}
