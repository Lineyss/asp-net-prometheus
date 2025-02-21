using AutoMapper;
using Server.DataAccess.Entities;
using Server.Domain.Models.DTO.Host;
using Server.Domain.Models.DTO.HostSoftware;
using Server.Domain.Models.DTO.Publisher;
using Server.Domain.Models.DTO.Software;

namespace Server.API.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<HostEntities, Domain.Models.DTO.Host.Host>()
                .ForMember(element => element.Softwares, opt => opt.MapFrom(host =>
                    host.HostSoftwares != null ? host.HostSoftwares.Select(host_software => new Host_HostSoftware
                    {
                        Id = host_software.Id,
                        Added = host_software.Added,
                        Software = new Host_Software
                        {
                            Id = host_software.Software.Id,
                            Name = host_software.Software.Name,
                            Version = host_software.Software.Version,
                            Publisher = host_software.Software.Publisher == null ? null : new Host_Publisher
                            {
                                Id = host_software.Software.Publisher.Id,
                                Title = host_software.Software.Publisher.Title
                            }
                        }
                    }) : new List<Host_HostSoftware>()
                ));

            CreateMap<HostEntities, HostCreated>()
                .ForMember(element => element.Title, opt => opt.MapFrom(src => src.Hostname))
                .ForMember(element => element.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<Domain.Models.DTO.Host.Host, HostCreated>()
                .ForMember(element => element.Title, opt => opt.MapFrom(src => src.Hostname))
                .ForMember(element => element.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<PublisherEntities, Publisher>()
                .ForMember(element => element.Softwares, opt => opt.MapFrom(publisher =>
                    publisher.Softwares != null ? publisher.Softwares.Select(software => new Publisher_Software
                    {
                        Id = software.Id,
                        Name = software.Name,
                        Version = software.Version,
                        Hosts = software.HostSoftwares.Select(host_software => new Publisher_HostSoftware
                        {
                            Id = host_software.Id,
                            Added = host_software.Added,
                            Host = new Publisher_Host
                            {
                                Id = host_software.Host.Id,
                                Hostname = host_software.Host.Hostname
                            }
                        }).ToList()
                    }) : null
                ));

            CreateMap<PublisherEntities, PublisherCreated>()
              .ForMember(element => element.ID, opt => opt.MapFrom(src => src.Id))
              .ForMember(element => element.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<SoftwareEntities, Software>()
                .ForMember(element => element.Publisher, opt => opt.MapFrom(software =>
                    software.Publisher != null ? new Software_Publisher
                    {
                        Id = software.Publisher.Id,
                        Title = software.Publisher.Title
                    } : null
                ))
                .ForMember(element => element.Hosts, opt => opt.MapFrom(software =>
                    software.HostSoftwares.Select(host_software => new Software_HostSoftware
                    {
                        Id = host_software.Id,
                        Added = host_software.Added,
                        Host = new Software_Host
                        {
                            Id = host_software.Host.Id,
                            Hostname = host_software.Host.Hostname
                        }
                    })
                ));

            CreateMap<SoftwareEntities, SoftwareCreated>()
                .ForMember(element => element.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(element => element.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(element => element.Version, opt => opt.MapFrom(src => src.Version))
                .ForMember(element => element.Publisherid, opt => opt.MapFrom(src => src.Publisherid))
                .ReverseMap();

            CreateMap<SoftwareCreate, SoftwareEntities>()
                .ForMember(element => element.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(element => element.Version, opt => opt.MapFrom(src => src.Version))
                .ForMember(element => element.Publisherid, opt => opt.MapFrom(src => src.Publisherid))
                .ReverseMap();

            CreateMap<SoftwareCreated, Software>()
                .ForMember(element => element.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(element => element.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(element => element.Version, opt => opt.MapFrom(src => src.Version))
                .ForMember(element => element.Hosts, opt => opt.MapFrom(src => new List<Software_HostSoftware>()));

            CreateMap<SoftwareCreated, SoftwareCreate>()
                .ForMember(element => element.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(element => element.Version, opt => opt.MapFrom(src => src.Version))
                .ForMember(element => element.Publisherid, opt => opt.MapFrom(src => src.Publisherid));

            CreateMap<PublisherCreated, Publisher>()
                .ForMember(element => element.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(element => element.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(element => element.Softwares, opt => opt.MapFrom(src => new List<Publisher_Software>()));

            CreateMap<Domain.Models.DTO.Host.Host, HostCreated>()
                .ForMember(element => element.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(element => element.Title, opt => opt.MapFrom(src => src.Hostname));

            CreateMap<HostSoftwareEntities, HostSoftware>()
                .ForMember(element => element.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(element => element.Added, opt => opt.MapFrom(src => src.Added))
                .ForMember(element => element.Host, opt => opt.MapFrom(src => src.Host != null ?
                    new HostCreated
                    {
                        Id = src.Host.Id,
                        Title = src.Host.Hostname
                    } : null
                ))
                .ForMember(element => element.Software, opt => opt.MapFrom(src => src.Software != null ?
                    new HostSoftware_Software
                    {
                        ID = src.Software.Id,
                        Name = src.Software.Name,
                        Version = src.Software.Version,
                        Publisher = src.Software.Publisher != null ? new PublisherCreated
                        {
                            ID = src.Software.Publisher.Id,
                            Title = src.Software.Publisher.Title
                        } : null
                    } : null
                ));

            CreateMap<HostSoftwareEntities, HostSoftwareCreated>()
                .ForMember(element => element.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(element => element.Added, opt => opt.MapFrom(src => src.Added))
                .ForMember(element => element.Hostid, opt => opt.MapFrom(src => src.Hostid))
                .ForMember(element => element.Softwareid, opt => opt.MapFrom(src => src.Softwareid));

        }

    }
}

