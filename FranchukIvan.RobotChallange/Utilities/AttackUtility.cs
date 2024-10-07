using Robot.Common;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange.Utilities
{
    public static class AttackUtility
    {
        private const int BaseAttackCost = 30;
        private const double EnergyGainFactor = 0.1;

        public static List<KeyValuePair<int, Position>> GetRobotsToNearAttack(
            Position currentPosition,
            IList<Robot.Common.Robot> robots,
            string author,
            int roundNumber)
        {
            return robots
                .Where(robot => robot.OwnerName != author)
                .Select(robot =>
                {
                    int distanceCost = DistanceUtility.GetDistanceCost(currentPosition, robot.Position) + BaseAttackCost;
                    int netEnergyGain = (int)(robot.Energy * EnergyGainFactor) - distanceCost;

                    if (netEnergyGain >= 0)
                    {
                        int key = distanceCost - (int)(robot.Energy * EnergyGainFactor);
                        return new KeyValuePair<int, Position>(key, robot.Position);
                    }
                    return new KeyValuePair<int, Position>(int.MaxValue, robot.Position);
                })
                .Where(pair => pair.Key != int.MaxValue)
                .OrderBy(pair => pair.Key)
                .ToList();
        }
    }
}