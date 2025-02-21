using AutoMapper;
using Server.Domain.Models;
using Server.Domain.Models.DTO.Host;
using Server.Domain.Models.DTO.Software;
using Server.Domain.Absctract.Repositories;
using Server.Domain.Models.DTO.HostSoftware;
using Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Server.DataAccess.Repositories
{
    public class HostSoftwareRepository(DataBaseContext db, IMapper mapper,
        IHostRepository hostRepository,
        ISoftwareRepository softwareRepository) : IHostSoftwareRepository
    {
        private readonly DataBaseContext db = db;
        private readonly IMapper mapper = mapper;
        private readonly IHostRepository hostRepository = hostRepository;
        private readonly ISoftwareRepository softwareRepository = softwareRepository;

        public virtual async Task<Result<HostSoftware>> GetByID(int ID)
        {
            try
            {
                HostSoftwareEntities? hostSoftwareEntities = await db.HostSoftwares.AsNoTracking()
                    .FirstOrDefaultAsync(element => element.Id == ID);

                if (hostSoftwareEntities == null)
                    return Result<HostSoftware>.Fail(new Error("Такой объект не существует", 400, new List<Violation>
                    {
                        new Violation(nameof(ID), "Объект с таким ID не существует")
                    }));

                HostSoftware hostSoftware = mapper.Map<HostSoftware>(hostSoftwareEntities);

                return Result<HostSoftware>.Success(hostSoftware);
            }
            catch(Exception e)
            {
                return Result<HostSoftware>.Fail(new Error(e.Message, 500));
            }
        }
        public virtual async Task<Result<HostSoftwareCreated>> Create(HostSoftwareCreate hostSoftwareCreate)
        {
            try
            {
                List<Violation> violations = new List<Violation>();

                Result<Host> resultHost = await hostRepository.GetByID(hostSoftwareCreate.Hostid);

                Result<Software> resultSoftware = await softwareRepository.GetByID(hostSoftwareCreate.Softwareid);

                if (!resultHost.IsSuccess)
                    violations = [ .. violations, .. resultHost.Error.Violations];

                if(!resultSoftware.IsSuccess)
                    violations = [.. violations, .. resultSoftware.Error.Violations];

                if (resultHost.IsSuccess && resultSoftware.IsSuccess)
                {
                    HostSoftwareEntities? hostSoftwareEntities = await db.HostSoftwares
                        .FirstOrDefaultAsync(element => element.Hostid == hostSoftwareCreate.Hostid &&
                                                        element.Softwareid == hostSoftwareCreate.Softwareid);

                    if (hostSoftwareEntities != null)
                        return Result<HostSoftwareCreated>.Fail(new Error("Такой объект уже существует", 400));

                    var result = await db.HostSoftwares.AddAsync(new HostSoftwareEntities
                    {
                        Added = DateTime.Now.ToLocalTime(),
                        Isdeleted = false,
                        Hostid = hostSoftwareCreate.Hostid,
                        Softwareid = hostSoftwareCreate.Softwareid
                    });

                    await db.SaveChangesAsync();

                    if (result.Entity == null)
                        return Result<HostSoftwareCreated>.Fail(new Error("Не удалось создать объект", 500));

                    HostSoftwareCreated hostSoftwareCreated = mapper.Map<HostSoftwareCreated>(result.Entity);
                    
                    return Result<HostSoftwareCreated>.Success(hostSoftwareCreated);
                }

                return Result<HostSoftwareCreated>.Fail(new Error("Не валидные данные", 400, violations));
            }
            catch(Exception e)
            {
                return Result<HostSoftwareCreated>.Fail(new Error(e.Message, 500));
            }
        }
        public virtual async Task<Result<HostSoftware>> FindOrCreate(int Hostid, Software software)
        {
            try
            {
                Result<HostSoftware> hostSoftware = await Find(Hostid, software);

                if(!hostSoftware.IsSuccess)
                {
                    Result<HostSoftwareCreated> result = await Create(new HostSoftwareCreate
                    {
                        Hostid = Hostid,
                        Softwareid = software.Id
                    });

                    if (result.IsSuccess)
                        return await GetByID(result.Object.Id);

                    return Result<HostSoftware>.Fail(result.Error);
                }

                return hostSoftware;
            }
            catch (Exception e)
            {
                return Result<HostSoftware>.Fail(new Error(e.Message, 500));
            }
        }
        public virtual async Task<Result<HostSoftware>> Find(int Hostid, Software software)
        {
            try
            {
                HostSoftwareEntities? hostSoftwareEntities = await db.HostSoftwares
                    .FirstOrDefaultAsync(element => element.Hostid == Hostid && element.Softwareid == software.Id);

                if (hostSoftwareEntities == null)
                    return Result<HostSoftware>.Fail(new Error("Объект не существует", 400));

                HostSoftware hostSoftware = mapper.Map<HostSoftware>(hostSoftwareEntities);

                return Result<HostSoftware>.Success(hostSoftware);
            }
            catch(Exception e)
            {
                return Result<HostSoftware>.Fail(new Error(e.Message, 500));
            }
        }
    }
}
