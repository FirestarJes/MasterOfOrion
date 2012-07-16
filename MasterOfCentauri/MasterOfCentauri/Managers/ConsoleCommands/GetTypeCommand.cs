using System.Linq;
using DigitalRune.Game.UI.Controls;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public class GetTypeCommand : CallCommand
    {
        public GetTypeCommand(Console console, IConsoleCommandHost commandHost)
            : base("GetType", console, commandHost)
        {
            Description = "Writes out the objects type";
        }

        protected override void ExecuteCommand(string[] arguments)
        {
            CallMethod("GetType", Enumerable.Empty<object>());
        }
    }
}