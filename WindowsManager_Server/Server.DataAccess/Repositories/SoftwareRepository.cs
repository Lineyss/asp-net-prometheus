using AutoMapper;
using Server.Domain.Models;
using Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Models.DTO.Software;
using Server.Domain.Absctract.Repositories;

namespace Server.DataAccess.Repositories
{
    public class SoftwareRepository(DataBaseContext db, IMapper mapper, IPublisherRepository publisherRepository) : ISoftwareRepository
    {
        private readonly DataBaseContext db = db;
        private readonly IMapper mapper = mapper;
        private readonly IPublisherRepository publisherRepository = publisherRepository;

        public async Task<Result<List<Software>>> GetAll(string? Name = null, string? Version = null, int? Publisherid = null)
        {
            try
            {
                List<SoftwareEntities> softwareEntities = await db.Softwares.AsNoTracking()
                    .Include(element => element.HostSoftwares)
                    .ThenInclude(element => element.Host)
                    .Include(element => element.Publisher)
                    .ToListAsync();

                if(Name != null)
                    softwareEntities = softwareEntities.Where(element => element.Name == Name)
                                                       .ToList();

                if (Version != null)
                    softwareEntities = softwareEntities.Where(element => element.Version == Version)
                                                       .ToList();

                if (Publisherid != null)
                    softwareEntities = softwareEntities.Where(element => element.Publisherid == Publisherid)
                                                       .ToList();



                List<Software> softwares = mapper.Map<List<Software>>(softwareEntities);

                return Result<List<Software>>.Success(softwares);
            }
            catch (Exception e)
            {
                return Result<List<Software>>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<Software>> GetByID(int ID)
        {
            try
            {
                SoftwareEntities? softwareEntities = await db.Softwares.Include(e => e.HostSoftwares)
                            .ThenInclude(e => e.Host)
                            .Include(e => e.Publisher)
                            .FirstOrDefaultAsync(element => element.Id == ID);


                if (softwareEntities == null)
                    return Result<Software>.Fail(new Error("Обьект не существует", 400, new List<Violation>
                    {
                        new Violation(nameof(ID), "Объекта с таким ID не существует")
                    }));

                Software software = mapper.Map<Software>(softwareEntities);

                return Result<Software>.Success(software);
            }
            catch (Exception e)
            {
                return Result<Software>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<SoftwareCreated>> Create(SoftwareCreate softwareCreate)
        {
            try
            {
                var ID = softwareCreate.Publisherid;

                if (ID != null)
                {
                    int _ID = (int)ID;
                    var publisher = await publisherRepository.GetByID(_ID);

                    if (!publisher.IsSuccess)
                        return Result<SoftwareCreated>.Fail(new Error("Не верный парметр", 400, new List<Violation>
                        {
                            new Violation("Publisherid", "Publisher с таким ID не существует")
                        }));
                }


                SoftwareEntities? softwareEntity = await db.Softwares.AsNoTracking()
                    .FirstOrDefaultAsync(element => element.Name == softwareCreate.Name &&
                                                    element.Version == softwareCreate.Version &&
                                                    element.Publisherid == softwareCreate.Publisherid);

                if (softwareEntity != null)
                    return Result<SoftwareCreated>.Fail(new Error("Такой объект уже существует", 409, new List<Violation>
                    {
                        new Violation(nameof(softwareCreate.Name), "Программа от этого автора уже существует"),
                        new Violation(nameof(softwareCreate.Publisherid), "Программа от этого автора уже существует")
                    }));


                softwareEntity = mapper.Map<SoftwareEntities>(softwareCreate);

                var result = await db.Softwares.AddAsync(softwareEntity);

                await db.SaveChangesAsync();

                softwareEntity = result.Entity;

                SoftwareCreated softwareCreated = mapper.Map<SoftwareCreated>(softwareEntity);

                return Result<SoftwareCreated>.Success(softwareCreated);
            }
            catch (Exception e)
            {
                return Result<SoftwareCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<SoftwareCreated>> Update(SoftwareCreated softwareCreate)
        {
            try
            {
                SoftwareEntities? softwareEntity = await db.Softwares.AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Name == softwareCreate.Name && e.Publisherid == softwareCreate.Publisherid);

                if (softwareEntity != null)
                    return Result<SoftwareCreated>.Fail(new Error("Такой объект уже существует", 409, new List<Violation>
                    {
                        new Violation(nameof(softwareCreate.Name), "Программа от этого автора уже существует"),
                        new Violation(nameof(softwareCreate.Publisherid), "Программа от этого автора уже существует")
                    }));

                softwareEntity = mapper.Map<SoftwareEntities>(softwareCreate);
                await db.Softwares.AddAsync(softwareEntity);
                await db.SaveChangesAsync();

                SoftwareCreated softwareCreated = mapper.Map<SoftwareCreated>(softwareEntity);

                return Result<SoftwareCreated>.Success(softwareCreated);
            }
            catch (Exception e)
            {
                return Result<SoftwareCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<SoftwareCreate>> DeleteByID(int ID)
        {
            try
            {
                SoftwareEntities? softwareEntities = await db.Softwares.FindAsync(ID);

                if (softwareEntities == null)
                    return Result<SoftwareCreate>.Fail(new Error("Объект не существует", 400));

                db.Softwares.Remove(softwareEntities);
                await db.SaveChangesAsync();

                SoftwareCreate hostDomain = mapper.Map<SoftwareCreate>(softwareEntities);

                return Result<SoftwareCreate>.Success(hostDomain);
            }
            catch (Exception e)
            {
                return Result<SoftwareCreate>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<Software>> FindOrCreate(SoftwareCreate softwareCreate)
        {
            try
            {
                SoftwareEntities? softwareEntities = await db.Softwares.AsNoTracking()
                    .FirstOrDefaultAsync(element => element.Name == softwareCreate.Name &&
                                                    element.Publisherid == softwareCreate.Publisherid &&
                                                    element.Version == softwareCreate.Version);

                if(softwareEntities == null)
                {
                    Result<SoftwareCreated> softwareCreated = await Create(softwareCreate);

                    if (softwareCreated.IsSuccess)
                    {
                        return await GetByID(softwareCreated.Object.Id);
                    }

                    return Result<Software>.Fail(softwareCreated.Error);
                }

                Software _software = mapper.Map<Software>(softwareEntities);

                return Result<Software>.Success(_software);
            }
            catch(Exception e)
            {
                return Result<Software>.Fail(new Error(e.Message, 500));
            }
        }
        public async Task<Result<Software>> Find(SoftwareCreate softwareCreate)
        {
            try
            {
                Result<List<Software>>? result = await GetAll(softwareCreate.Name, softwareCreate.Version, softwareCreate.Publisherid);

                if (!result.IsSuccess)
                    return Result<Software>.Fail(result.Error);

                Software? software = result.Object.LastOrDefault();

                if (software == null)
                    return Result<Software>.Fail(new Error("Объект не существует", 400));

                return Result<Software>.Success(software);
            }
            catch(Exception e)
            {
                return Result<Software>.Fail(new Error(e.Message, 500));
            }
        }
    }
}
