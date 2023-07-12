using System;
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
            if(cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            dbcontext.CommandItems.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
           if(cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            dbcontext.CommandItems.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return dbcontext.CommandItems.ToList();
        }

        public Command GetCommandById(int id)
        {
            return dbcontext.CommandItems.FirstOrDefault(cmd => cmd.Id == id);
        }

        public void UpdateCommand(Command cmd)
        {
            //Nothing to do.
        }

        public bool SaveChanges()
        {
            return (dbcontext.SaveChanges() >= 0);
        }
    }
}