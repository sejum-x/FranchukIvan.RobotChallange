using System;
using Robot.Common;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange.Utilities
{
    public static class RobotUtility
    {
        public static int GetAuthorRobotCount(IList<Robot.Common.Robot> robots, string author) =>
            robots.Count(robot => robot.OwnerName == author);

        public static int GetNearbyRobotCount(IList<Robot.Common.Robot> robots, Position position, int radius) =>
            robots.Count(robot => PositionUtility.IsWithinRadius(position, robot.Position, radius));

        public static bool IsWithinRadius(Position center, Position point, int radius) =>
            Math.Abs(center.X - point.X) <= radius && Math.Abs(center.Y - point.Y) <= radius;
    }
}