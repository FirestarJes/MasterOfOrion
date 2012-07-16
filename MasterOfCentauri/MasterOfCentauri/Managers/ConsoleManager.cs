using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Consoles;
using DigitalRune.Game.UI.Controls;
using MasterOfCentauri.Managers.ConsoleCommands;
using Console = DigitalRune.Game.UI.Controls.Console;

namespace MasterOfCentauri.Managers
{
    public class ConsoleManager
    {
        public Console Console;

        protected List<IConsoleCommandHost> Hosts = new List<IConsoleCommandHost>();

        public ConsoleManager()
        {
            CreateConsoleControl();
        }

        
        public void HookInto(IConsoleCommandHost commandHost)
        {
            if (AlreadyHookedIntoCommandHost(commandHost))
                return;

            AddHost(commandHost);
            AddHostCommands(commandHost);
            AddReflectionCommands(commandHost);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        private void CreateConsoleControl()
        {
            Console = new Console
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }
        
        private void AddHostCommands(IConsoleCommandHost commandHost)
        {
            foreach (ConsoleCommand command in commandHost.Commands)
                AddHostCommand(commandHost, command);
        }

        private void AddHost(IConsoleCommandHost commandHost)
        {
            Hosts.Add(commandHost);
            AttachEventHandlerToRemoveCommands(commandHost);
            Console.WriteLine("Hooked into " + commandHost.Name);
        }

        private void AddHostCommand(IConsoleCommandHost commandHost, ConsoleCommand command)
        {
            ConsoleCommand localCommand = command;
            ConsoleCommand cmd = CreateScopedCommand(commandHost, command, localCommand);
            Console.Interpreter.Commands.Add(cmd);
        }

        private ConsoleCommand CreateScopedCommand(IConsoleCommandHost commandHost, ConsoleCommand command,
                                                          ConsoleCommand localCommand)
        {
            return new ConsoleCommand(commandHost.Name + "." + command.Name, command.Description,
                                      x => localCommand.Execute(x));
        }

        private void AttachEventHandlerToRemoveCommands(IConsoleCommandHost commandHost)
        {
            commandHost.RemoveCommands += (sender, e) => RemoveAllCommands(commandHost.Commands);
        }

        private bool AlreadyHookedIntoCommandHost(IConsoleCommandHost commandHost)
        {
            return Hosts.Any(x => x == commandHost);
        }

        private void AddReflectionCommands(IConsoleCommandHost commandHost)
        {
            Console.Interpreter.Commands.Add(new CallCommand(Console, commandHost));
            Console.Interpreter.Commands.Add(new PropertySetCommand(Console, commandHost));
            Console.Interpreter.Commands.Add(new PropertyGetCommand(Console, commandHost));
            Console.Interpreter.Commands.Add(new DumpCommand(Console, commandHost));
            Console.Interpreter.Commands.Add(new ToStringCommand(Console,commandHost));
            Console.Interpreter.Commands.Add(new GetTypeCommand(Console,commandHost));
            Console.Interpreter.Commands.Add(new SliderCommand(Console, commandHost));
            Console.Interpreter.Commands.Add(new MethodButtonCommand(Console, commandHost));
        }

        private void RemoveAllCommands(IEnumerable<ConsoleCommand> commands)
        {
            foreach (ConsoleCommand command in commands)
                Console.Interpreter.Commands.Remove(command);
        }

    }
}