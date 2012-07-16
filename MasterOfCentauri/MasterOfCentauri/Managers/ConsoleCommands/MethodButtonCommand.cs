using System.Linq;
using System.Reflection;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Controls;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public class MethodButtonCommand : ReflectionUiCommandBase
    {
        public MethodButtonCommand(Console console, IConsoleCommandHost commandHost)
            : base("Button", console, commandHost)
        {
            Description = "Creates a Button To Call A Method [methods,(args*)]";
        }

        protected override void ExecuteCommand(string[] arguments)
        {
            MethodInfo methodInfo = CommandHost.GetType().GetMethod(arguments.First());

            if (methodInfo == null)
            {
                Console.WriteLine("Could not create button. Property not found");
                return;
            }

            MethodButton(methodInfo, arguments.Skip(1).Cast<object>().ToArray());
        }

        protected override int MinimumNumberOfArguments
        {
            get { return 1; }
        }

        private void MethodButton(MethodInfo methodInfo, object[] args)
        {
            var win = new ConsoleToolWindow(CommandHost, methodInfo.Name);

            var button = CreateButton(methodInfo, args);
            win.StackPanel.Children.Add(button);
            win.Show(Console);
        }
    }
}