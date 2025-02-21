using Server.Domain.Models;
using Server.Domain.Models.DTO.Host;
using Server.Domain.Models.DTO.HostSoftware;

namespace Server.Domain.Absctract.Services
{
    public interface IHostSoftwareService
    {
        Task<Result<HostSoftware>> Find(int Hostid, Host_AddSoftwares hostSoftwareCreate);
        Task<Result<HostSoftware>> FindOrCreate(int Hostid, Host_AddSoftwares hostSoftwareCreate);
    }
}
