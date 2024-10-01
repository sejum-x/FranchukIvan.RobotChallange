using System.Collections.Generic;
using Robot.Common;

namespace FranchukIvan.RobotChallange
{
    public interface ICommandHandler
    {
        RobotCommand Handle(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, Map map, string author, int currentRound);
    }
}