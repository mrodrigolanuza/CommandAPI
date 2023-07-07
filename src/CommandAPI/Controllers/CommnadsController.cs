using System;
using System.Collections.Generic;
using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo repository;

        public CommandsController(ICommandAPIRepo repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(repository.GetAllCommands());
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Command> GetCommandById(int id)
        {
            var command = repository.GetCommandById(id);
            if(command is null)
                return NotFound();
            
            return Ok(command);
        }
    }    
}