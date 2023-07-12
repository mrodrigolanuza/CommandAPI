using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Profiles;
using Moq;
using Xunit;

namespace CommandAPI.test
{
    public class CommandsControllerTests
    {
        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //Arrange
            var mockRepo = new Mock<ICommandAPIRepo>();                                             //Repo mocking
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));

            var realProfile = new CommandsProfile();                                                //Not mocking Automapper
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            IMapper mapper = new Mapper(configuration);

            var controller = new CommandsController(mockRepo.Object, mapper);                       //Controller instance under test with dependencies configured

            //Act
            

            //Assert
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