using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Models;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly CommandContext _context;

        public CommandsController(CommandContext context) {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetCommandItems()
        {
            return _context.CommandItems;
        }
        // public ActionResult<IEnumerable<string>> Get()
        // {
        // return new string[] {"this", "is", "hard", "coded"};
        // }

        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandItem(int id)
        {
            var commandItem = _context.CommandItems.Find(id);

            if (commandItem == null)
            {
                return NotFound();
            }

            return commandItem;
        }

        [HttpGet("{id}")]
        public CommandAPI Get(int id) => _context.CommandItems.Find(id);

        [HttpPost]
        public ActionResult<CommandAPI> PostCommandItem(CommandAPI command)
        {
            _context.CommandItems.Add(command);
            _context.SaveChanges();

            return CreatedAtAction("GetCommandItem", new Command{Id = command.Id}, command);
        }
    }
}