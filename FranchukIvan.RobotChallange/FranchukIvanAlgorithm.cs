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

        public string Author => "Ivan Franchuk";

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            InitializeRound(robotToMoveIndex);

            if (currentRound == 51)
                return new CollectEnergyCommand();

            Robot.Common.Robot robot = robots[robotToMoveIndex];

            if (ShouldCreateNewRobot(robots, robot))
                return new CreateNewRobotCommand();

            var attackCommand = TryAttack(robot, map, robots);
            if (attackCommand != null)
                return attackCommand;

            if (HasEnoughEnergyNearby(robot, map))
                return new CollectEnergyCommand();

            var moveCommand = TryMoveToBestPosition(robot, map, robots);
            if (moveCommand != null)
                return moveCommand;

            return new CollectEnergyCommand();
        }

        private void InitializeRound(int robotToMoveIndex)
        {
            if (firstRobotIndex == -1)
                firstRobotIndex = robotToMoveIndex;

            if (firstRobotIndex == robotToMoveIndex)
                ++currentRound;
        }

        private bool ShouldCreateNewRobot(IList<Robot.Common.Robot> robots, Robot.Common.Robot robot)
        {
            return Functions.GetAuthorRobotCount(robots, Author) < 100 && robot.Energy > 250;
        }

        private RobotCommand TryAttack(Robot.Common.Robot robot, Map map, IList<Robot.Common.Robot> robots)
        {
            var potentialTargets = Functions.FindAttackTargets(map, robot.Position, robots, Author);
            if (!potentialTargets.Any())
                return null;

            Position targetPosition = potentialTargets.First().Value;
            int moveCost = Functions.GetDistanceCost(robot.Position, targetPosition);

            int roundThreshold = 100 + (Math.Max(0, currentRound - 20) / 5) * 200;

            if (robot.Energy > moveCost + roundThreshold)
                return new MoveCommand { NewPosition = targetPosition };

            return null;
        }

        private bool HasEnoughEnergyNearby(Robot.Common.Robot robot, Map map)
        {
            int totalEnergyNearby = map.GetNearbyResources(robot.Position, 2)
                                     .Sum(station => station.Energy);
            return totalEnergyNearby > 150;
        }

        private RobotCommand TryMoveToBestPosition(Robot.Common.Robot robot, Map map, IList<Robot.Common.Robot> robots)
        {
            foreach (var positionRate in Functions.EvaluatePositionValue(map, robot.Position, robots, Author))
            {
                if (robot.Energy > Functions.GetDistanceCost(robot.Position, positionRate.Value) &&
                    Functions.IsAvailablePosition(map, robots, positionRate.Value, Author))
                {
                    return new MoveCommand { NewPosition = positionRate.Value };
                }
            }

            return null;
        }

        public int Round { get; set; }

        public string Description => throw new System.NotImplementedException();

        public IvanFranchukAlgorithm() => Logger.OnLogRound += LogRound;

        private void LogRound(object sender, LogRoundEventArgs e) => ++Round;
    }
}
