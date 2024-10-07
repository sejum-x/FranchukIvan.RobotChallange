using Robot.Common;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange.Utilities
{
    public static class EvaluateUtility
    {
        private const int MapSize = 100;

        public static List<KeyValuePair<int, Position>> EvaluatePositionValue(
            Map map,
            Position robotPosition,
            IList<Robot.Common.Robot> robots,
            string author)
        {
            return Enumerable
                .Range(0, MapSize + 1)
                .SelectMany(x => Enumerable.Range(0, MapSize + 1), (x, y) => new Position(x, y))
                .Where(position => PositionUtility.IsAvailablePosition(map, robots, position, author))
                .Select(position =>
                {
                    int energyValue = EnergyUtility.GetTotalNearbyEnergy(map, position);
                    int score = energyValue - DistanceUtility.GetDistanceCost(robotPosition, position);
                    return new KeyValuePair<int, Position>(score, position);
                })
                .Where(pair => pair.Key > 0)
                .OrderByDescending(pair => pair.Key)
                .ToList();
        }
    }
}