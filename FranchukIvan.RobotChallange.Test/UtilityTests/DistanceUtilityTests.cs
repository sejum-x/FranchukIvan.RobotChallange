using FranchukIvan.RobotChallange.Utilities;
using NUnit.Framework;
using Robot.Common;

namespace FranchukIvan.RobotChallange.Test
{
    [TestFixture]
    public class DistanceUtilityTests
    {
        [Test]
        public void IsAdjacent_Should_Return_True_When_Positions_Are_Adjacent()
        {
            Position pos1 = new Position(1, 1);
            Position pos2 = new Position(1, 2);
            bool result = DistanceUtility.IsAdjacent(pos1, pos2);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsAdjacent_Should_Return_False_When_Positions_Are_Not_Adjacent()
        {
            Position pos1 = new Position(1, 1);
            Position pos2 = new Position(3, 3);
            bool result = DistanceUtility.IsAdjacent(pos1, pos2);
            Assert.IsFalse(result);
        }

        [Test]
        public void GetDistanceCost_Should_Return_Correct_Distance()
        {
            Position pos1 = new Position(0, 0);
            Position pos2 = new Position(3, 4);
            int distance = DistanceUtility.GetDistanceCost(pos1, pos2);
            Assert.AreEqual(25, distance);
        }
    }
}   