using DigitalRune.Game.UI.Controls;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public class PropertyGetCommand : ReflectionPropertyConsoleCommandBase
    {
        public PropertyGetCommand(Console console, IConsoleCommandHost commandHost)
            : base("Get", console, commandHost)
        {
            Description = "Gets a Property [name]";
        }

        protected override void ExecuteCommand(string[] arguments)
        {
            Console.WriteLine(GetPropertryValue(arguments[0]).ToString());
        }

        protected override int MinimumNumberOfArguments
        {
            get { return 1; }
        }
    }
}