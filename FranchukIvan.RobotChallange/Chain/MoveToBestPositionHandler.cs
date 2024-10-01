using Robot.Common;
using System.Collections.Generic;
using System.Linq;
using FranchukIvan.RobotChallange;

namespace FranchukIvan.RobotChallange
{
    public class MoveToBestPositionHandler : ICommandHandler
    {
        public RobotCommand Handle(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, Map map, string author, int currentRound)
        {
            var bestPosition = Functions
                .EvaluatePositionValue(map, robot.Position, robots, author)
                .AsParallel()
                .FirstOrDefault(positionRate =>
                    robot.Energy > Functions.GetDistanceCost(robot.Position, positionRate.Value) &&
                    Functions.IsAvailablePosition(map, robots, positionRate.Value, author)
                );

            if (!bestPosition.Equals(default(KeyValuePair<int, Position>)))
            {
                return new MoveCommand { NewPosition = bestPosition.Value };
            }

            return null;
        }
    }
}