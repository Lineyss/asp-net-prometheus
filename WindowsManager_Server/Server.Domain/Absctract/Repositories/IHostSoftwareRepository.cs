using Server.Domain.Models;
using Server.Domain.Models.DTO.HostSoftware;
using Server.Domain.Models.DTO.Software;

namespace Server.Domain.Absctract.Repositories
{
    public interface IHostSoftwareRepository
    {
        Task<Result<HostSoftware>> FindOrCreate(int Hostid, Software software);
        Task<Result<HostSoftware>> Find(int Hostid, Software software);
    }
}
