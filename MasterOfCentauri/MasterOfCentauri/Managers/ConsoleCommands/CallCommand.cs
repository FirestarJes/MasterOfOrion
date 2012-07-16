using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console = DigitalRune.Game.UI.Controls.Console;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public class CallCommand : ReflectionConsoleCommandBase
    {
        public CallCommand(string name, Console console, IConsoleCommandHost commandHost) : base(name, console, commandHost)
        {
            Description = "Calls a method on the object [method,(args*)]";
        }

        public CallCommand(Console console, IConsoleCommandHost commandHost) : this("Call", console, commandHost)
        {
        }

        protected override void ExecuteCommand(string[] arguments)
        {
            CallMethod(arguments.First(), arguments.Skip(1));
        }

        protected override int MinimumNumberOfArguments
        {
            get { return 1; }
        }

        protected void CallMethod(string name, IEnumerable<object> arguments)
        {
            try
            {
                MethodInfo method = CommandHostType.GetMethod(name);

                if(method == null)
                {
                    Console.WriteLine("Could not find a method with that name");
                    return;
                }
                
                object response = method.Invoke(CommandHost, arguments.ToArray());

                if (response != null)
                    Console.WriteLine(response.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not call method: " + ex.Message);
            }
        }
    }
}