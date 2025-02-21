using Server.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Models.DTO.Host;
using Server.Domain.Absctract.Services;

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController(IHostService hostService, ILogger<HostController> logger) : ControllerBase
    {
        private readonly IHostService hostService = hostService;
        private readonly ILogger<HostController> logger = logger;

        [HttpGet]
        public async Task<ActionResult<List<Domain.Models.DTO.Host.Host>>> GetAll()
        {
            Result<List<Domain.Models.DTO.Host.Host>> result = await hostService.GetAll();

            if (!result.IsSuccess)
                return StatusCode(result.Error.Status, result.Error);

            return Ok(result.Object);
        }

        [HttpGet("{ID}")]
        public async Task<ActionResult<Domain.Models.DTO.Host.Host>> GetByID(int ID)
        {
            Result<Domain.Models.DTO.Host.Host> result = await hostService.GetByID(ID);

            if (!result.IsSuccess)
                return StatusCode(result.Error.Status, result.Error);

            return Ok(result.Object);
        }

        [HttpGet("Hostname/{Hostname}")]
        public async Task<ActionResult<Domain.Models.DTO.Host.Host>> GetByHostname(string Hostname)
        {
            Result<Domain.Models.DTO.Host.Host> result = await hostService.GetByHostname(Hostname);

            if (!result.IsSuccess)
                return StatusCode(result.Error.Status, result.Error);

            return Ok(result.Object);
        }

        [HttpPost]
        public async Task<ActionResult<HostCreated>> Create([FromBody] string Hostname)
        {

            Result<HostCreated> result = await hostService.Create(Hostname);

            if (!result.IsSuccess)
                return StatusCode(result.Error.Status, result.Error);

            return Ok(result.Object);
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult<HostCreated>> DeleteByID(int ID)
        {
            Result<HostCreated> result = await hostService.DeleteByID(ID);

            if (!result.IsSuccess)
                return StatusCode(result.Error.Status, result.Error);

            return Ok(result.Object);
        }

        [HttpDelete("Hostname/{Hostname}")]
        public async Task<ActionResult<HostCreated>> DeleteByHostname(string Hostname)
        {
            Result<HostCreated> result = await hostService.DeleteByHostname(Hostname);

            if (!result.IsSuccess)
                return StatusCode(result.Error.Status, result.Error);

            return Ok(result.Object);
        }

        [HttpPost("{Hostname}/software", Name = "AddSoftwares")]
        public async Task<ActionResult<Domain.Models.DTO.Host.Host>> AddSoftwares(string Hostname, [FromBody] List<Host_AddSoftwares> softwares)
        {
            logger.LogInformation($"{DateTime.Now}: Запрос на {nameof(AddSoftwares)}");
            Result<Domain.Models.DTO.Host.Host> result = await hostService.AddSoftwares(Hostname, softwares);

            if (!result.IsSuccess)
                return StatusCode(result.Error.Status, result.Error);

            return Ok(result.Object);
        }

        [HttpPut("{Hostname}/software", Name="CheckValidSoftwares")]
        public async Task<ActionResult<List<Host_AddSoftwares>>> CheckSoftwares(string Hostname, [FromBody] List<Host_AddSoftwares> softwares)
        {
            Result<List<Host_AddSoftwares>> result = await hostService.CheckSoftwares(Hostname, softwares);

            if (!result.IsSuccess)
                return StatusCode(result.Error.Status, result.Error);

            return Ok(result.Object);
        }
    }
}
