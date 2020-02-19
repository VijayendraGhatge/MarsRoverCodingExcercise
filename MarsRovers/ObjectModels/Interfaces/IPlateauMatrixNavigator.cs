using System;

namespace MarsRovers.ObjectModels
{
    public interface IPlateauMatrixNavigator
    {
        PlateauPosition StartPosition { get; }

        PlateauPosition CurrentPosition { get; }

        Tuple<int, int> PlateauMatrix { get; }

        void GotoStartPosition(string startPosCmdTxt);

        void ExecuteCommand(BaseCommand command);

        void TurnRover(TurnCommand turnCommand);

        void PositionRover(PositioningCommand positioningCommand);

        void MoveRover(PositioningCommand positioningCommand);
    }
}
