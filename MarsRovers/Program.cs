using MarsRovers.ObjectModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace MarsRovers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome Mars Rover Program!");
            Console.WriteLine("We will lanch 2 rovers on Mars. On a Plateau divided into [5X5] squares.");
            RoverLauncher roverLauncher = new RoverLauncher(2, new Tuple<int, int>(5, 5));
            Console.WriteLine("Reading the command file for commands...");
            string cmdFilePath = Path.Combine(Environment.CurrentDirectory, @"Data\CommandFile.txt");
            List<string> commands = null;
            commands = GetAllCommands(cmdFilePath, commands);
            roverLauncher.GetAllCommands(commands);
            roverLauncher.InitializeRoverSquad();
            roverLauncher.Run();
        }
        private static List<string> GetAllCommands(string cmdFilePath, List<string> commands)
        {
            if (File.Exists(cmdFilePath))
            {
                commands = new List<string>();
                StreamReader cmdFile = new StreamReader(cmdFilePath);
                string cmdLine = cmdFile.ReadLine();
                while (cmdLine != null)
                {
                    commands.Add(cmdLine);
                    cmdLine = cmdFile.ReadLine();
                }
            }

            return commands;
        }
    }

    public class RoverLauncher
    {
        protected readonly int _numberOfRovers;
        protected readonly Tuple<int, int> _plateauMatrix;
        private Dictionary<IMarsPlateauRover, List<string>> _roverSquad;
        List<string> allCommands;

        public RoverLauncher(int noOfRoversinSquad, Tuple<int, int> plateauMatrix)
        {
            if (noOfRoversinSquad <= 0)
                throw new ArgumentOutOfRangeException("No of Rovers to Launch should be 1 more than 1.");

            //Assuming that if plateauMatrix is not supplied then set it to default 5X5
            _plateauMatrix = new Tuple<int, int>(5, 5);
            _numberOfRovers = noOfRoversinSquad;
        }
        public void GetAllCommands(List<string> commands)
        {
            if (commands != null && commands.Count > 0)
                allCommands = commands; 
        }
        public void InitializeRoverSquad()
        {
            if (!allCommands.Any(c => string.IsNullOrEmpty(c)))
            {
                _roverSquad = new Dictionary<IMarsPlateauRover, List<string>>();
                for (int i = 0; i < this._numberOfRovers; i++)
                {
                    List<string> roverCmds = new List<string>();
                    
                    // As the First Command is positioning command and is common to all the 
                    // rovers add a copy in the commands list.
                    roverCmds.Add(allCommands[0]);

                    // Filter/Pick the commands for each rover.
                    roverCmds.AddRange(allCommands.Skip((i * 2) + 1).Take(2));

                    // Store Rover commands against the Rover object in dictionary.
                    // so it becomes easy to pass it on to the respective rovers.
                    _roverSquad.Add(new MarsPlateauRover("Rover"+i, _plateauMatrix, roverCmds), roverCmds);
                }
            }
        }
        public void Run()
        {
            foreach (IMarsPlateauRover rover in _roverSquad.Keys)
            {
                Console.WriteLine("Started Executing {0} Commmands.", rover.Name);
                _roverSquad[rover].ForEach(rc => Console.WriteLine(rc));

                rover.ExecuteAllCommands();
                Console.WriteLine("Finished Executing all Commands for {0} current position is: {1}", 
                    rover.Name, rover.CurrentPosition.ToString());
            }
        }
    }
}
