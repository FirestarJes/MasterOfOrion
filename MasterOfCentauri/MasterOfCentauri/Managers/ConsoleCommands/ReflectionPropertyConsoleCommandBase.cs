using System;
using System.Reflection;
using Console = DigitalRune.Game.UI.Controls.Console;

namespace MasterOfCentauri.Managers.ConsoleCommands
{
    public abstract class ReflectionPropertyConsoleCommandBase : ReflectionConsoleCommandBase
    {
        protected ReflectionPropertyConsoleCommandBase(string name, Console console, IConsoleCommandHost commandHost) : base(name, console, commandHost)
        {
        }

        protected void SetPropertryValue(string name, string value)
        {
            try
            {
                PropertyInfo property = CommandHostType.GetProperty(name);
                property.SetValue(CommandHost, Convert.ChangeType(value, property.PropertyType), null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not set property: " + ex.Message);
            }
        }

        protected object GetPropertryValue(string name)
        {
            try
            {
                PropertyInfo property = CommandHostType.GetProperty(name);

                return property.GetValue(CommandHost, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not get property: " + ex.Message);
                return null;
            }
        }
    }
}