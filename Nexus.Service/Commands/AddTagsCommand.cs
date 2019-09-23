using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Core.Entities;
using Nexus.Data;

namespace Nexus.Service.Commands
{
    public interface ICommand
    {
        void Execute();
    }

    public class TransactionalCommandDecorator : ICommand
    {
        private readonly NexusContext _context;
        private readonly IEnumerable<ICommand> _decoratedCommands;
        private readonly ICommand _decoratedCommand;

        public TransactionalCommandDecorator(NexusContext context, IEnumerable<ICommand> decoratedCommands)
        {
            _context = context;
            _decoratedCommands = decoratedCommands;
        }

        public void Execute()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var command in _decoratedCommands.ToArray())
                        command.Execute();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public class AddTagsCommand : ICommand
    {
        private readonly NexusContext _context;
        private readonly IEnumerable<string> _tags;

        public AddTagsCommand(NexusContext context, IEnumerable<string> tags)
        {
            _context = context;
            _tags = tags;
        }

        public void Execute()
        {
            foreach (var incomingTag in _tags.ToArray())
            {
                var tag = new Tag { Title = incomingTag };

                _context.Set<Tag>().Add(tag);
            }

            _context.SaveChanges();
        }
    }

    public class SaveTagCommand : ICommand
    {
        private readonly NexusContext _context;
        private readonly ICommand _decoratedCommand;

        public SaveTagCommand(NexusContext context, ICommand decoratedCommand)
        {
            _context = context;
            _decoratedCommand = decoratedCommand;
        }

        public void Execute()
        {
            _decoratedCommand.Execute();
            _context.SaveChanges();
        }
    }
}
