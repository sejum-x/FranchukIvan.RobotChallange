using Robot.Common;
using System.Collections.Generic;

namespace FranchukIvan.RobotChallange
{
    public class CreateNewRobotHandler : ICommandHandler
    {
        private const int MinEnergyForNewRobot = 250;
        private const int MaxRobotsCount = 100;

        public RobotCommand Handle(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, Map map, string author, int currentRound)
        {
            if (Functions.GetAuthorRobotCount(robots, author) < MaxRobotsCount && robot.Energy > MinEnergyForNewRobot)
            {
                return new CreateNewRobotCommand();
            }

            return null;
        }
    }
}