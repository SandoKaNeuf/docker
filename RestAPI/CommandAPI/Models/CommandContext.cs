using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CommandAPI.Models
{
    public class CommandConext : DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> options) :base(options)
        {

        }

        public DbSet<Command> CommandItems {get; set;}
    }
}