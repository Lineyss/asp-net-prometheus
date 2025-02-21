using Server.Domain.Models;
using Server.Domain.Models.DTO.Host;
using Server.Domain.Models.DTO.Software;

namespace Server.Domain.Absctract.Services
{
    public interface ISoftwareService
    {
        Task<Result<SoftwareCreated>> Create(SoftwareCreate softwareCreate);
        Task<Result<SoftwareCreate>> DeleteByID(int ID);
        Task<Result<List<Software>>> GetAll();
        Task<Result<Software>> GetByID(int ID);
        Task<Result<Software>> FindOrCreate(Host_AddSoftwares software);
        Task<Result<Software>> Find(Host_AddSoftwares software);
    }
}
