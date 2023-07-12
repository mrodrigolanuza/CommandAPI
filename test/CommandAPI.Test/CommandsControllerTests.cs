using System;
using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.DTOs;
using CommandAPI.Models;
using CommandAPI.Profiles;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommandAPI.test
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests()
        {
            //Repo mocking
            mockRepo = new Mock<ICommandAPIRepo>();                                             
            //Not mocking Automapper, used real dependency
            realProfile = new CommandsProfile();                                                
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }

        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));
            var controller = new CommandsController(mockRepo.Object, mapper);  //Controller instance under test with dependencies configured

            //Act
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandItems_ReturnsOneItem_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);  

            //Act
            var result = controller.GetAllCommands();

            //Assert
            var okResult = (OkObjectResult)result.Result; 
            var commands = (List<CommandReadDTO>)okResult.Value;

            Assert.Single(commands);
        }

        [Fact]
        public void GetAllCommands_Returns200OK_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandByID_Returns404NotFound_WhenNonExistentIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandByID_Returns200OK__WhenValidIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1,
                                                                                 HowTo = "mock",
                                                                                 Platform = "Mock",
                                                                                 CommandLine = "Mock" });
            var controller = new CommandsController(mockRepo.Object, mapper);
            
            //Act
            var result = controller.GetCommandById(1);
            
            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandByID_ReturnsOneItem__WhenValidIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1,
                                                                                 HowTo = "mock",
                                                                                 Platform = "Mock",
                                                                                 CommandLine = "Mock" });
            var controller = new CommandsController(mockRepo.Object, mapper);
            
            //Act
            var result = controller.GetCommandById(1);
            
            //Assert
            Assert.IsType<ActionResult<CommandReadDTO>>(result);
        }

        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1,
                                                                                 HowTo = "mock",
                                                                                 Platform = "Mock",
                                                                                 CommandLine = "Mock" });
            var controller = new CommandsController(mockRepo.Object, mapper);
            
            //Act
            var result = controller.CreateCommand(new CommandCreateDTO { });
            
            //Assert
            Assert.IsType<ActionResult<CommandReadDTO>>(result);
        }

        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1,
                                                                                 HowTo = "mock",
                                                                                 Platform = "Mock",
                                                                                 CommandLine = "Mock" });
            var controller = new CommandsController(mockRepo.Object, mapper);
            
            //Act
            var result = controller.CreateCommand(new CommandCreateDTO { });
            
            //Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1,
                                                                                 HowTo = "mock",
                                                                                 Platform = "Mock",
                                                                                 CommandLine = "Mock" });
            var controller = new CommandsController(mockRepo.Object, mapper);
            
            //Act
            var result = controller.UpdateCommand(1, new CommandUpdateDTO { });
           
            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);
            
            //Act
            var result = controller.UpdateCommand(0, new CommandUpdateDTO { });
            
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.PartialCommandUpdate(0, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDTO>{ });

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns204NoContent_WhenValidResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1,
                                                                                 HowTo = "mock", 
                                                                                 Platform = "Mock", 
                                                                                 CommandLine = "Mock" });
            var controller = new CommandsController(mockRepo.Object, mapper);
            
            //Act
            var result = controller.DeleteCommand(1);
            
            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns_404NotFound_WhenNonExistentResourceIDSubmitted()
        {
        //Arrange
        mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
        var controller = new CommandsController(mockRepo.Object, mapper);
        
        //Act
        var result = controller.DeleteCommand(0);
        
        //Assert
        Assert.IsType<NotFoundResult>(result);
        }

        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if(num > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Migration Name>",
                    Platform = ".NET Core EF"
                });
            }

            return commands;                
        }
    }
}