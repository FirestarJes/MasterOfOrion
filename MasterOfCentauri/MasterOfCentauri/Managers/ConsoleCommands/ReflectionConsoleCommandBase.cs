using System;
using System.Collections.Generic;
using System.Linq;
using DigitalRune.Game.UI.Consoles;
using Console = DigitalRune.Game.UI.Controls.Console;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public abstract class ReflectionConsoleCommandBase : ConsoleCommand
    {
        protected Console Console { get; private set; }
        protected IConsoleCommandHost CommandHost  { get; private set; }
        protected Type CommandHostType;

        protected ReflectionConsoleCommandBase(string name, Console console, IConsoleCommandHost commandHost) : base(commandHost.Name + "." + name)
        {
            Console = console;
            CommandHost = commandHost;
            Execute = x => InvokeCommand(x.Skip(1).ToArray());
            CommandHostType = CommandHost.GetType();
        }

        private void InvokeCommand(string[] arguments)
        {


            try
            {
                ValidateNumberOfArguments(arguments);

                ExecuteCommand(arguments);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not execute command: " + ex.Message);
            }
        }

        private void ValidateNumberOfArguments(string[] arguments)
        {
            if (arguments.Length < MinimumNumberOfArguments)
            {
                throw new ArgumentException(string.Format(
                    "{0} must be called with at least {1} argument. See help for more information.", Name,
                    MinimumNumberOfArguments));
            }
        }

        protected abstract void ExecuteCommand(string[] arguments);
        protected abstract int MinimumNumberOfArguments { get; }
    }
}