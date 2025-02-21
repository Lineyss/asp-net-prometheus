using Server.Domain.Models.DTO.Software;
using Server.Domain.Models;

namespace Server.Domain.Absctract.Repositories
{
    public interface ISoftwareRepository
    {
        Task<Result<SoftwareCreated>> Create(SoftwareCreate softwareCreate);
        Task<Result<SoftwareCreate>> DeleteByID(int ID);
        Task<Result<List<Software>>> GetAll(string? Name = null, string? Version = null, int? Publisherid = null);
        Task<Result<Software>> GetByID(int ID);
        Task<Result<Software>> FindOrCreate(SoftwareCreate softwareCreate);
        Task<Result<Software>> Find(SoftwareCreate softwareCreate);
    }
}
