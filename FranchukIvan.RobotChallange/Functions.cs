using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange
{
    public class Functions
    {
        private const int NearbyRadius = 1;
        private const int MapSize = 100;
        private const int AttackRadius = 10;
        private const int EnergyThreshold = 1000;

        protected Functions() { }

        public static int GetAuthorRobotCount(IList<Robot.Common.Robot> robots, string author) =>
            robots.Count(robot => robot.OwnerName == author);

        public static int GetDistanceCost(Position from, Position to)
        {
            int deltaX = from.X - to.X;
            int deltaY = from.Y - to.Y;
            return deltaX * deltaX + deltaY * deltaY;
        }

        public static int GetNearbyRobotCount(IList<Robot.Common.Robot> robots, Position position) =>
            robots.Count(robot => IsWithinRadius(position, robot.Position, NearbyRadius));

        public static bool IsWithinRadius(Position center, Position point, int radius) =>
            Math.Abs(center.X - point.X) <= radius && Math.Abs(center.Y - point.Y) <= radius;

        public static bool IsAvailablePosition(Map map, IList<Robot.Common.Robot> robots, Position position, string author) =>
            !IsOutOfBounds(position) && !robots.Any(robot => robot.Position.Equals(position) && robot.OwnerName == author);

        public static List<KeyValuePair<int, Position>> FindAttackTargets(
            Map map,
            Position robotPosition,
            IList<Robot.Common.Robot> robots,
            string author)
        {
            var killRate = robots
                .Where(robot => robot.OwnerName != author &&
                                IsWithinRadius(robotPosition, robot.Position, AttackRadius) &&
                                robot.Energy >= EnergyThreshold)
                .Select(robot => new KeyValuePair<int, Position>(
                    CalculateKillPriority(robotPosition, robot),
                    robot.Position))
                .OrderByDescending(k => k.Key)
                .ToList();

            return killRate;
        }

        private static int CalculateKillPriority(Position from, Robot.Common.Robot targetRobot)
        {
            int distanceCost = GetDistanceCost(from, targetRobot.Position);
            int energyImpact = (int)(targetRobot.Energy * 0.1);
            return distanceCost + 30 - energyImpact;
        }

        public static List<KeyValuePair<int, Position>> EvaluatePositionValue(
            Map map,
            Position robotPosition,
            IList<Robot.Common.Robot> robots,
            string author)
        {
            var ratedPositions = Enumerable
                .Range(0, MapSize + 1)
                .SelectMany(x => Enumerable.Range(0, MapSize + 1), (x, y) => new Position(x, y))
                .Select(position =>
                {
                    if (!IsAvailablePosition(map, robots, position, author))
                        return new KeyValuePair<int, Position>(0, position);

                    int energyValue = GetTotalNearbyEnergy(map, position);
                    int score = energyValue - GetDistanceCost(robotPosition, position);
                    return new KeyValuePair<int, Position>(score, position);
                })
                .OrderByDescending(p => p.Key)
                .ToList();

            return ratedPositions;
        }

        private static int GetTotalNearbyEnergy(Map map, Position position) =>
            map.GetNearbyResources(position, 2).Sum(station => station.Energy);

        private static bool IsOutOfBounds(Position position) =>
            position.X < 0 || position.X > MapSize || position.Y < 0 || position.Y > MapSize;
    }
}
