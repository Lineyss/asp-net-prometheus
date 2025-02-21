using WindowsManager.Models;

namespace WindowsManager.Abstracts
{
    public interface IHttpHostWorker
    {
        Task<HttpResult<HostModel>> CreateHostnameAsync(string Hostname, CancellationToken token);
        Task<HttpResult<HostModel>> GetByHostnameAsync(string Hostname, CancellationToken token);
        Task<HttpResult<HashSet<HttpSoftware>>> CheckSoftware(string Hostname, HashSet<HttpSoftware> softwares, CancellationToken token);
        Task<HttpResult<HashSet<HttpSoftware>>> AddSoftwares(string Hostname, HashSet<HttpSoftware> softwares, CancellationToken token);
    }
}
