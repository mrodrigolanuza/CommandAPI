using System;
using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.DTOs;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo repository;
        private readonly IMapper mapper;

        public CommandsController(ICommandAPIRepo repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDTO>> GetAllCommands()
        {
            var commandItems = repository.GetAllCommands();
            return Ok(mapper.Map<IEnumerable<CommandReadDTO>>(commandItems));
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Command> GetCommandById(int id)
        {
            var command = repository.GetCommandById(id);
            if(command is null)
                return NotFound();
            
            return Ok(mapper.Map<CommandReadDTO>(command));
        }
    }    
}