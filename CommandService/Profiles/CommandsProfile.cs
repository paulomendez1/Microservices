﻿using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;
using PlatformService;

namespace CommandService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<CommandCreateDTO, Command>();
            CreateMap<Command, CommandReadDTO>();
            CreateMap<PlatformPublishedDTO, Platform>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcPlatformModel, Platform>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
                                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                                    .ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}
