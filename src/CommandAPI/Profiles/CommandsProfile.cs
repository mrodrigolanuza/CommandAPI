using AutoMapper;
using CommandAPI.DTOs;
using CommandAPI.Models;

namespace CommandAPI.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            //Source >> Target
            CreateMap<Command, CommandReadDTO>();
            CreateMap<CommandCreateDTO, Command>();
            CreateMap<CommandUpdateDTO, Command>();
            CreateMap<Command, CommandUpdateDTO>();
        }
    }
}