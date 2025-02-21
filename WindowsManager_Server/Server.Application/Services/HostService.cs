using AutoMapper;
using Server.Domain.Models;
using Server.Domain.Models.DTO.Host;
using Microsoft.Extensions.Logging;
using Server.Domain.Absctract.Services;
using Server.Domain.Absctract.Repositories;
using Server.Domain.Models.DTO.HostSoftware;

namespace Server.Application.Services
{
    public class HostService(IHostRepository hostRepository, 
                            IHostSoftwareService hostSoftwareService, 
                            IMapper mapper, ILogger<HostService> logger) : IHostService
    {
        private readonly IMapper mapper = mapper;
        private readonly IHostRepository hostRepository = hostRepository;
        private readonly IHostSoftwareService hostSoftwareService = hostSoftwareService;
        private readonly ILogger<HostService> logger = logger;

        protected virtual List<Violation> Validation(string Value, string Nameof)
        {
            List<Violation> violations = new List<Violation>();

            if (string.IsNullOrWhiteSpace(Value))
                violations.Add(new Violation(Nameof, $"{Nameof} не может быть пустым"));

            if (Value.Length > 255)
                violations.Add(new Violation(Nameof, $"Длинна {Nameof} не может быть больше 255"));

            return violations;
        }
        public virtual async Task<Result<List<Host>>> GetAll() => await hostRepository.GetAll();
        public virtual async Task<Result<Host>> GetByHostname(string Hostname)
        {
            List<Violation> violations = Validation(Hostname, nameof(Hostname));

            if (violations.Count > 0)
                return Result<Host>.Fail(new Error("Не верный параметр", 400, violations));

            return await hostRepository.GetByHostname(Hostname);
        }
        public virtual async Task<Result<Host>> GetByID(int ID) => await hostRepository.GetByID(ID);
        public virtual async Task<Result<HostCreated>> Create(string Hostname)
        {
            List<Violation> violations = Validation(Hostname, nameof(Hostname));

            if (violations.Count > 0)
                return Result<HostCreated>.Fail(new Error("Не верный параметр", 400, violations));

            return await hostRepository.Create(Hostname);
        }
        public virtual async Task<Result<HostCreated>> UpdateByHostname(string Hostname, string ChangeHostname)
        {
            List<Violation> violationsHostname = Validation(Hostname, nameof(Hostname));
            List<Violation> violationsChangeHostname = Validation(ChangeHostname, nameof(ChangeHostname));

            List<Violation> violations = [.. violationsHostname, .. violationsChangeHostname];

            if (violations.Count > 0)
                return Result<HostCreated>.Fail(new Error("Не верный параметр", 400, violations));

            return await hostRepository.UpdateByHostname(Hostname, ChangeHostname);
        }
        public virtual async Task<Result<HostCreated>> UpdateByID(int ID, string ChangeHostname)
        {
            List<Violation> violations = Validation(ChangeHostname, nameof(ChangeHostname));

            if (violations.Count > 0)
                return Result<HostCreated>.Fail(new Error("Не верный параметр", 400, violations));

            return await hostRepository.UpdateByID(ID, ChangeHostname);
        }
        public virtual async Task<Result<HostCreated>> DeleteByID(int ID) => await hostRepository.DeleteByID(ID);
        public virtual async Task<Result<HostCreated>> DeleteByHostname(string Hostname)
        {
            List<Violation> violations = Validation(Hostname, nameof(Hostname));

            if (violations.Count > 0)
                return Result<HostCreated>.Fail(new Error("Не верный параметр", 400, violations));

            return await hostRepository.DeleteByHostname(Hostname);
        }
        public virtual async Task<Result<Host>> FindOrCreate(string Hostname)
        {
            List<Violation> violations = Validation(Hostname, nameof(Hostname));

            if (violations.Count > 1)
                return Result<Host>.Fail(new Error("Не верный параметр", 400, violations));

            return await hostRepository.FindOrCreate(Hostname);
        }
        public virtual async Task<Result<Host>> AddSoftwares(string Hostname, List<Host_AddSoftwares> softwares)
        {
            Result<Host> result = await GetByHostname(Hostname);

            if (!result.IsSuccess)
                return Result<Host>.Fail(result.Error);

            foreach (Host_AddSoftwares software in softwares)
            {
                Result<HostSoftware> resultHostSoftware = await hostSoftwareService.FindOrCreate(result.Object.Id, software);

                if (!resultHostSoftware.IsSuccess)
                    return Result<Host>.Fail(resultHostSoftware.Error);
            }

            return await GetByID(result.Object.Id);
        }
        public virtual async Task<Result<List<Host_AddSoftwares>>> CheckSoftwares(string Hostname, List<Host_AddSoftwares> softwares)
        {
            List<Host_AddSoftwares> need_delete = new List<Host_AddSoftwares>();

            Result<Host> result = await GetByHostname(Hostname);

            if (!result.IsSuccess)
                return Result<List<Host_AddSoftwares>>.Fail(result.Error);

            foreach (Host_AddSoftwares software in softwares)
            {
                Result<HostSoftware> resultHostSoftware = await hostSoftwareService.Find(result.Object.Id, software);

                if (!resultHostSoftware.IsSuccess)
                {
                    Host_AddSoftwares host_AddSoftwares = mapper.Map<Host_AddSoftwares>(software);
                    logger.LogWarning($"{DateTime.Now}: {software.Name} - {software.Publisher} - {software.Version} - {host_AddSoftwares}");
                    need_delete.Add(host_AddSoftwares);
                }
            }

            return Result<List<Host_AddSoftwares>>.Success(need_delete);
        }
    }
}
