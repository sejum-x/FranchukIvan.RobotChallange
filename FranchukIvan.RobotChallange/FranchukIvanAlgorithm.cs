using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange
{
    public class FranchukIvanAlgorithm : IRobotAlgorithm
    {
        private const int MinEnergyForNewRobot = 500;  // Minimum energy required to create a new robot
        private const int EnergyForNewRobot = 200;     // Energy cost for creating a new robot
        private const int MaxStationEnergy = 2000;     // Maximum energy of a station
        private const int CollectionRange = 2;         // Range to collect energy from a station
        private const int AttackEnergyLoss = 30;       // Energy lost when attacking another robot
        private const int EnergyPercentageFromAttack = 10; // Percentage of energy taken from the attacked robot

        public string Author => "Franchuk Ivan";

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            var robot = robots[robotToMoveIndex];

            // Try to create a new robot if energy is sufficient
            if (robot.Energy > MinEnergyForNewRobot)
            {
                var freePosition = FindFreePosition(robot.Position, robots, map);
                if (freePosition != null)
                {
                    return new CreateNewRobotCommand { NewRobotEnergy = EnergyForNewRobot };
                }
            }

            var closestStation = FindClosestEnergyStation(robot, robots, map);

            if (closestStation != null)
            {
                // If the robot is within the collection range, collect energy
                if (GetDistance(robot.Position, closestStation.Position) <= CollectionRange)
                {
                    return new CollectEnergyCommand();
                }
                else
                {
                    // Move towards the closest energy station
                    var nextPosition = GetNextPositionTowards(robot.Position, closestStation.Position, robots, map);
                    return new MoveCommand { NewPosition = nextPosition };
                }
            }

            // If no station is found, stay in place
            return new MoveCommand { NewPosition = robot.Position };
        }

        private EnergyStation FindClosestEnergyStation(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, Map map)
        {
            return map.Stations
                      .Where(station => IsPositionFree(station.Position, robots, map))
                      .OrderBy(station => GetDistance(robot.Position, station.Position))
                      .FirstOrDefault();
        }

        private int GetDistance(Position a, Position b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private Position GetNextPositionTowards(Position current, Position target, IList<Robot.Common.Robot> robots, Map map)
        {
            int newX = current.X;
            int newY = current.Y;

            if (current.X < target.X) newX++;
            else if (current.X > target.X) newX--;

            if (current.Y < target.Y) newY++;
            else if (current.Y > target.Y) newY--;

            var nextPosition = new Position(newX, newY);

            // Ensure that the new position is within bounds and not occupied
            if (IsPositionFree(nextPosition, robots, map))
            {
                return nextPosition;
            }

            // If the direct move is not possible, stay in place or find another path
            return current;
        }

        private Position FindFreePosition(Position current, IList<Robot.Common.Robot> robots, Map map)
        {
            var directions = new List<Position>
            {
                new Position(current.X + 1, current.Y),
                new Position(current.X - 1, current.Y),
                new Position(current.X, current.Y + 1),
                new Position(current.X, current.Y - 1),
            };

            // Use LINQ to find the first free position
            return directions.Find(direction => IsPositionFree(direction, robots, map));
        }

        private bool IsPositionFree(Position position, IList<Robot.Common.Robot> robots, Map map)
        {
            // Check if the position is within map bounds
            if (position.X < map.MinPozition.X || position.Y < map.MinPozition.Y ||
                position.X >= map.MaxPozition.X || position.Y >= map.MaxPozition.Y)
            {
                return false;
            }

            // Check if the position is not occupied by another robot
            return !robots.Any(robot => robot.Position.X == position.X && robot.Position.Y == position.Y);
        }
    }
}