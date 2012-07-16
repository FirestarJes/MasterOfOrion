using Console = DigitalRune.Game.UI.Controls.Console;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public class PropertySetCommand : ReflectionPropertyConsoleCommandBase
    {
        public PropertySetCommand(Console console, IConsoleCommandHost commandHost)
            : base("Set", console, commandHost)
        {
            Description = "Sets a Property [name, value]";
        }

        protected override void ExecuteCommand(string[] arguments)
        {
            SetPropertryValue(arguments[0], arguments[1]);
        }

        protected override int MinimumNumberOfArguments
        {
            get { return 2; }
        }
    }
}