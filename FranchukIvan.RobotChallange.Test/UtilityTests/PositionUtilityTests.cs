using FranchukIvan.RobotChallange.Utilities;
using NUnit.Framework;
using Robot.Common;

namespace FranchukIvan.RobotChallange.Test
{
    [TestFixture]
    public class PositionUtilityTests
    {
        [Test]
        public void IsWithinRadius_Should_Return_True_When_Within_Radius()
        {
            Position center = new Position(5, 5);
            Position point = new Position(6, 6);
            bool result = PositionUtility.IsWithinRadius(center, point, 1);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsWithinRadius_Should_Return_False_When_Not_Within_Radius()
        {
            Position center = new Position(5, 5);
            Position point = new Position(8, 8);
            bool result = PositionUtility.IsWithinRadius(center, point, 1);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsAvailablePosition_Should_Return_True_When_Position_Is_Free()
        {
            Map map = new Map();
            List<Robot.Common.Robot> robots = new List<Robot.Common.Robot>();
            Position pos = new Position(50, 50);

            bool result = PositionUtility.IsAvailablePosition(map, robots, pos, "Ivan Franchuk");
            Assert.IsTrue(result);
        }

        [Test]
        public void IsAvailablePosition_Should_Return_False_When_Position_Occupied()
        {
            Map map = new Map();
            List<Robot.Common.Robot> robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot { Position = new Position(0, 0), Energy = 500, OwnerName = "Ivan Franchuk" }
            };
            Position pos = new Position(0, 0);

            bool result = PositionUtility.IsAvailablePosition(map, robots, pos, "Ivan Franchuk");
            Assert.IsFalse(result);
        }
    }
}
