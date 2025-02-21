using Server.Domain.Models;
using Server.Domain.Models.DTO.Host;

namespace Server.Domain.Absctract.Services
{
    public interface IHostService
    {
        Task<Result<Host>> GetByID(int ID);
        Task<Result<List<Host>>> GetAll();
        Task<Result<HostCreated>> DeleteByID(int ID);
        Task<Result<Host>> GetByHostname(string hostname);
        Task<Result<HostCreated>> Create(string hostDomain);
        Task<Result<HostCreated>> DeleteByHostname(string hostname);
        Task<Result<Host>> AddSoftwares(string Hostname, List<Host_AddSoftwares> softwares);
        Task<Result<List<Host_AddSoftwares>>> CheckSoftwares(string Hostname, List<Host_AddSoftwares> softwares);
    }
}
