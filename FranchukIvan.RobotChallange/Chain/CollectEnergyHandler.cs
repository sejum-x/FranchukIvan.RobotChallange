using Robot.Common;
using System.Collections.Generic;
using System.Linq;
using FranchukIvan.RobotChallange;

namespace FranchukIvan.RobotChallange
{
    public class CollectEnergyHandler : ICommandHandler
    {
        public RobotCommand Handle(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, Map map, string author, int currentRound)
        {
            if (map.GetNearbyResources(robot.Position, 2).Sum(station => station.Energy) > 150)
            {
                return new CollectEnergyCommand();
            }

            return null;
        }
    }
}