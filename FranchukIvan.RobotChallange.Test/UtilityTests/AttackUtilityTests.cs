using FranchukIvan.RobotChallange.Utilities;
using NUnit.Framework;
using Robot.Common;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange.Test
{
    [TestFixture]
    public class AttackUtilityTests
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
        public void GetRobotsToNearAttack_Should_Calculate_NetEnergyGain()
        {
            Position currentPosition = new Position(0, 0);
            int roundNumber = 1;
            robots.Add(new Robot.Common.Robot { Position = new Position(2, 2), Energy = 400, OwnerName = "Opponent" });

            var result = AttackUtility.GetRobotsToNearAttack(currentPosition, robots, "Ivan Franchuk", roundNumber);
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

            var result = AttackUtility.GetRobotsToNearAttack(currentPosition, robots, "Ivan Franchuk", roundNumber);

            Assert.IsTrue(result.Count >= 2, "Expected at least two robots in the result");
            Assert.AreEqual(1, result[0].Value.X);
            Assert.AreEqual(2, result[1].Value.X);
        }
    }
}
