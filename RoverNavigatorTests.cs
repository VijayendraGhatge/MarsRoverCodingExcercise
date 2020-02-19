using System;
using MarsRovers.ObjectModels;
using NUnit.Framework;

namespace MarsRovers.Tests
{
    [TestFixture]
    public class RoverNavigatorTests
    {
        private IPlateauMatrixNavigator _plateauNavigator;

        [SetUp]
        public void TestInit()
        {
            _plateauNavigator = new PlateauNavigator(new Tuple<int, int>(5, 5));
        }

        [Test]
        public void StartPositionNav_Test()
        {
            Assert.That(_plateauNavigator != null);
            string expectedCordinates = "5 5";
            CompassDirection expectedFacingDirection = CompassDirection.S;

            _plateauNavigator.GotoStartPosition(expectedCordinates);
            Assert.That(_plateauNavigator.StartPosition != null);
            Assert.That(_plateauNavigator.CurrentPosition != null);
            Assert.That(_plateauNavigator.StartPosition.Equals(_plateauNavigator.CurrentPosition));
            Assert.That(_plateauNavigator.StartPosition.X == 5);
            Assert.That(_plateauNavigator.StartPosition.Y == 5);
            Assert.That(_plateauNavigator.StartPosition.FacingDirection == expectedFacingDirection);
        }

        [Test]
        public void PositionCmd_Test()
        {
            Assert.That(_plateauNavigator != null);
            string startPoint = "5 5";
            _plateauNavigator.GotoStartPosition(startPoint);
            string positioningCmd = "1 2 N";

            _plateauNavigator.ExecuteCommand(CommandGenerator.GenerateAppropriateCommand(positioningCmd));
            Assert.That(_plateauNavigator.CurrentPosition != null);
            Assert.That(!_plateauNavigator.CurrentPosition.Equals(_plateauNavigator.StartPosition));
            Assert.That(_plateauNavigator.CurrentPosition.X == 1);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 2);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.N);
        }

        [Test]
        public void MultiNavCmd_Test()
        {
            Assert.That(_plateauNavigator != null);
            _plateauNavigator.GotoStartPosition("5 5");
            _plateauNavigator.ExecuteCommand(CommandGenerator.GenerateAppropriateCommand("1 2 N"));
            string multiNavCmd = "LMLMLMLMM";

            _plateauNavigator.ExecuteCommand(CommandGenerator.GenerateAppropriateCommand(multiNavCmd));
            Assert.That(_plateauNavigator.CurrentPosition != null);
            Assert.That(!_plateauNavigator.CurrentPosition.Equals(_plateauNavigator.StartPosition));
            Assert.That(_plateauNavigator.CurrentPosition.X == 1);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 3);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.N);
        }

        [Test]
        public void MultiNavCmd_Test2()
        {
            Assert.That(_plateauNavigator != null);
            _plateauNavigator.GotoStartPosition("5 5");
            _plateauNavigator.ExecuteCommand(CommandGenerator.GenerateAppropriateCommand("3 3 E"));
            string multiNavCmd = "MMRMMRMRRM";

            _plateauNavigator.ExecuteCommand(CommandGenerator.GenerateAppropriateCommand(multiNavCmd));
            Assert.That(_plateauNavigator.CurrentPosition != null);
            Assert.That(!_plateauNavigator.CurrentPosition.Equals(_plateauNavigator.StartPosition));
            Assert.That(_plateauNavigator.CurrentPosition.X == 5);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 1);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.E);
        }

        [Test]
        public void PositionRover_Test()
        {
            Assert.IsNotNull(_plateauNavigator);
            string posRoverCmd = "3 4 W";

            BaseCommand posCmd = CommandGenerator.GenerateAppropriateCommand(posRoverCmd);
            Assert.That(posCmd is PositioningCommand);
            _plateauNavigator.PositionRover(posCmd as PositioningCommand);

            Assert.That(_plateauNavigator.CurrentPosition.X == 3);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 4);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.W);
            Assert.Pass();
        }

        [Test]
        public void MoveRover_XAxisSubtract_Test()
        {
            Assert.IsNotNull(_plateauNavigator);
            string posRoverCmd = "3 4 W";
            _plateauNavigator.GotoStartPosition(posRoverCmd);
            string mvRoverXACmd = "M";

            BaseCommand moveCmd = CommandGenerator.GenerateAppropriateCommand(mvRoverXACmd);
            Assert.That(moveCmd is MultiMoveCommand);
            // Avoiding using Mock Framework
            //_navigator.MoveRover(moveCmd as PositioningCommand); should be called
            _plateauNavigator.ExecuteCommand(moveCmd as MultiMoveCommand);

            Assert.That(_plateauNavigator.CurrentPosition.X == 2);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 4);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.W);
            Assert.Pass();
        }

        [Test]
        public void MoveRover_XAxisAdd_Test()
        {
            Assert.IsNotNull(_plateauNavigator);
            string posRoverCmd = "3 4 E";
            _plateauNavigator.GotoStartPosition(posRoverCmd);
            string mvRoverXACmd = "M";

            BaseCommand moveCmd = CommandGenerator.GenerateAppropriateCommand(mvRoverXACmd);
            Assert.That(moveCmd is MultiMoveCommand);
            // Avoiding using Mock Framework
            //_navigator.MoveRover(moveCmd as PositioningCommand); should be called
            _plateauNavigator.ExecuteCommand(moveCmd as MultiMoveCommand);

            Assert.That(_plateauNavigator.CurrentPosition.X == 4);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 4);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.E);
            Assert.Pass();
        }

        [Test]
        public void MoveRover_YAxisSubtract_Test()
        {
            Assert.IsNotNull(_plateauNavigator);
            string posRoverCmd = "3 4 S";
            _plateauNavigator.GotoStartPosition(posRoverCmd);
            string mvRoverXACmd = "M";

            BaseCommand moveCmd = CommandGenerator.GenerateAppropriateCommand(mvRoverXACmd);
            Assert.That(moveCmd is MultiMoveCommand);
            // Avoiding using Mock Framework
            //_navigator.MoveRover(moveCmd as PositioningCommand); should be called
            _plateauNavigator.ExecuteCommand(moveCmd as MultiMoveCommand);

            Assert.That(_plateauNavigator.CurrentPosition.X == 3);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 3);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.S);
            Assert.Pass();
        }

        [Test]
        public void MoveRover_YAxisAdd_Test()
        {
            Assert.IsNotNull(_plateauNavigator);
            string posRoverCmd = "3 4 N";
            _plateauNavigator.GotoStartPosition(posRoverCmd);
            string mvRoverXACmd = "M";

            BaseCommand moveCmd = CommandGenerator.GenerateAppropriateCommand(mvRoverXACmd);
            Assert.That(moveCmd is MultiMoveCommand);
            // Avoiding using Mock Framework
            //_navigator.MoveRover(moveCmd as PositioningCommand); should be called
            _plateauNavigator.ExecuteCommand(moveCmd as MultiMoveCommand);

            Assert.That(_plateauNavigator.CurrentPosition.X == 3);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 5);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.N);
            Assert.Pass();
        }

        [Test]
        public void TurnRover_Left_Test()
        {
            Assert.IsNotNull(_plateauNavigator);
            string posRoverCmd = "3 4 N";
            _plateauNavigator.GotoStartPosition(posRoverCmd);
            string trnRoverCmd = "L";

            BaseCommand trnCmd = CommandGenerator.GenerateAppropriateCommand(trnRoverCmd);
            Assert.That(trnCmd is MultiMoveCommand);
            // Avoiding using Mock Framework
            //_navigator.MoveRover(moveCmd as PositioningCommand); should be called
            _plateauNavigator.ExecuteCommand(trnCmd as MultiMoveCommand);

            Assert.That(_plateauNavigator.CurrentPosition.X == 3);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 4);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.W);
            Assert.Pass();
        }

        [Test]
        public void TurnRover_Right_Test()
        {
            Assert.IsNotNull(_plateauNavigator);
            string posRoverCmd = "3 4 N";
            _plateauNavigator.GotoStartPosition(posRoverCmd);
            string trnRoverCmd = "R";

            BaseCommand trnCmd = CommandGenerator.GenerateAppropriateCommand(trnRoverCmd);
            Assert.That(trnCmd is MultiMoveCommand);
            // Avoiding using Mock Framework
            //_navigator.MoveRover(moveCmd as PositioningCommand); should be called
            _plateauNavigator.ExecuteCommand(trnCmd as MultiMoveCommand);

            Assert.That(_plateauNavigator.CurrentPosition.X == 3);
            Assert.That(_plateauNavigator.CurrentPosition.Y == 4);
            Assert.That(_plateauNavigator.CurrentPosition.FacingDirection == CompassDirection.E);
            Assert.Pass();
        }

        [TearDown]
        public void CleanTests()
        {
            _plateauNavigator = null;
        }
    }
}
