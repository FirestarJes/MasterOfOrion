using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRune.Game.UI.Consoles;

namespace MasterOfCentauri.Managers
{
    public interface IConsoleCommandHost
    {
        IEnumerable<ConsoleCommand> Commands { get; }
        string Name { get; }
        event EventHandler RemoveCommands;
    }
}