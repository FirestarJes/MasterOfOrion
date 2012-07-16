using System.Reflection;
using DigitalRune.Game.UI.Controls;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public class DumpCommand : ReflectionPropertyConsoleCommandBase
    {
        public DumpCommand(Console console, IConsoleCommandHost commandHost) : base("Dump", console, commandHost)
        {
            Description = "Dumps all the objects properties to the console";
        }

        protected override void ExecuteCommand(string[] arguments)
        {
            Dump();
        }

        protected override int MinimumNumberOfArguments
        {
            get { return 0; }
        }

        private void Dump()
        {
            PropertyInfo[] properties = CommandHost.GetType().GetProperties();

            Console.WriteLine(CommandHostType.Name + ":");
            foreach (PropertyInfo prop in properties)
            {
                object value = prop.GetValue(CommandHost, null);
                if (value == null)
                    value = "NULL";

                value = value.ToString().Replace("\n", "\n   ");
                Console.WriteLine("   " + prop.Name + ": " + value);
            }
        }

    }
}