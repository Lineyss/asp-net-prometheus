using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Absctract.Services;
using Server.Domain.Models;
using Server.Domain.Models.DTO.Publisher;

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            this.publisherService = publisherService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Publisher>>> GetAll()
        {
            Result<List<Publisher>> result = await publisherService.GetAll();

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetByID(int id)
        {
            Result<Publisher> result = await publisherService.GetByID(id);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpPost]
        public async Task<ActionResult<PublisherCreated>> Create([FromBody] string title)
        {
            Result<PublisherCreated> result = await publisherService.Create(title);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PublisherCreated>> UpdateByID(int id, [FromBody] string title)
        {
            Result<PublisherCreated> result = await publisherService.UpdateByID(id, title);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpPatch("title")]
        public async Task<ActionResult<PublisherCreated>> UpdateByTitle(string Title, string TitleChange)
        {
            Result<PublisherCreated> result = await publisherService.UpdateByTitle(Title, TitleChange);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PublisherCreated>> DeleteByID(int id)
        {
            Result<PublisherCreated> result = await publisherService.DeleteByID(id);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpDelete("title")]
        public async Task<ActionResult<PublisherCreated>> DeleteByTitle([FromBody] string title)
        {
            Result<PublisherCreated> result = await publisherService.DeleteByTitle(title);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpPost("findOrCreate")]
        public async Task<ActionResult<Publisher>> FindOrCreate([FromBody] string title)
        {
            Result<Publisher> result = await publisherService.FindOrCreate(title);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }
    }
}
