using FranchukIvan.RobotChallange.Utilities;
using Robot.Common;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange
{
    public class MoveToBestPositionHandler : ICommandHandler
    {
        public RobotCommand Handle(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, Map map, string author, int currentRound)
        {
            var bestPosition = EvaluateUtility
                .EvaluatePositionValue(map, robot.Position, robots, author)
                .AsParallel()
                .FirstOrDefault(positionRate =>
                    robot.Energy > DistanceUtility.GetDistanceCost(robot.Position, positionRate.Value) &&
                    PositionUtility.IsAvailablePosition(map, robots, positionRate.Value, author)
                );

            if (!bestPosition.Equals(default(KeyValuePair<int, Position>)))
            {
                return new MoveCommand { NewPosition = bestPosition.Value };
            }

            return null;
        }
    }
}