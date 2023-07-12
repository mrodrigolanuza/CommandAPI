using System;
using CommandAPI.Models;
using Xunit;

namespace CommandAPI.Test
{
    public class CommandTests : IDisposable
    {
        Command testCommand;

        //Constructor is called for every test
        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do Something",
                Platform = "Testing Platform",
                CommandLine = "dotnet test"
            };    
        }

        //Clean up resources for every test
        public void Dispose()
        {
            testCommand = null;
        }

        [Fact]
        public void CanChangeHowTo()
        {
            //Arrange
           
            //Act
            testCommand.HowTo = "Execute Unit Tests";

            //Assert
            Assert.Equal("Execute Unit Tests", testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {
            //Arrange
           
            //Act
            testCommand.Platform = "New Testing Platform";

            //Assert
            Assert.Equal("New Testing Platform", testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
            //Arrange
           
            //Act
            testCommand.CommandLine = "dotnet build";

            //Assert
            Assert.Equal("dotnet build", testCommand.CommandLine);
        }
    }
}