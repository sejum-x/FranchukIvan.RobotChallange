using System;
using Robot.Common;

namespace FranchukIvan.RobotChallange.Utilities
{
    public static class DistanceUtility
    {
        public static int GetDistanceCost(Position from, Position to)
        {
            int deltaX = from.X - to.X;
            int deltaY = from.Y - to.Y;
            return deltaX * deltaX + deltaY * deltaY;
        }

        public static bool IsAdjacent(Position p1, Position p2) =>
            Math.Abs(p1.X - p2.X) <= 1 && Math.Abs(p1.Y - p2.Y) <= 1;
    }
}