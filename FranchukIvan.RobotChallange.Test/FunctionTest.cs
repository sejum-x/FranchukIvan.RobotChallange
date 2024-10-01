using NUnit.Framework;
using Robot.Common;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange.Test
{
    public class FunctionsTests
    {
        private List<Robot.Common.Robot> robots;
        private Map map;

        [SetUp]
        public void Setup()
        {
            map = new Map();
            robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot { Position = new Position(0, 0), Energy = 500, OwnerName = "Ivan Franchuk" },
                new Robot.Common.Robot { Position = new Position(10, 10), Energy = 300, OwnerName = "Opponent" }
            };
        }

        [Test]
        public void GetAuthorRobotCount_Should_Return_Correct_Count()
        {
            int count = Functions.GetAuthorRobotCount(robots, "Ivan Franchuk");
            Assert.AreEqual(1, count);
        }

        [Test]
        public void GetDistanceCost_Should_Return_Correct_Distance()
        {
            Position pos1 = new Position(0, 0);
            Position pos2 = new Position(3, 4);
            int distance = Functions.GetDistanceCost(pos1, pos2);
            Assert.AreEqual(25, distance);
        }

        [Test]
        public void IsAdjacent_Should_Return_True_When_Positions_Are_Adjacent()
        {
            Position pos1 = new Position(1, 1);
            Position pos2 = new Position(1, 2);
            bool result = Functions.IsAdjacent(pos1, pos2);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsAdjacent_Should_Return_False_When_Positions_Are_Not_Adjacent()
        {
            Position pos1 = new Position(1, 1);
            Position pos2 = new Position(3, 3);
            bool result = Functions.IsAdjacent(pos1, pos2);
            Assert.IsFalse(result);
        }

        [Test]
        public void GetNearbyRobotCount_Should_Return_Correct_Count()
        {
            int nearbyRobots = Functions.GetNearbyRobotCount(robots, new Position(0, 0));
            Assert.AreEqual(1, nearbyRobots);
        }

        [Test]
        public void IsWithinRadius_Should_Return_True_When_Within_Radius()
        {
            Position center = new Position(5, 5);
            Position point = new Position(6, 6);
            bool result = Functions.IsWithinRadius(center, point, 1);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsWithinRadius_Should_Return_False_When_Not_Within_Radius()
        {
            Position center = new Position(5, 5);
            Position point = new Position(8, 8);
            bool result = Functions.IsWithinRadius(center, point, 1);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsAvailablePosition_Should_Return_True_When_Position_Is_Free()
        {
            Position pos = new Position(50, 50);
            bool result = Functions.IsAvailablePosition(map, robots, pos, "Ivan Franchuk");
            Assert.IsTrue(result);
        }

        [Test]
        public void IsAvailablePosition_Should_Return_False_When_Position_Occupied()
        {
            Position pos = new Position(0, 0);
            bool result = Functions.IsAvailablePosition(map, robots, pos, "Ivan Franchuk");
            Assert.IsFalse(result);
        }

        [Test]
        public void GetRobotsToNearAttack_Should_Calculate_NetEnergyGain()
        {
            Position currentPosition = new Position(0, 0);
            int roundNumber = 1;


            robots.Add(new Robot.Common.Robot { Position = new Position(2, 2), Energy = 400, OwnerName = "Opponent" });

            var result = Functions.GetRobotsToNearAttack(currentPosition, robots, "Ivan Franchuk", roundNumber);

            Assert.IsTrue(result.Count > 0);
            var attackPosition = result.FirstOrDefault().Value;
            Assert.AreEqual(2, attackPosition.X);
            Assert.AreEqual(2, attackPosition.Y);
        }

        [Test]
        public void GetRobotsToNearAttack_Should_Sort_By_Key_Ascending()
        {
            Position currentPosition = new Position(0, 0);
            int roundNumber = 1;

            robots.Add(new Robot.Common.Robot { Position = new Position(1, 1), Energy = 1000, OwnerName = "Opponent" });
            robots.Add(new Robot.Common.Robot { Position = new Position(2, 2), Energy = 1000, OwnerName = "Opponent" });

            var result = Functions.GetRobotsToNearAttack(currentPosition, robots, "Ivan Franchuk", roundNumber);

            Assert.IsTrue(result.Count >= 2, "Expected at least two robots in the result");

            Assert.AreEqual(1, result[0].Value.X);
            Assert.AreEqual(2, result[1].Value.X);
        }

        [Test]
        public void GetTotalNearbyEnergy_Should_Sum_Nearby_Energy()
        {
            var position = new Position(5, 5);

            map.Stations.Add(new EnergyStation { Position = new Position(5, 6), Energy = 300 });
            map.Stations.Add(new EnergyStation { Position = new Position(6, 6), Energy = 200 });

            int totalEnergy = Functions.GetTotalNearbyEnergy(map, position);

            Assert.AreEqual(500, totalEnergy);
        }

    }
}
