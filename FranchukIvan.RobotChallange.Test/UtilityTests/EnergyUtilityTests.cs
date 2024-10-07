using FranchukIvan.RobotChallange.Utilities;
using NUnit.Framework;
using Robot.Common;

namespace FranchukIvan.RobotChallange.Test
{
    [TestFixture]
    public class EnergyUtilityTests
    {
        private Map map;

        [SetUp]
        public void Setup()
        {
            map = new Map();
        }

        [Test]
        public void GetTotalNearbyEnergy_Should_Sum_Nearby_Energy()
        {
            var position = new Position(5, 5);
            map.Stations.Add(new EnergyStation { Position = new Position(5, 6), Energy = 300 });
            map.Stations.Add(new EnergyStation { Position = new Position(6, 6), Energy = 200 });

            int totalEnergy = EnergyUtility.GetTotalNearbyEnergy(map, position);
            Assert.AreEqual(500, totalEnergy);
        }
    }
}