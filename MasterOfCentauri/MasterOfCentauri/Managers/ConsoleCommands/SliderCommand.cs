using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DigitalRune.Game;
using DigitalRune.Game.UI;
using DigitalRune.Game.UI.Controls;
using Console = DigitalRune.Game.UI.Controls.Console;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public class SliderCommand : ReflectionUiCommandBase
    {
        public SliderCommand(Console console, IConsoleCommandHost commandHost)
            : base("Slider", console, commandHost)
        {
            Description = "Creates a Slider To Set The Property [property,(min),(max),(autocall)]";
        }

        protected override void ExecuteCommand(string[] arguments)
        {
            try
            {
                var sliderParams = new SliderParams(CommandHost, arguments);
                Slider(sliderParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not create slider:" + ex.Message);
            }
        }

        protected override int MinimumNumberOfArguments
        {
            get { return 1; }
        }

        private void Slider(SliderParams sliderParams)
        {
            var intialValue = GetInitialValue(sliderParams);
            var range = GetRange(intialValue, sliderParams.Minimum, sliderParams.Maximum);
            
            var toolWindow = new ConsoleToolWindow(CommandHost, sliderParams.PropertyToControl.Name);
            
            var textBlock = CreateTextBoxShowingCurrentValue(intialValue);
            var slider = CreateSlider(sliderParams.PropertyToControl, sliderParams.AutoCall, intialValue, range, SetTextBoxValue(textBlock));

            toolWindow.StackPanel.Children.Add(slider);
            toolWindow.StackPanel.Children.Add(textBlock);

            toolWindow.Show(Console);
        }

        private static Action<float> SetTextBoxValue(TextBlock textBlock)
        {
            return x=>textBlock.Text = x.ToString();
        }

        private float GetInitialValue(SliderParams sliderParams)
        {
            return (float) Convert.ChangeType(GetPropertryValue(sliderParams.PropertyToControl.Name), typeof (float));
        }

        private static TextBlock CreateTextBoxShowingCurrentValue(float startValue)
        {
            var textBlock = new TextBlock
                                {
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center
                                };
            textBlock.Text = startValue.ToString();
            return textBlock;
        }

        private static Range GetRange(float startValue, float mimimum, float maximum)
        {
            // If the value is zero we will insert some default values
            if(startValue != 0)
            {
                if (mimimum == 0f)
                    mimimum = startValue - (startValue);
                if (mimimum == 0f)
                    maximum = startValue + (startValue);
            }
            else
            {
                if (mimimum == 0 && maximum == 0)
                {
                    mimimum = -100;
                    maximum = 100;
                }
            }

            return new Range() { Maximum = maximum, Minimum = mimimum};
        }

        public class Range
        {
            public float Minimum { get; set; }
            public float Maximum { get; set; }
        }

        public class SliderParams
        {
            public SliderParams(IConsoleCommandHost commandHost, string[] arguments)
            {
                
                string propertyName = arguments[0];
                PropertyToControl = commandHost.GetType().GetProperty(propertyName);
                if (PropertyToControl == null)
                {
                    throw new ArgumentException("Property not found");
                }

                if (arguments.Length > 2)
                {
                    try
                    {
                        Minimum = float.Parse(arguments[1]);
                        Maximum = float.Parse(arguments[2]);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("Minimum or Maximum not a valid float", ex);
                    }
                }

                if (arguments.Length > 3)
                {
                    AutoCall = commandHost.GetType().GetMethod(arguments[3]);

                    if (AutoCall == null)
                    {
                        throw new ArgumentException("AutoCall Method not found");
                    }
                }
            }

            public PropertyInfo PropertyToControl { get; set; }
            public float Minimum { get; set; }
            public float Maximum { get; set; }
            public MethodInfo AutoCall { get; set; }
        }
    }
}