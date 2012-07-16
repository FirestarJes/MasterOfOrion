using System.Linq;
using DigitalRune.Game.UI.Controls;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public class ToStringCommand : CallCommand
    {
        public ToStringCommand(Console console, IConsoleCommandHost commandHost) : base("ToString", console, commandHost)
        {
            Description = "Writes out the objects ToString method";
        }

        protected override void ExecuteCommand(string[] arguments)
        {
            CallMethod("ToString", Enumerable.Empty<object>());
        }
    }
}