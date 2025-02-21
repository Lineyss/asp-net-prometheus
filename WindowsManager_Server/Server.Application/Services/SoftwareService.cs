using Server.Domain.Models;
using Server.Domain.Absctract.Services;
using Server.Domain.Models.DTO.Software;
using Server.Domain.Absctract.Repositories;
using Server.Domain.Models.DTO.Host;
using Server.Domain.Models.DTO.Publisher;

namespace Server.Application.Services
{
    public class SoftwareService(ISoftwareRepository softwareRepository, IPublisherService publisherService) : ISoftwareService
    {
        private readonly IPublisherService publisherService = publisherService;
        private readonly ISoftwareRepository softwareRepository = softwareRepository;

        protected virtual List<Violation> Validation(string? Value, string Nameof)
        {
            List<Violation> violations = new List<Violation>();

            Violation? nullOrEmpty = ValidateNullOrEmpty(Value, Nameof);

            if (nullOrEmpty != null)
                violations.Add(nullOrEmpty);

            Violation? lenght = ValidateLenght(Value, Nameof);

            if (lenght != null)
                violations.Add(lenght);

            return violations;
        }
        protected virtual Violation? ValidateNullOrEmpty(string? Value, string Nameof)
        {
            if (string.IsNullOrWhiteSpace(Value))
                return new Violation(Nameof, $"{Nameof} не может быть пустым");

            return null;
        }
        protected virtual Violation? ValidateLenght(string? Value, string Nameof)
        {
            if (Value?.Length > 255)
                return new Violation(Nameof, $"Длинна {Nameof} не может быть больше 255");

            return null;
        }

        public virtual async Task<Result<List<Software>>> GetAll() => await softwareRepository.GetAll();
        public virtual async Task<Result<Software>> GetByID(int ID) => await softwareRepository.GetByID(ID);
        public virtual async Task<Result<SoftwareCreated>> Create(SoftwareCreate softwareCreate)
        {
            List<Violation> violationsName = Validation(softwareCreate.Name, nameof(softwareCreate.Name));
            Violation? violationsVersione = ValidateLenght(softwareCreate.Version, nameof(softwareCreate.Version));

            if(violationsVersione != null)
                violationsName.Add(violationsVersione);

            if (violationsName.Count > 1)
                return Result<SoftwareCreated>.Fail(new Error("Не верный параметр", 400, violationsName));

            return await softwareRepository.Create(softwareCreate);
        }
        public virtual async Task<Result<SoftwareCreate>> DeleteByID(int ID) => await softwareRepository.DeleteByID(ID);
        public virtual async Task<Result<Software>> FindOrCreate(Host_AddSoftwares software)
        {
            Result<Publisher> result = null;
            List<Violation> violationsName = Validation(software.Name, nameof(software.Name));
            Violation? violationsVersione =  ValidateLenght(software.Version, nameof(software.Version));

            if (violationsVersione != null)
                violationsName.Add(violationsVersione);

            if(!string.IsNullOrWhiteSpace(software.Publisher))
            {
                result = await publisherService.FindOrCreate(software.Publisher);
                if (!result.IsSuccess)
                    violationsName = [.. violationsName, .. result.Error.Violations];
            }

            if (violationsName.Count > 0)
                return Result<Software>.Fail(new Error("Не верный параметр(ы)", 400, violationsName));

            SoftwareCreate softwareCreate = new SoftwareCreate
            {
                Name = software.Name,
                Version = software.Version,
                Publisherid = result?.Object.Id
            };

            return await softwareRepository.FindOrCreate(softwareCreate);
        }
        public virtual async Task<Result<Software>> Find(Host_AddSoftwares software)
        {
            Result<Publisher>? result = null;

            if(!string.IsNullOrWhiteSpace(software.Publisher))
            {
                result = await publisherService.GetByTitle(software.Publisher);
                if (!result.IsSuccess)
                    return Result<Software>.Fail(new Error("Объект не существует", 400));
            }

            SoftwareCreate softwareCreate = new SoftwareCreate
            {
                Name = software.Name,
                Version = software.Version,
                Publisherid = result?.Object.Id
            };

            return await softwareRepository.Find(softwareCreate);
        }
    }
}
