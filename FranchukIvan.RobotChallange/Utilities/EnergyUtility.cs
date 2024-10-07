using System.Linq;
using Robot.Common;

namespace FranchukIvan.RobotChallange.Utilities
{
    public static class EnergyUtility
    {
        private const int NearbyRadius = 1;

        public static int GetTotalNearbyEnergy(Map map, Position position) =>
            map.GetNearbyResources(position, NearbyRadius)
                .Sum(resource => resource.Energy);
    }
}