using NUnit.Framework;
using Robot.Common;
using System.Collections.Generic;
using FranchukIvan.RobotChallange.Utilities;

namespace FranchukIvan.RobotChallange.Test
{
    [TestFixture]
    public class RobotUtilityTests
    {
        private List<Robot.Common.Robot> robots;

        [SetUp]
        public void Setup()
        {
            robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot { Position = new Position(0, 0), Energy = 500, OwnerName = "Ivan Franchuk" },
                new Robot.Common.Robot { Position = new Position(10, 10), Energy = 300, OwnerName = "Opponent" }
            };
        }

        [Test]
        public void GetAuthorRobotCount_Should_Return_Correct_Count()
        {
            int count = RobotUtility.GetAuthorRobotCount(robots, "Ivan Franchuk");
            Assert.AreEqual(1, count);
        }

        [Test]
        public void GetNearbyRobotCount_Should_Return_Correct_Count()
        {
            int nearbyRobots = RobotUtility.GetNearbyRobotCount(robots, new Position(0, 0), 2);
            Assert.AreEqual(1, nearbyRobots);
        }
    }
}