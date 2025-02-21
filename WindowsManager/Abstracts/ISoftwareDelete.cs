using WindowsManager.Models;

namespace WindowsManager.Abstracts
{
    public interface ISoftwareDelete
    {
        Task Delete(HttpSoftware software);
    }
}
