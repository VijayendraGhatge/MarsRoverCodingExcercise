using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRovers.ObjectModels
{
    public class TurnCommand : BaseCommand
    {
        public TurnCommand(string rawTxtCmd, NavigationDirection turnToDirection) : base (rawTxtCmd) 
        {
            TurnToDirection = turnToDirection;
        }

        public NavigationDirection TurnToDirection { get; }
    }

    public class PositioningCommand : BaseCommand
    {
        public PositioningCommand(string rawTxtCmd) : base(rawTxtCmd) { }
    }

    public class MultiMoveCommand : BaseCommand
    {
        public MultiMoveCommand(string rawTxtCmd) : base (rawTxtCmd) { }
    }

    public class CommandGenerator 
    {
        public static BaseCommand GenerateAppropriateCommand(string rawTxtCmd)
        {
            if (string.IsNullOrEmpty(rawTxtCmd))
                throw new ArgumentNullException("Command text cannot be null or empty.");

            BaseCommand command = null;
            string[] parameters = rawTxtCmd.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            
            switch(parameters.Length)
            {
                case 1:
                default:
                    command = new MultiMoveCommand(parameters[0]);
                    break;
                case 2: // Both the commands e.g. 5 5 and 1 2 N moves/positions the rover to posiotion
                case 3: // they are both treated as Positioning Command, but the processing differs
                    // Both have common characteristics i.e. Coordinates and Direction.
                    command = new PositioningCommand(rawTxtCmd);
                    break;
            }

            return command;
        }
    }
}