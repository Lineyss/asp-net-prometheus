using Server.Domain.Models.DTO.Publisher;
using Server.Domain.Models;

namespace Server.Domain.Absctract.Services
{
    public interface IPublisherService
    {
        Task<Result<PublisherCreated>> Create(string Title);
        Task<Result<PublisherCreated>> DeleteByID(int ID);
        Task<Result<PublisherCreated>> DeleteByTitle(string Title);
        Task<Result<Publisher>> GetByID(int ID);
        Task<Result<List<Publisher>>> GetAll();
        Task<Result<PublisherCreated>> UpdateByID(int ID, string Title);
        Task<Result<PublisherCreated>> UpdateByTitle(string Title, string TitleChange);
        Task<Result<Publisher>> FindOrCreate(string Title);
        Task<Result<Publisher>> GetByTitle(string Title);
    }
}
