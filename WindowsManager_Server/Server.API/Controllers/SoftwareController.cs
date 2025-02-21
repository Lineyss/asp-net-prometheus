using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Absctract.Services;
using Server.Domain.Models;
using Server.Domain.Models.DTO.Host;
using Server.Domain.Models.DTO.Software;

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoftwareController(ISoftwareService softwareService) : ControllerBase
    {
        private readonly ISoftwareService softwareService = softwareService;

        [HttpGet]
        public async Task<ActionResult<List<Software>>> GetAll()
        {
            Result<List<Software>> result = await softwareService.GetAll();

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<Software>> GetByID(int ID)
        {
            Result<Software> result = await softwareService.GetByID(ID);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpPost]
        public async Task<ActionResult<SoftwareCreated>> Create(SoftwareCreate softwareCreate)
        {
            Result<SoftwareCreated> result = await softwareService.Create(softwareCreate);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult<SoftwareCreate>> DeleteByID(int ID)
        {
            Result<SoftwareCreate> result = await softwareService.DeleteByID(ID);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpPost("findOrCreate")]
        public async Task<ActionResult<Software>> FindOrCreate([FromBody] Host_AddSoftwares software)
        {
            Result<Software> result = await softwareService.FindOrCreate(software);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }

        [HttpPost("find")]
        public async Task<ActionResult<Software>> Find([FromBody] Host_AddSoftwares software)
        {
            Result<Software> result = await softwareService.Find(software);

            if (result.IsSuccess)
                return Ok(result.Object);

            return StatusCode(result.Error.Status, result.Error);
        }
    }
}
