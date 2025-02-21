using Server.Domain.Models;
using Server.Domain.Absctract.Services;
using Server.Domain.Models.DTO.Publisher;
using Server.Domain.Absctract.Repositories;

namespace Server.Application.Services
{

    public class PublisherService(IPublisherRepository publisherRepository) : IPublisherService
    {
        public readonly IPublisherRepository publisherRepository = publisherRepository;

        protected virtual List<Violation> Validation(string Value, string Nameof)
        {
            List<Violation> violations = new List<Violation>();

            if (string.IsNullOrWhiteSpace(Value))
                violations.Add(new Violation(Nameof, $"{Nameof} не может быть пустым"));

            if (Value.Length > 255)
                violations.Add(new Violation(Nameof, $"Длинна {Nameof} не может быть больше 255"));

            return violations;
        }

        public virtual async Task<Result<List<Publisher>>> GetAll() => await publisherRepository.GetAll();
        public virtual async Task<Result<Publisher>> GetByID(int ID) => await publisherRepository.GetByID(ID);
        public virtual async Task<Result<Publisher>> GetByTitle(string Title)
        {
            List<Violation> violations = Validation(Title, nameof(Title));

            if (violations.Count > 1)
                return Result<Publisher>.Fail(new Error("Не верный параметр", 400, violations));

            return await publisherRepository.GetByTitle(Title);
        }
        public virtual async Task<Result<PublisherCreated>> Create(string Title)
        {
            List<Violation> violations = Validation(Title, nameof(Title));

            if (violations.Count > 1)
                return Result<PublisherCreated>.Fail(new Error("Не верный параметр", 400, violations));

            return await publisherRepository.Create(Title);
        }
        public virtual async Task<Result<PublisherCreated>> UpdateByID(int ID, string Title)
        {
            List<Violation> violations = Validation(Title, nameof(Title));

            if (violations.Count > 1)
                return Result<PublisherCreated>.Fail(new Error("Не верный параметр", 400, violations));

            return await publisherRepository.UpdateByID(ID, Title);
        }
        public virtual async Task<Result<PublisherCreated>> UpdateByTitle(string Title, string TitleChange)
        {
            List<Violation> violationsTitle = Validation(Title, nameof(Title));

            List<Violation> titleChangeViolations = Validation(TitleChange, nameof(TitleChange));

            List<Violation>  violations = [.. violationsTitle, .. titleChangeViolations];

            if (violations.Count > 0)
                return Result<PublisherCreated>.Fail(new Error("Не верный параметр", 400, violations));

            return await publisherRepository.UpdateByTitle(Title, TitleChange);
        }
        public virtual async Task<Result<PublisherCreated>> DeleteByID(int ID) => await publisherRepository.DeleteByID(ID);
        public virtual async Task<Result<PublisherCreated>> DeleteByTitle(string Title)
        {
            List<Violation> violations = Validation(Title, nameof(Title));

            if (violations.Count > 1)
                return Result<PublisherCreated>.Fail(new Error("Не верный параметр", 400, violations));

            return await publisherRepository.DeleteByTitle(Title);
        }
        public virtual async Task<Result<Publisher>> FindOrCreate(string Title)
        {
            List<Violation> violations = Validation(Title, nameof(Title));

            if (violations.Count > 0)
                return Result<Publisher>.Fail(new Error("Не верный параметр", 400, violations));

            return await publisherRepository.FindOrCreate(Title);
        }
    }
}
