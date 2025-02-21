using AutoMapper;
using Server.Domain.Models;
using Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Models.DTO.Publisher;
using Server.Domain.Absctract.Repositories;

namespace Server.DataAccess.Repositories
{

    public class PublisherRepository(DataBaseContext db, IMapper mapper) : IPublisherRepository
    {
        private readonly DataBaseContext db = db;
        private readonly IMapper mapper = mapper;

        public async Task<Result<List<Publisher>>> GetAll()
        {
            try
            {
                List<PublisherEntities> publisherEntities = await db.Publishers.AsNoTracking()
                    .Include(element => element.Softwares)
                    .ThenInclude(element => element.HostSoftwares)
                    .ThenInclude(element=> element.Host)
                    .ToListAsync();

                List<Publisher> publishers = mapper.Map<List<Publisher>>(publisherEntities);

                return Result<List<Publisher>>.Success(publishers);
            }
            catch(Exception e)
            {
                return Result<List<Publisher>>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<Publisher>> GetByID(int ID)
        {
            try
            {
                PublisherEntities? publisherEntity = await db.Publishers.Include(element => element.Softwares)
                    .ThenInclude(element => element.HostSoftwares)
                    .ThenInclude(element => element.Host)
                    .FirstOrDefaultAsync(element => element.Id == ID);

                if (publisherEntity == null)
                    return Result<Publisher>.Fail(new Error("Объект не найден", 400));

                Publisher publisher = mapper.Map<Publisher>(publisherEntity);

                return Result<Publisher>.Success(publisher);
            }
            catch(Exception e)
            {
                return Result<Publisher>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<Publisher>> GetByTitle(string Title)
        {
            try
            {
                PublisherEntities? publisherEntity = await db.Publishers.AsTracking()
                    .FirstOrDefaultAsync(element => element.Title == Title);

                if (publisherEntity == null)
                    return Result<Publisher>.Fail(new Error("Объект не найден", 400));

                Publisher publisher = mapper.Map<Publisher>(publisherEntity);

                return Result<Publisher>.Success(publisher);
            }
            catch(Exception e)
            {
                return Result<Publisher>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<PublisherCreated>> Create(string Title)
        {
            try
            {
                PublisherEntities? publisherEntities = await db.Publishers.AsNoTracking()
                    .FirstOrDefaultAsync(element => element.Title == Title);

                if (publisherEntities != null)
                    return Result<PublisherCreated>.Fail(new Error("Такой объект уже существует", 409, new List<Violation>
                    {
                        new Violation(nameof(Title), "Title должен быть уникальным")
                    }));


                var result = await db.Publishers.AddAsync(new()
                {
                    Title = Title
                });

                await db.SaveChangesAsync();

                publisherEntities = result.Entity;

                PublisherCreated publisherCreate = mapper.Map<PublisherCreated>(publisherEntities);

                return Result<PublisherCreated>.Success(publisherCreate);
            }
            catch(Exception e)
            {
                return Result<PublisherCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<Publisher>> FindOrCreate(string Title)
        {
            try
            {
                Result<Publisher> findPublisher = await GetByTitle(Title);

                if(!findPublisher.IsSuccess)
                {
                    Result<PublisherCreated> publisherCreated = await Create(Title);

                    if(publisherCreated.IsSuccess)
                    {
                        Publisher publisher = mapper.Map<Publisher>(publisherCreated.Object);
                        return Result<Publisher>.Success(publisher);
                    }

                    return Result<Publisher>.Fail(publisherCreated.Error);
                }

                return findPublisher;
            }
            catch(Exception e)
            {
                return Result<Publisher>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<PublisherCreated>> UpdateByID(int ID, string Title)
        {
            try
            {
                PublisherEntities? publisherEntities = await db.Publishers.FirstOrDefaultAsync(element => element.Id == ID);

                PublisherEntities? publisherEntities2 = await db.Publishers.AsNoTracking()
                    .FirstOrDefaultAsync(element => element.Title == Title);

                if (publisherEntities == null)
                    return Result<PublisherCreated>.Fail(new Error("Объект не существует", 400));

                if (publisherEntities2 != null)
                    return Result<PublisherCreated>.Fail(new Error("Не удалось обновить объект", 400, new List<Violation>
                    {
                        new Violation(nameof(Title), "Title должен быть уникальным")
                    }));

                publisherEntities.Title = Title;

                await db.SaveChangesAsync();

                return Result<PublisherCreated>.Success(mapper.Map<PublisherCreated>(publisherEntities));
            }
            catch (Exception e)
            {
                return Result<PublisherCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<PublisherCreated>> UpdateByTitle(string Title, string TitleChange)
        {
            try
            {
                PublisherEntities? publisherEntities = await db.Publishers.FirstOrDefaultAsync(element => element.Title == Title);

                PublisherEntities? publisherEntities2 = await db.Publishers.AsNoTracking()
                    .FirstOrDefaultAsync(element => element.Title == TitleChange);

                if (publisherEntities == null)
                    return Result<PublisherCreated>.Fail(new Error("Объект не существует", 400));

                if (publisherEntities2 != null)
                    return Result<PublisherCreated>.Fail(new Error("Не удалось обновить объект", 400, new List<Violation>
                    {
                        new Violation(nameof(Title), "Title должен быть уникальным")
                    }));

                publisherEntities.Title = Title;

                await db.SaveChangesAsync();

                return Result<PublisherCreated>.Success(mapper.Map<PublisherCreated>(publisherEntities));
            }
            catch (Exception e)
            {
                return Result<PublisherCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<PublisherCreated>> DeleteByID(int ID)
        {
            try
            {
                PublisherEntities? publisherEntities = await db.Publishers.FindAsync(ID);

                if(publisherEntities == null)
                    return Result<PublisherCreated>.Fail(new Error("Объект не существует", 400));

                db.Publishers.Remove(publisherEntities);
                await db.SaveChangesAsync();

                PublisherCreated publisherCreate = mapper.Map<PublisherCreated>(publisherEntities);

                return Result<PublisherCreated>.Success(publisherCreate);
            }
            catch(Exception e)
            {
                return Result<PublisherCreated>.Fail(new Error(e.Message, 500));
            }
        }

        public async Task<Result<PublisherCreated>> DeleteByTitle(string Title)
        {
            try
            {
                PublisherEntities? publisherEntities = await db.Publishers.AsTracking()
                    .FirstOrDefaultAsync(element => element.Title == Title);

                if (publisherEntities == null)
                    return Result<PublisherCreated>.Fail(new Error("Объект не существует", 400));

                db.Publishers.Remove(publisherEntities);
                await db.SaveChangesAsync();

                PublisherCreated publisherCreate = mapper.Map<PublisherCreated>(publisherEntities);

                return Result<PublisherCreated>.Success(publisherCreate);
            }
            catch (Exception e)
            {
                return Result<PublisherCreated>.Fail(new Error(e.Message, 500));
            }
        }
    }
}
