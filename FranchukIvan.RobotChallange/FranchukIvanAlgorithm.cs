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
        private const int MaxRobotsCount = 100;
        private const int MinEnergyForNewRobot = 250;
        
        public string Author => "Ivan Franchuk";

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            InitializeRound(robotToMoveIndex);
            var robot = robots[robotToMoveIndex];

            var actions = new Dictionary<Func<bool>, Func<RobotCommand>>
            {
                { () => currentRound == 51, () => new CollectEnergyCommand() },
                { () => ShouldCreateNewRobot(robots, robot), () => new CreateNewRobotCommand() },
                { () => HasEnoughEnergyNearby(robot, map), () => new CollectEnergyCommand() },
                { () => TryAttack(robot, robots) != null, () => TryAttack(robot, robots) },
                { () => TryMoveToBestPosition(robot, map, robots) != null, () => TryMoveToBestPosition(robot, map, robots) }
            };

            return ExecuteActions(actions) ?? new CollectEnergyCommand();
        }


        private void InitializeRound(int robotToMoveIndex)
        {
            if (firstRobotIndex == -1) firstRobotIndex = robotToMoveIndex;
            if (firstRobotIndex == robotToMoveIndex) ++currentRound;
        }

        private RobotCommand ExecuteActions(Dictionary<Func<bool>, Func<RobotCommand>> actions)
        {
            foreach (var action in actions)
            {
                if (action.Key()) return action.Value();
            }
            return null;
        }

        private bool ShouldCreateNewRobot(IList<Robot.Common.Robot> robots, Robot.Common.Robot robot)
        {
            return Functions.GetAuthorRobotCount(robots, Author) < MaxRobotsCount && robot.Energy > MinEnergyForNewRobot;
        }
        
        private RobotCommand TryAttack(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots)
        {
            var attackTargets = Functions.GetRobotsToNearAttack(robot.Position, robots, this.Author, currentRound);
            KeyValuePair<int, Position> adjacentTarget = attackTargets.FirstOrDefault(target => Functions.IsAdjacent(robot.Position, target.Value));

            if (adjacentTarget.Value != null)
            {
                return new MoveCommand { NewPosition = adjacentTarget.Value };
            }
             
            if (attackTargets.Count > 0)
            {
                KeyValuePair<int, Position> bestTarget = attackTargets.First();
                if (robot.Energy > Functions.GetDistanceCost(robot.Position, bestTarget.Value))
                {
                    return new MoveCommand { NewPosition = bestTarget.Value };
                }
            }

            return null;
        }
        
        private bool HasEnoughEnergyNearby(Robot.Common.Robot robot, Map map)
        {
            return map.GetNearbyResources(robot.Position, 2).Sum(station => station.Energy) > 150;
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

        public string Description => "Ivan Franchuk's Robot Algorithm for competition.";

        public IvanFranchukAlgorithm() => Logger.OnLogRound += LogRound;

        private void LogRound(object sender, LogRoundEventArgs e) => ++Round;
    }
}
