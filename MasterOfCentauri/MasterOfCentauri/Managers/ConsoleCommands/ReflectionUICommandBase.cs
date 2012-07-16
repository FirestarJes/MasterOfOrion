using System;
using System.Linq;
using System.Reflection;
using DigitalRune.Game;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Controls;
using Console = DigitalRune.Game.UI.Controls.Console;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public abstract class ReflectionUiCommandBase : ReflectionPropertyConsoleCommandBase
    {
        protected ReflectionUiCommandBase(string name, Console console, IConsoleCommandHost commandHost)
            : base(name, console, commandHost)
        {
        }

        protected Button CreateButton(MethodInfo methodInfo, object[] args)
        {
            var button = new Button
                             {
                                 HorizontalAlignment = HorizontalAlignment.Stretch,
                                 VerticalAlignment = VerticalAlignment.Stretch
                             };
            button.Content = new TextBlock {Text = methodInfo.Name};
            button.Click += (x, y) => methodInfo.Invoke(CommandHost, args.ToArray());
            return button;
        }

        protected Slider CreateSlider(PropertyInfo propertyInfo, MethodInfo autoCall, float startValue, SliderCommand.Range range, Action<float> onValueChanged)
        {
            var slider = new Slider
                             {
                                 Minimum = range.Minimum,
                                 Maximum = range.Maximum,
                                 HorizontalAlignment = HorizontalAlignment.Stretch,
                                 VerticalAlignment = VerticalAlignment.Stretch,
                                 Value = startValue
                             };

            slider.PropertyChanged += SliderOnPropertyChanged(propertyInfo, onValueChanged, autoCall);
            return slider;
        }

        private EventHandler<GamePropertyEventArgs> SliderOnPropertyChanged(PropertyInfo propertyInfo, Action<float> onValueChanged, MethodInfo autoCall)
        {
            return (x, y) =>
                       {
                           if (y.Property.Name == "Value")
                           {
                               propertyInfo.SetValue(CommandHost, y.Property.Value, null);
                               onValueChanged((float)y.Property.Value);
                               
                               if (autoCall != null)
                               {
                                   autoCall.Invoke(CommandHost, null);
                               }
                           }
                       };
        }
    }
}