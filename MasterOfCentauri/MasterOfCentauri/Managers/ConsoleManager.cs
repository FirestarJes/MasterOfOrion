using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Consoles;
using MasterOfCentauri.UIControls;
using Console = DigitalRune.Game.UI.Controls.Console;

namespace MasterOfCentauri.Managers
{
    public class ConsoleManager
    {
        public Console Console;

        protected List<IConsoleCommandHost> Hosts = new List<IConsoleCommandHost>();

        public ConsoleManager()
        {
            Console = new Console
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }

        public void HookInto(IConsoleCommandHost commandHost)
        {
            if (Hosts.Any(x => x == commandHost))
                return;

            Hosts.Add(commandHost);

            Console.WriteLine("Hooked into " + commandHost.Name);

            commandHost.RemoveCommands += (sender, e) => RemoveAllCommands(commandHost.Commands);
            foreach (var command in commandHost.Commands)
            {
                var localCommand = command;
                var cmd = new ConsoleCommand(commandHost.Name + "." + command.Name, command.Description, x => localCommand.Execute(x));
                Console.Interpreter.Commands.Add(cmd);

                
            }
            AddReflectionCommands(commandHost);
        }

        private void AddReflectionCommands(IConsoleCommandHost commandHost)
        {
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Call", "Calls a method on the object", x => CallMethod(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Set", "Sets a Property", x => SetProperty(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Get", "Gets a Property", x => GetProperty(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Dump", "Dumps all property values in console", x => Dump(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "ToString", "Writes out the objects ToString method", x => CallToString(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "GetType", "Writes out the objects ToString method", x => CallGetType(commandHost, x)));
        }

        private void CallToString(IConsoleCommandHost commandHost, string[] strings)
        {
            CallMethod(commandHost,new string[] {strings[0], "ToString"});
        }

        private void CallGetType(IConsoleCommandHost commandHost, string[] strings)
        {
            CallMethod(commandHost, new string[] { strings[0], "GetType" });
        }

        private void Dump(IConsoleCommandHost commandHost, string[] strings)
        {
            var properties = commandHost.GetType().GetProperties();

            Console.WriteLine(commandHost.GetType().Name + ":");
            foreach(var prop in properties)
            {
                var value = prop.GetValue(commandHost, null);
                if (value == null)
                    value = "NULL";

                value = value.ToString().Replace("\n", "\n   ");
                Console.WriteLine("   " + prop.Name + ": " + value);   
            }
        }

        private void GetProperty(IConsoleCommandHost commandHost, string[] arguments)
        {
            var type = commandHost.GetType();
            var property = type.GetProperty(arguments[1]);
            
            Console.WriteLine(property.GetValue(commandHost, null).ToString());
        }

        private void SetProperty(IConsoleCommandHost commandHost, string[] arguments)
        {
            var type = commandHost.GetType();
            var property = type.GetProperty(arguments[1]);
            var value = arguments[2];

            property.SetValue(commandHost, Convert.ChangeType(value, property.PropertyType), null);
        }

        private void CallMethod(IConsoleCommandHost commandHost, string[] arguments)
        {
            var type = commandHost.GetType();
            var method = type.GetMethod(arguments[1]);
            var args = GetArguments(arguments);

            var response = method.Invoke(commandHost, args.ToArray());

            if(response != null)
                Console.WriteLine(response.ToString());
        }

        private static List<object> GetArguments(string[] arguments)
        {
            var args = new List<object>();
            args.AddRange(arguments);
            args.RemoveAt(0);
            args.RemoveAt(0);
            return args;
        }

        private void RemoveAllCommands(IEnumerable<ConsoleCommand> commands)
        {
            foreach (var command in commands)
                Console.Interpreter.Commands.Remove(command);
        }
    }
}