using System.Collections.Generic;
using System.Linq;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    public class SqlCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandContext dbcontext;

        public SqlCommandAPIRepo(CommandContext context)
        {
            dbcontext = context;
        }

        public void CreateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return dbcontext.CommandItems.ToList();
        }

        public Command GetCommandById(int id)
        {
            return dbcontext.CommandItems.FirstOrDefault(cmd => cmd.Id == id);
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}