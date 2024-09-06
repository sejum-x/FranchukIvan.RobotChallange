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

        public string Author => "Franchuk Ivan";

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            var robot = robots[robotToMoveIndex];

            // Check if the robot has enough energy to create a new robot
            if (robot.Energy > MinEnergyForNewRobot)
            {
                var freePosition = FindFreePosition(robot.Position, robots, map);
                if (freePosition != null)
                {
                    // Create a new robot
                    return new CreateNewRobotCommand { NewRobotEnergy = EnergyForNewRobot };
                }
            }

            // Proceed with existing robot actions
            var closestStation = FindClosestEnergyStation(robot, map);

            if (closestStation != null)
            {
                if (GetDistance(robot.Position, closestStation.Position) <= 2)
                {
                    return new CollectEnergyCommand();
                }
                else
                {
                    var nextPosition = GetNextPositionTowards(robot.Position, closestStation.Position);
                    return new MoveCommand { NewPosition = nextPosition };
                }
            }

            return new MoveCommand { NewPosition = robot.Position };
        }

        private EnergyStation FindClosestEnergyStation(Robot.Common.Robot robot, Map map)
        {
            return map.Stations
                      .OrderBy(station => GetDistance(robot.Position, station.Position))
                      .FirstOrDefault();
        }

        private int GetDistance(Position a, Position b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private Position GetNextPositionTowards(Position current, Position target)
        {
            int newX = current.X;
            int newY = current.Y;

            if (current.X < target.X) newX++;
            else if (current.X > target.X) newX--;

            if (current.Y < target.Y) newY++;
            else if (current.Y > target.Y) newY--;

            return new Position(newX, newY);
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