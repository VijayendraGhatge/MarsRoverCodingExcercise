using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRovers.ObjectModels
{
    public class PlateauNavigator : IPlateauMatrixNavigator
    {
        #region Variables

        private PlateauPosition _startPosition;
        private PlateauPosition _currentPosition;

        #endregion Variables

        #region Constructor and Properties
        public PlateauNavigator(Tuple<int, int> plateauMatrix)
        {
            if (plateauMatrix == null)
                throw new ArgumentNullException("Plateau Matrix cannot be null.");

            PlateauMatrix = plateauMatrix;
        }

        public PlateauPosition StartPosition => _startPosition;

        public PlateauPosition CurrentPosition => _currentPosition;

        public Tuple<int, int> PlateauMatrix { get; }

        #endregion Constructor and Properties

        #region Private Methods
        private List<BaseCommand> GenerateAllCommands(string rawCmdTxt)
        {
            List<BaseCommand> genCmds = new List<BaseCommand>();
            rawCmdTxt.ToCharArray().ToList().ForEach(c =>
            {
                switch (c)
                {
                    case 'M':
                        genCmds.Add(new PositioningCommand("M"));
                        break;
                    case 'L':
                        //genCmds.Add(new TurnCommand("L", NavigationDirection.L, faceToDirection));
                        genCmds.Add(new TurnCommand("L", NavigationDirection.L));
                        break;
                    case 'R':
                        //genCmds.Add(new TurnCommand("R", NavigationDirection.R, faceToDirection));
                        genCmds.Add(new TurnCommand("R", NavigationDirection.R));
                        break;
                }
            });
            return genCmds;
        }
        private void ExecuteMultiMoveCommand(MultiMoveCommand multiMoveCommand)
        {
            List<BaseCommand> genMMCmds = GenerateAllCommands(multiMoveCommand.RawCmdText);
            genMMCmds.ForEach(cmd =>
            {
                if (cmd is TurnCommand)
                    ExecuteTurnCommand(cmd as TurnCommand);
                else
                    ExecuteMoveCommand(cmd as PositioningCommand);
            });
        }

        private void ExecuteTurnCommand(TurnCommand turnCommand)
        {
            CompassDirection prevDirection = _currentPosition.FacingDirection;
            TurnRover(turnCommand);
        }

        private void ExecuteMoveCommand(PositioningCommand positioningCommand)
        {
            PlateauPosition prevPos = _currentPosition;
            MoveRover(positioningCommand);
        }
        
        private void ExecutePositionCommand(PositioningCommand positioningCommand)
        {
            PlateauPosition prevPos = _currentPosition;
            PositionRover(positioningCommand);
        }

        private PlateauPosition GetCorrdinatePosition(string startPosCmdTxt)
        {
            PlateauPosition position = new PlateauPosition();

            string[] pars = startPosCmdTxt.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            int icordVal = 0;
            int.TryParse(pars[0], out icordVal);
            position.X = icordVal;

            icordVal = 0;
            int.TryParse(pars[1], out icordVal);
            position.Y = icordVal;

            if(pars.Length <= 2)
                position.FacingDirection = CompassDirection.N; // Defaulting the Facing Direction to North
            else
                position.FacingDirection = Enum.Parse<CompassDirection>(pars[2]);

            return position;
        }

        private CompassDirection GetInitialFaceDirection(int x, int y)
        {
            if (x == 1 && y == 1)
                return CompassDirection.N;

            if (x == PlateauMatrix.Item1 && y == PlateauMatrix.Item2)
                return CompassDirection.S;

            if ((y <= PlateauMatrix.Item2 && y > 0) && x == 1)
                return CompassDirection.S;

            if ((x <= PlateauMatrix.Item1 && x > 0) && y == 1)
                return CompassDirection.N;
            else
                return CompassDirection.S;
        }
        
        #endregion Private Methods

        #region Public Methods

        public void ExecuteCommand(BaseCommand command)
        {
            if (command is PositioningCommand)
            {
                ExecutePositionCommand(command as PositioningCommand);
            }
            else //It has to be MultiMove Command
            {
                ExecuteMultiMoveCommand(command as MultiMoveCommand);
            }
        }

        public void TurnRover(TurnCommand turnCommand)
        {
            switch (turnCommand.TurnToDirection)
            {
                case NavigationDirection.L:
                    TurnLeft();
                    break;
                case NavigationDirection.R:
                    TurnRight();
                    break;
            }
        }

        private void TurnLeft()
        {
            switch(_currentPosition.FacingDirection)
            {
                case CompassDirection.N:
                    _currentPosition.FacingDirection = CompassDirection.W;
                    break;
                case CompassDirection.S:
                    _currentPosition.FacingDirection = CompassDirection.E;
                    break;
                case CompassDirection.E:
                    _currentPosition.FacingDirection = CompassDirection.N;
                    break;
                case CompassDirection.W:
                    _currentPosition.FacingDirection = CompassDirection.S;
                    break;
            }   
        }

        private void TurnRight()
        {
            switch(_currentPosition.FacingDirection)
            {
                case CompassDirection.N:
                    _currentPosition.FacingDirection = CompassDirection.E;
                    break;
                case CompassDirection.S:
                    _currentPosition.FacingDirection = CompassDirection.W;
                    break;
                case CompassDirection.E:
                    _currentPosition.FacingDirection = CompassDirection.S;
                    break;
                case CompassDirection.W:
                    _currentPosition.FacingDirection = CompassDirection.N;
                    break;
            }    
        }

        public void MoveRover(PositioningCommand positioningCommand)
        {
            if(positioningCommand.RawCmdText.Equals("M"))
            {
                switch(_currentPosition.FacingDirection)
                {
                    case CompassDirection.N:
                        _currentPosition.Y++;
                        break;
                    case CompassDirection.S:
                        _currentPosition.Y--;
                        break;
                    case CompassDirection.E:
                        _currentPosition.X++;
                        break;
                    case CompassDirection.W:
                        _currentPosition.X--;
                        break;
                }
            }
        }

        public void PositionRover(PositioningCommand positioningCommand)
        {
            _currentPosition = GetCorrdinatePosition(positioningCommand.RawCmdText);
        }

        public void GotoStartPosition(string startPosCmdTxt)
        {
            // While creating the Rovers we need to pass start point and plateauMatrix
            // Now every rover has a same starting point. As the Rover needs X, Y and Direction to Face.
            // ASSUMPTION: As the start Position is given as only 5,5 and Matrix is 5,5 then we can assume.
            // that the rover should start at upper-right corner and face south. 
            // Hence if the direction is not mentioned in the Start Position, assume it is facing south.
            // And the explorer needs to explore the inside of the Plateau.
            _startPosition = GetCorrdinatePosition(startPosCmdTxt);
            // If the StartPosition has Direction then skip GetInitialFaceDirection();
            if(startPosCmdTxt.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length <= 2)
                _startPosition.FacingDirection = GetInitialFaceDirection(_startPosition.X, _startPosition.Y);
            _currentPosition = _startPosition;
        }

        #endregion Public Methods
    }
}
