using AutoMapper;
using Server.Domain.Models;
using Server.DataAccess.Entities;
using Server.Domain.Models.DTO.Host;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Absctract.Repositories;

namespace Server.DataAccess.Repositories
{
    public class HostRepository(DataBaseContext db, IMapper mapper) : IHostRepository
    {
        private readonly DataBaseContext db = db;
        private readonly IMapper mapper = mapper;

        public async Task<Result<List<Host>>> GetAll()
        {
            try
            {
                List<HostEntities> hosts = await db.Hosts.AsNoTracking()
                                                 .Include(element => element.HostSoftwares)
                                                 .ThenInclude(element => element.Software)
                                                 .ThenInclude(element => element.Publisher)
                                                 .ToListAsync();

                List<Host> hostDomains = mapper.Map<List<Host>>(hosts);

                return Result<List<Host>>.Success(hostDomains);
            }
            catch (Exception e)
            {
                return Result<List<Host>>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<Host>> GetByHostname(string Hostname)
        {
            try
            {
                HostEntities? host = await db.Hosts.AsNoTracking()
                                            .Include(element => element.HostSoftwares)
                                            .ThenInclude(element => element.Software)
                                            .ThenInclude(element => element.Publisher)
                                            .FirstOrDefaultAsync(element => element.Hostname == Hostname);

                if (host == null)
                    return Result<Host>.Fail(new Error("Объект не существует", 400));

                Host hostDomain = mapper.Map<Host>(host);

                return Result<Host>.Success(hostDomain);
            }
            catch (Exception e)
            {
                return Result<Host>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<Host>> GetByID(int ID)
        {
            try
            {
                HostEntities? host = await db.Hosts
                                .Include(element => element.HostSoftwares)
                                .ThenInclude(element => element.Software)
                                .ThenInclude(element => element.Publisher)
                                .FirstOrDefaultAsync(element => element.Id == ID);

                if (host == null)
                    return Result<Host>.Fail(new Error("Объект не существует", 400, new List<Violation>
                    {
                        new Violation(nameof(ID), "Объекта с таким ID не существует")
                    }));

                Host hostDomain = mapper.Map<Host>(host);

                return Result<Host>.Success(hostDomain);
            }
            catch (Exception e)
            {
                return Result<Host>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<HostCreated>> UpdateByHostname(string Hostname, string ChaneHostname)
        {
            try
            {
                HostEntities? hostEntities = await db.Hosts.FirstOrDefaultAsync(element => element.Hostname == Hostname);

                HostEntities? hostEntities2 = await db.Hosts.AsNoTracking()
                    .FirstOrDefaultAsync(element => element.Hostname == ChaneHostname);

                if (hostEntities == null)
                    return Result<HostCreated>.Fail(new Error("Объект не существует", 400));

                if (hostEntities2 != null)
                    return Result<HostCreated>.Fail(new Error("Не удалось обновить объект", 400, new List<Violation>
                    {
                        new Violation(nameof(Hostname), "Hostname должен быть уникальным")
                    }));

                hostEntities.Hostname = ChaneHostname;

                await db.SaveChangesAsync();

                return Result<HostCreated>.Success(mapper.Map<HostCreated>(hostEntities));
            }
            catch (Exception e)
            {
                return Result<HostCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<HostCreated>> UpdateByID(int ID, string ChaneHostname)
        {
            try
            {
                HostEntities? hostEntities = await db.Hosts.FirstOrDefaultAsync(element => element.Id == ID);

                HostEntities? hostEntities2 = await db.Hosts.AsNoTracking()
                    .FirstOrDefaultAsync(element => element.Hostname == ChaneHostname);

                if (hostEntities == null)
                    return Result<HostCreated>.Fail(new Error("Объект не существует", 400));

                if (hostEntities2 != null)
                    return Result<HostCreated>.Fail(new Error("Не удалось обновить объект", 400, new List<Violation>
                    {
                        new Violation("Hostname", "Hostname должен быть уникальным")
                    }));

                hostEntities.Hostname = ChaneHostname;

                await db.SaveChangesAsync();

                return Result<HostCreated>.Success(mapper.Map<HostCreated>(hostEntities));
            }
            catch (Exception e)
            {
                return Result<HostCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<HostCreated>> Create(string Hostname)
        {
            try
            {
                HostEntities? host = await db.Hosts.FirstOrDefaultAsync(elemen => elemen.Hostname == Hostname);

                if (host != null)
                    return Result<HostCreated>.Fail(new Error("Такой объект уже существует", 409, new List<Violation>
                    {
                        new Violation(nameof(Hostname), "Hostname должен быть уникальным")
                    }));

                var result = await db.Hosts.AddAsync(new HostEntities
                {
                    Hostname = Hostname
                });

                await db.SaveChangesAsync();

                HostCreated hostdomain = mapper.Map<HostCreated>(result.Entity);

                return Result<HostCreated>.Success(hostdomain);
            }
            catch (Exception e)
            {
                return Result<HostCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<Host>> FindOrCreate(string Hostname)
        {
            try
            {
                Result<Host> host = await GetByHostname(Hostname);

                if (!host.IsSuccess)
                {
                    Result<HostCreated> hostCreated = await Create(Hostname);

                    if (!hostCreated.IsSuccess)
                        return Result<Host>.Fail(hostCreated.Error);

                    Host _host = mapper.Map<Host>(hostCreated);

                    return Result<Host>.Success(_host);
                }

                return Result<Host>.Success(host.Object);
            }
            catch (Exception e)
            {
                return Result<Host>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<HostCreated>> DeleteByID(int ID)
        {
            try
            {
                HostEntities? host = await db.Hosts
                    .FirstOrDefaultAsync(h => h.Id == ID);

                if (host == null)
                    return Result<HostCreated>.Fail(new Error("Объект не существует", 400));

                db.Hosts.Remove(host);
                await db.SaveChangesAsync();

                HostCreated hostDomain = mapper.Map<HostCreated>(host);

                return Result<HostCreated>.Success(hostDomain);
            }
            catch (Exception e)
            {
                return Result<HostCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<HostCreated>> DeleteByHostname(string Hostname)
        {
            try
            {
                HostEntities? host = await db.Hosts.FirstOrDefaultAsync(element => element.Hostname == Hostname);

                if (host == null)
                    return Result<HostCreated>.Fail(new Error("Объект не существует", 400));

                db.Hosts.Remove(host);
                await db.SaveChangesAsync();

                HostCreated hostDomain = mapper.Map<HostCreated>(host);

                return Result<HostCreated>.Success(hostDomain);
            }
            catch (Exception e)
            {
                return Result<HostCreated>.Fail(new Error(e.Message, 500));
            }
        }
    }
}
