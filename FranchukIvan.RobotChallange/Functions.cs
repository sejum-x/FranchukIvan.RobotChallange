using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranchukIvan.RobotChallange
{
    public class Functions
    {
        private const int NearbyRadius = 1;
        private const int MapSize = 100;

        protected Functions() { }

        public static int GetAuthorRobotCount(IList<Robot.Common.Robot> robots, string author) =>
            robots.AsParallel().Count(robot => robot.OwnerName == author);

        public static int GetDistanceCost(Position from, Position to)
        {
            int deltaX = from.X - to.X;
            int deltaY = from.Y - to.Y;
            return deltaX * deltaX + deltaY * deltaY;
        }

        public static bool IsAdjacent(Position p1, Position p2) => Math.Abs(p1.X - p2.X) <= 1 && Math.Abs(p1.Y - p2.Y) <= 1;

        public static int GetNearbyRobotCount(IList<Robot.Common.Robot> robots, Position position) =>
            robots.AsParallel().Count(robot => IsWithinRadius(position, robot.Position, NearbyRadius));
        
        public static bool IsWithinRadius(Position center, Position point, int radius) =>
            Math.Abs(center.X - point.X) <= radius && Math.Abs(center.Y - point.Y) <= radius;

        public static bool IsAvailablePosition(Map map, IList<Robot.Common.Robot> robots, Position position, string author) =>
            !IsOutOfBounds(position) && !robots.Any(robot => robot.Position.Equals(position));

        public static List<KeyValuePair<int, Position>> getRobotsToNearAttack(
            Position currentPosition,
            IList<Robot.Common.Robot> robots,
            string author,
            int roundNumber)
        {
            var robotsToNearAttack = robots
                .AsParallel()
                .Where(robot => robot.OwnerName != author)
                .Select(robot =>
                {
                    int distanceCost = GetDistanceCost(currentPosition, robot.Position) + 30;
                    int netEnergyGain = (int)(robot.Energy * 0.1) - distanceCost;
                    if (netEnergyGain >= 0)
                    {
                        int key = distanceCost - (int)(robot.Energy * 0.1);
                        return new KeyValuePair<int, Position>(key, robot.Position);
                    }
                    return new KeyValuePair<int, Position>(int.MaxValue, robot.Position); 
                })
                .Where(pair => pair.Key != int.MaxValue)
                .ToList();

            robotsToNearAttack.Sort((a, b) => a.Key.CompareTo(b.Key));
            return robotsToNearAttack;
        }

        public static List<KeyValuePair<int, Position>> EvaluatePositionValue(
            Map map,
            Position robotPosition,
            IList<Robot.Common.Robot> robots,
            string author)
        {
            var ratedPositions = Enumerable
                .Range(0, MapSize + 1)
                .AsParallel()
                .SelectMany(x => Enumerable.Range(0, MapSize + 1).AsParallel(), (x, y) => new Position(x, y))
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

        public static int GetTotalNearbyEnergy(Map map, Position position) =>
            map.GetNearbyResources(position, 2).Sum(station => station.Energy);
        private static bool IsOutOfBounds(Position position) =>
            position.X < 0 || position.X > MapSize || position.Y < 0 || position.Y > MapSize;
    }
}
