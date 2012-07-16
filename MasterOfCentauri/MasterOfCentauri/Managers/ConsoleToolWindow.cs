using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Controls;

namespace MasterOfCentauri.Managers
{
    public class ConsoleToolWindow : Window
    {
        public StackPanel StackPanel { get; set; }

        public ConsoleToolWindow(IConsoleCommandHost host, string methodOrPropertyName)
        {
            Height = 70;
            CanResize = true;
            Title = host.Name + "." + methodOrPropertyName;
            StackPanel = new StackPanel
                             {
                                 HorizontalAlignment = HorizontalAlignment.Stretch,
                                 VerticalAlignment = VerticalAlignment.Stretch
                             };

            Content = StackPanel;
        }
    }
}