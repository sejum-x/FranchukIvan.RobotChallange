using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange
{
    public class IvanFranchukAlgorithm : IRobotAlgorithm
    {
        private int firstRobotIndex = -1;
        private int currentRound = 0;
        private readonly CommandChain commandChain;
        public string Author => "Ivan Franchuk";

        // Конструктор з ін'єкцією залежності
        public IvanFranchukAlgorithm(CommandChain commandChain)
        {
            this.commandChain = commandChain ?? throw new ArgumentNullException(nameof(commandChain));
            Logger.OnLogRound += LogRound;
        }

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            InitializeRound(robotToMoveIndex);
            var robot = robots[robotToMoveIndex];

            return commandChain.Execute(robot, robots, map, Author, currentRound);
        }

        private void InitializeRound(int robotToMoveIndex)
        {
            if (firstRobotIndex == -1) firstRobotIndex = robotToMoveIndex;
            if (firstRobotIndex == robotToMoveIndex) ++currentRound;
        }

        public int Round { get; set; }

        public string Description => "Ivan Franchuk's Robot Algorithm for competition.";

        private void LogRound(object sender, LogRoundEventArgs e) => ++Round;
    }
}