using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange
{
    internal class Functions
    {
        private const int NearbyRadius = 1;
        private const int EnergyRadius = 2;

        public static int DistanceCost(Position center, Position distant) =>
            (center.X - distant.X) * (center.X - distant.X) + (center.Y - distant.Y) * (center.Y - distant.Y);

        public static int GetAuthorRobotCount(IList<Robot.Common.Robot> robots, string author) =>
            robots.Count(robot => robot.OwnerName == author);

        public static int GetNearbyRobotCount(IList<Robot.Common.Robot> robots, Position position) =>
            robots.Count(robot => IsWithinRadius(position, robot.Position, NearbyRadius));

        public static bool IsWithinRadius(Position center, Position point, int radius) =>
            Math.Abs(center.X - point.X) <= radius && Math.Abs(center.Y - point.Y) <= radius;

        public static bool IsAvailablePosition(Map map, IList<Robot.Common.Robot> robots, Position position, string author)
        {
            if (position.X > 100 || position.X < 0 || position.Y > 100 || position.Y < 0)
                return false;

            return !robots.Any(robot => robot.Position.Equals(position) && robot.OwnerName == author);
        }

        public static List<KeyValuePair<int, Position>> BestPositions(Map map, Position center, int radius)
        {
            int length = 2 * radius + 1;
            int[,] numArray = new int[length, length];
            int num1 = center.Y - radius;
            int num2 = center.X - radius;

            foreach (EnergyStation station in map.Stations)
            {
                if (station.Position.X >= num2 - 2 && station.Position.X < num2 + length + 2 &&
                    station.Position.Y >= num1 - 2 && station.Position.Y < num1 + length + 2)
                {
                    int num3 = station.Position.X - num2;
                    int num4 = station.Position.Y - num1;

                    for (int index1 = -2; index1 <= 2; ++index1)
                    {
                        for (int index2 = -2; index2 <= 2; ++index2)
                        {
                            if (num3 + index2 >= 0 && num3 + index2 < length && num4 + index1 >= 0 && num4 + index1 < length)
                                numArray[num4 + index1, num3 + index2] += station.Energy;
                        }
                    }
                }
            }

            var keyValuePairList = new List<KeyValuePair<int, Position>>();
            for (int index3 = 0; index3 < length; ++index3)
            {
                for (int index4 = 0; index4 < length; ++index4)
                {
                    numArray[index3, index4] -= DistanceCost(center, new Position(num2 + index4, num1 + index3));
                    keyValuePairList.Add(new KeyValuePair<int, Position>(numArray[index3, index4], new Position(num2 + index4, num1 + index3)));
                }
            }

            keyValuePairList.Sort((pair1, pair2) => -pair1.Key.CompareTo(pair2.Key));
            return keyValuePairList;
        }
    }

    public class IvanFranchukAlgorithm : IRobotAlgorithm
    {
        private static int Fri = -1;
        private static int round = 0;

        public string Author => "Ivan Franchuk";

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            if (Fri == -1)
                Fri = robotToMoveIndex;

            if (Fri == robotToMoveIndex)
                ++round;

            if (round == 51)
                return new CollectEnergyCommand();

            Robot.Common.Robot robot = robots[robotToMoveIndex];

            if (round % 10 == 5)
            {
                foreach (var bestPosition in Functions.BestPositions(map, robot.Position, 100))
                {
                    if (Functions.IsAvailablePosition(map, robots, bestPosition.Value, Author) &&
                        robot.Energy > Functions.DistanceCost(robot.Position, bestPosition.Value))
                    {
                        return new MoveCommand { NewPosition = bestPosition.Value };
                    }
                }
            }

            if (Functions.GetAuthorRobotCount(robots, Author) < 100 && robot.Energy > 300)
                return new CreateNewRobotCommand();

            IList<EnergyStation> nearbyResources = map.GetNearbyResources(robot.Position, 2);
            if (nearbyResources.Count > 0)
            {
                foreach (var energyStation in nearbyResources)
                {
                    if (round < 5 || energyStation.Energy > 100)
                        return new CollectEnergyCommand();
                }
            }

            foreach (var bestPosition in Functions.BestPositions(map, robot.Position, 5))
            {
                if (Functions.IsAvailablePosition(map, robots, bestPosition.Value, Author) &&
                    robot.Energy > Functions.DistanceCost(robot.Position, bestPosition.Value))
                {
                    return new MoveCommand { NewPosition = bestPosition.Value };
                }
            }

            return new CollectEnergyCommand();
        }

        public int Round { get; set; }

        public IvanFranchukAlgorithm() => Logger.OnLogRound += LogRound;

        private void LogRound(object sender, LogRoundEventArgs e) => ++Round;
    }
}