using NUnit.Framework;
using Robot.Common;

namespace FranchukIvan.RobotChallange.Test
{
    public class IvanFranchukAlgorithmTests
    {
        private IvanFranchukAlgorithm algorithm;
        private Map map;
        private IList<Robot.Common.Robot> robots;
        private CommandChain commandChain;

        [SetUp]
        public void Setup()
        {
            commandChain = new CommandChain();
            algorithm = new IvanFranchukAlgorithm(commandChain);

            map = new Map();

            robots = new List<Robot.Common.Robot>
            {
                new Robot.Common.Robot { Position = new Position(0, 0), Energy = 500, OwnerName = "Ivan Franchuk" },
                new Robot.Common.Robot { Position = new Position(4, 4), Energy = 300, OwnerName = "Opponent" },
                new Robot.Common.Robot { Position = new Position(10, 10), Energy = 150, OwnerName = "Opponent" }
            };
        }

        [Test]
        public void Should_Create_New_Robot_When_Energy_Enough()
        {
            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsInstanceOf<CreateNewRobotCommand>(command);
        }

        [Test]
        public void Should_Attack_When_Enemy_Nearby()
        {
            robots[0].Energy = 200;
            robots[0].Position = new Position(5, 4);

            robots[1].Energy = 1000;
            robots[1].Position = new Position(5, 5);

            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsInstanceOf<MoveCommand>(command);
            var moveCommand = command as MoveCommand;
            Assert.AreEqual(new Position(5, 5), moveCommand.NewPosition);
        }


        [Test]
        public void Should_Collect_Energy_When_Station_Nearby()
        {
            robots[0].Energy = 100;
            map.Stations.Add(new EnergyStation { Position = new Position(1, 1), Energy = 200 });

            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsInstanceOf<CollectEnergyCommand>(command);
        }

        [Test]
        public void Should_Not_Create_New_Robot_When_Limit_Reached()
        {
            for (int i = 0; i < 100; i++)
            {
                robots.Add(new Robot.Common.Robot { Position = new Position(i, i), OwnerName = "Ivan Franchuk", Energy = 300 });
            }

            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsNotInstanceOf<CreateNewRobotCommand>(command);
        }


        [Test]
        public void Should_Not_Attack_When_No_Enemies()
        {
            robots.RemoveAt(1);

            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsNotInstanceOf<MoveCommand>(command);
        }

        [Test]
        public void Should_Not_Collect_Energy_If_No_Station()
        {
            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsNotInstanceOf<CollectEnergyCommand>(command);
        }

        [Test]
        public void Should_Not_Move_If_No_Better_Position()
        {
            map.Stations.Add(new EnergyStation { Position = new Position(0, 0), Energy = 300 });

            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsNotInstanceOf<MoveCommand>(command);
        }

        [Test]
        public void Should_Not_Create_New_Robot_When_Not_Enough_Energy()
        {
            robots[0].Energy = 100;

            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsNotInstanceOf<CreateNewRobotCommand>(command);
        }


        [Test]
        public void Should_Attack_Adjacent_Target()
        {
            robots[0].Position = new Position(1, 1);
            robots[0].Energy = 200;

            robots[1].Position = new Position(1, 2);
            robots[1].Energy = 1000;

            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsInstanceOf<MoveCommand>(command);
            var moveCommand = command as MoveCommand;
            Assert.AreEqual(new Position(1, 2), moveCommand.NewPosition);
        }

        [Test]
        public void Should_Attack_Best_Target_When_No_Adjacent_Target()
        {
            robots[0].Position = new Position(1, 1);
            robots[0].Energy = 200;

            robots[1].Position = new Position(5, 5);
            robots[1].Energy = 1000;

            var robotToMoveIndex = 0;
            var command = algorithm.DoStep(robots, robotToMoveIndex, map);

            Assert.IsInstanceOf<MoveCommand>(command);
            var moveCommand = command as MoveCommand;
            Assert.AreEqual(new Position(5, 5), moveCommand.NewPosition);
        }

        [Test]
        public void Should_Have_Correct_Description()
        {
            Assert.AreEqual("Ivan Franchuk's Robot Algorithm for competition.", algorithm.Description);
        }
    }
}
