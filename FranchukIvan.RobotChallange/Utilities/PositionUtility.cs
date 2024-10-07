using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange.Utilities
{
    public static class PositionUtility
    {
        private const int MapSize = 100;

        public static bool IsAvailablePosition(Map map, IList<Robot.Common.Robot> robots, Position position, string author) =>
            !IsOutOfBounds(position) && !robots.Any(robot => robot.Position.Equals(position));

        public static bool IsOutOfBounds(Position position) =>
            position.X < 0 || position.X > MapSize || position.Y < 0 || position.Y > MapSize;

        public static bool IsWithinRadius(Position center, Position point, int radius) =>
            Math.Abs(center.X - point.X) <= radius && Math.Abs(center.Y - point.Y) <= radius;
    }
}