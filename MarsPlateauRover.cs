using System;
using System.Collections.Generic;

namespace MarsRovers.ObjectModels
{
    public class MarsPlateauRover : IMarsPlateauRover
    {
        private IPlateauMatrixNavigator _navigator;
        public IPlateauMatrixNavigator PlateauNavigator => _navigator;

        public string Name { get; }

        public List<string> _roverAllCommands { get; }

        public MarsPlateauRover(string roverName, Tuple<int, int> plateauMatrix, List<string> roverCmds)
        {
            if (roverCmds == null && roverCmds.Count > 1)
                throw new ArgumentNullException("Commands to the Rover cannot be null or empty.");

            if (plateauMatrix == null)
                throw new ArgumentNullException("Plateau matrix to navigate cannot be empty.");

            Name = roverName;
            _roverAllCommands = roverCmds;
            _navigator = new PlateauNavigator(plateauMatrix);
        }

        public void ExecuteAllCommands()
        {
            List<string> onlyNavCmds = _roverAllCommands;
            PlateauNavigator.GotoStartPosition(onlyNavCmds[0]);
            onlyNavCmds.RemoveAt(0);
            foreach (string cmdTxt in onlyNavCmds)
            {
                BaseCommand command = CommandGenerator.GenerateAppropriateCommand(cmdTxt);
                PlateauNavigator.ExecuteCommand(command);
            }
        }

        public PlateauPosition CurrentPosition { get { return PlateauNavigator.CurrentPosition; } }
    }
}
