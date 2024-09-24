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

            Robot.Common.Robot robot = robots[robotToMoveIndex];

            var actions = new Dictionary<Func<bool>, Func<RobotCommand>>
            {
                { () => currentRound == 51, () => new CollectEnergyCommand() },
                { () => ShouldCreateNewRobot(robots, robot), () => new CreateNewRobotCommand() },
                { () => TryAttack(robot, map, robots) != null, () => TryAttack(robot, map, robots) },
                { () => HasEnoughEnergyNearby(robot, map), () => new CollectEnergyCommand() },
                { () => TryMoveToBestPosition(robot, map, robots) != null, () => TryMoveToBestPosition(robot, map, robots) }
            };

            foreach (var action in actions)
            {
                if (action.Key())
                {
                    return action.Value();
                }
            }

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

            var energyThresholds = new Dictionary<int, int>
            {
                { 40, 2000 },
                { 30, 1500 },
                { 20, 1000 },
                { 10, 400 }, 
                { 0, 100 }    
            };

            int roundThreshold = energyThresholds
                .Where(kvp => currentRound > kvp.Key)
                .Select(kvp => kvp.Value)
                .FirstOrDefault();

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
