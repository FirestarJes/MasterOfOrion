using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Consoles;
using DigitalRune.Game.UI.Controls;
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
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Call", "Calls a method on the object [method,(args*)]", x => CallMethod(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Set", "Sets a Property [name, value]", x => SetProperty(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Get", "Gets a Property [name]", x => GetProperty(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Dump", "Dumps all property values in console", x => Dump(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "ToString", "Writes out the objects ToString method", x => CallToString(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "GetType", "Writes out the objects ToString method", x => CallGetType(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Slider", "Creates a Slider To Set The Property [property,(min),(max),(autocall)]", x => Slider(commandHost, x)));
            Console.Interpreter.Commands.Add(new ConsoleCommand(commandHost.Name + "." + "Button", "Creates a Button To Call A Method [methods,(args*)]", x => MethodButton(commandHost, x)));
        }

        private void MethodButton(IConsoleCommandHost commandHost, string[] strings)
        {
            var win = new Window();
            win.Height = 70;
            win.CanResize = true;
            var button = new Button() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };

            var methodName = strings[1];
            var methodInfo = commandHost.GetType().GetMethod(methodName);

            button.Content = new TextBlock {Text = methodName};
            win.Title = commandHost.Name;
            win.Show(Console);

            var args = GetArguments(strings);
            
            button.Click += (x, y) => methodInfo.Invoke(commandHost, args.ToArray());

            win.Content = button;
        }

        private void Slider(IConsoleCommandHost commandHost, string[] strings)
        {
            var win = new Window();
            win.Height = 70;
            win.CanResize = true;
            var stackPanel = new StackPanel()
                                 {
                                     HorizontalAlignment = HorizontalAlignment.Stretch,
                                     VerticalAlignment = VerticalAlignment.Stretch
                                 };
            var textBlock = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center};
            
            var propertyName = strings[1];
            var propertyInfo = commandHost.GetType().GetProperty(propertyName);
            var startValue = (float)Convert.ChangeType(propertyInfo.GetValue(commandHost, null), typeof(float));
            textBlock.Text = startValue.ToString();
            win.Title = commandHost.Name + "." + propertyName;
            win.Show(Console);

            float mimimum = 0;
            float maximum = 0;

            string autoCall = null;

            if(strings.Length > 2)
            {
                mimimum = float.Parse(strings[2]);
                maximum = float.Parse(strings[2]);
            }

            if (mimimum == 0f)
                mimimum = startValue - (startValue);
            if (mimimum == 0f)
                maximum = startValue + (startValue);

            if(strings.Length > 4)
            {
                autoCall = strings[4];
            }
            var slider = new Slider()
                             {
                                 Minimum = mimimum,
                                 Maximum = maximum,
                                 HorizontalAlignment = HorizontalAlignment.Stretch,
                                 VerticalAlignment = VerticalAlignment.Stretch,
                                 Value = startValue
                             };
            slider.PropertyChanged += (x, y) =>
                                          {
                                              if (y.Property.Name == "Value")
                                              {
                                                  propertyInfo.SetValue(commandHost, y.Property.Value, null);
                                                  textBlock.Text = y.Property.Value.ToString();

                                                  if(autoCall != null)
                                                  {
                                                      var method = commandHost.GetType().GetMethod(autoCall);
                                                      method.Invoke(commandHost, null);
                                                  }
                                              }
                                          };

            stackPanel.Children.Add(slider);
            stackPanel.Children.Add(textBlock);
            win.Content = stackPanel;
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

        private static List<object> GetArguments(string[] arguments, int toSkip = 2)
        {
            var args = new List<object>();
            args.AddRange(arguments);

            for (int x = 1; x <= toSkip;x++ )
                args.RemoveAt(0);
            
            return args;
        }

        private void RemoveAllCommands(IEnumerable<ConsoleCommand> commands)
        {
            foreach (var command in commands)
                Console.Interpreter.Commands.Remove(command);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}