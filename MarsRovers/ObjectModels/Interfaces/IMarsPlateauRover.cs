using System.Collections.Generic;

namespace MarsRovers.ObjectModels
{
    public interface IMarsPlateauRover
    {
        IPlateauMatrixNavigator PlateauNavigator { get; }
        List<string> _roverAllCommands { get; }
        PlateauPosition CurrentPosition { get; }
        string Name { get; }
        void ExecuteAllCommands();
    }
}