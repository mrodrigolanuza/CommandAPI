using System;
using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.DTOs;
using CommandAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id}", Name="GetCommandById")]
        public ActionResult<Command> GetCommandById(int id)
        {
            var command = repository.GetCommandById(id);
            if(command is null)
                return NotFound();
            
            return Ok(mapper.Map<CommandReadDTO>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDTO> CreateCommand(CommandCreateDTO commandCreateDTO)
        {
            var commandModel = mapper.Map<Command>(commandCreateDTO);
            repository.CreateCommand(commandModel);
            repository.SaveChanges();
            
            var commandReadDTO = mapper.Map<CommandReadDTO>(commandModel);
            var result = CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDTO.Id}, commandReadDTO);
            return result;
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDTO commandUpdateDTO)
        {
            var commandModelFromRepo = repository.GetCommandById(id);
            if(commandModelFromRepo is null)
                return NotFound();

            mapper.Map(commandUpdateDTO, commandModelFromRepo);
            repository.UpdateCommand(commandModelFromRepo);
            repository.SaveChanges();
            
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDTO> patchDoc)
        {
            //JsonPatchDocument expected that applies to a CommandUpdateDTO >> patchDoc 

            //Get actual DB info for the id
            var commandModelFromRepo = repository.GetCommandById(id);
            if(commandModelFromRepo is null)
                return NotFound();

            var commandToPatch = mapper.Map<CommandUpdateDTO>(commandModelFromRepo);
            
            //Validations with Data Annotations: Data received applied to existing Data 
            patchDoc.ApplyTo(commandToPatch, ModelState);
            if(!TryValidateModel(commandToPatch))
                return ValidationProblem(ModelState);
            
            //Perform changes from commandToPatch to commandModelFromRepo
            mapper.Map(commandToPatch, commandModelFromRepo);
            repository.UpdateCommand(commandModelFromRepo);
            repository.SaveChanges();
            return NoContent();
        }
    }    
}