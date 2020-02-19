using NUnit.Framework;
using MarsRovers.ObjectModels;

namespace MarsRovers.Tests
{
    [TestFixture]
    public class CommandGeneratorTests
    {
        [Test]
        public void GenarateCommandTest_PositioningCmd()
        {
            string positionCmd = "2 2 N";

            BaseCommand genCmd = CommandGenerator.GenerateAppropriateCommand(positionCmd);

            Assert.That(genCmd is PositioningCommand);
            PositioningCommand posCmd = genCmd as PositioningCommand;
            Assert.That(posCmd.RawCmdText.Equals(positionCmd));
            Assert.Pass();
        }

        [Test]
        public void GenarateCommandTest_MultiMoveCmd()
        {
            string multiMoveCmdTxt = "LMMLMMLMMR";

            BaseCommand genCmd = CommandGenerator.GenerateAppropriateCommand(multiMoveCmdTxt);

            Assert.That(genCmd is MultiMoveCommand);
            MultiMoveCommand multiMvCmd = genCmd as MultiMoveCommand;
            Assert.That(multiMvCmd.RawCmdText.Equals(multiMoveCmdTxt));
            Assert.Pass();
        }
    }
}