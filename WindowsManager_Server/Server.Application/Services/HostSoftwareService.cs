using AutoMapper;
using Server.Domain.Models;
using Server.Domain.Models.DTO.Host;
using Server.Domain.Absctract.Services;
using Server.Domain.Models.DTO.Software;
using Server.Domain.Absctract.Repositories;
using Server.Domain.Models.DTO.HostSoftware;

namespace Server.Application.Services
{

    public class HostSoftwareService(IHostSoftwareRepository hostSoftwareRepository, IMapper mapper,
        ISoftwareService softwareService) : IHostSoftwareService
    {
        private readonly IMapper mapper = mapper;
        private readonly ISoftwareService softwareService = softwareService;
        private readonly IHostSoftwareRepository hostSoftwareRepository = hostSoftwareRepository;

        public async Task<Result<HostSoftware>> FindOrCreate(int Hostid, Host_AddSoftwares hostSoftwareCreate)
        {
            Result<Software> result = await softwareService.FindOrCreate(hostSoftwareCreate);

            if (!result.IsSuccess)
                return Result<HostSoftware>.Fail(result.Error);

            return await hostSoftwareRepository.FindOrCreate(Hostid, result.Object);
        }

        public async Task<Result<HostSoftware>> Find(int Hostid, Host_AddSoftwares hostSoftwareCreate)
        {
            Result<Software> result = await softwareService.Find(hostSoftwareCreate);

            if (!result.IsSuccess)
                return Result<HostSoftware>.Fail(result.Error);

            return await hostSoftwareRepository.Find(Hostid, result.Object);
        }
    }
}
