using Server.Domain.Models;
using Server.Domain.Models.DTO.Host;

namespace Server.Domain.Absctract.Repositories
{
    public interface IHostRepository
    {
        Task<Result<List<Host>>> GetAll();
        Task<Result<Host>> GetByID(int ID);
        Task<Result<Host>> GetByHostname(string hostname);
        Task<Result<HostCreated>> Create(string host);
        Task<Result<HostCreated>> UpdateByID(int ID, string ChangeHostname);
        Task<Result<HostCreated>> UpdateByHostname(string hostname, string ChangeHostname);
        Task<Result<HostCreated>> DeleteByID(int ID);
        Task<Result<HostCreated>> DeleteByHostname(string Hostname);
        Task<Result<Host>> FindOrCreate(string Hostname);
    }
}
