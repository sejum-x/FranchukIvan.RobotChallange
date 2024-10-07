using Moq;
using NUnit.Framework;
using Robot.Common;
using System.Collections.Generic;

namespace FranchukIvan.RobotChallange.Test
{
    public class CommandChainTests
    {
        private CommandChain _commandChain;
        private Mock<ICommandHandler> _mockHandler;

        [SetUp]
        public void Setup()
        {
            _commandChain = new CommandChain();
            _mockHandler = new Mock<ICommandHandler>();
        }

        [Test]
        public void Execute_Should_Return_CollectEnergyCommand_When_NearbyEnergyIs_Sufficient()
        {
            var robot = new Robot.Common.Robot { Position = new Position(0, 0), Energy = 100, OwnerName = "Ivan" };
            var robots = new List<Robot.Common.Robot>();
            var map = new Map(); 
            int currentRound = 1;

            _mockHandler.Setup(h => h.Handle(robot, robots, map, "Ivan", currentRound))
                .Returns(new CollectEnergyCommand());

            var result = _commandChain.Execute(robot, robots, map, "Ivan", currentRound);

            Assert.IsInstanceOf<CollectEnergyCommand>(result);
        }

        [Test]
        public void Execute_Should_Return_CreateNewRobotCommand_When_EnergyIs_Sufficient()
        {
            var robot = new Robot.Common.Robot { Position = new Position(0, 0), Energy = 300, OwnerName = "Ivan" };
            var robots = new List<Robot.Common.Robot>();
            var map = new Map();
            int currentRound = 1;

            _mockHandler.Setup(h => h.Handle(robot, robots, map, "Ivan", currentRound))
                .Returns(new CreateNewRobotCommand());

            var result = _commandChain.Execute(robot, robots, map, "Ivan", currentRound);

            Assert.IsInstanceOf<CreateNewRobotCommand>(result);
        }
        
        [Test]
        public void CollectEnergyHandler_Should_Return_CollectEnergyCommand_When_NearbyEnergy_Is_Above_Threshold()
        {
            var handler = new CollectEnergyHandler();
            var robot = new Robot.Common.Robot { Position = new Position(0, 0), Energy = 100, OwnerName = "Ivan" };
            var robots = new List<Robot.Common.Robot>();

            var map = new Map();
            map.Stations.Add(new EnergyStation { Position = new Position(1, 1), Energy = 200 });
            map.Stations.Add(new EnergyStation { Position = new Position(1, 2), Energy = 100 });

            var result = handler.Handle(robot, robots, map, "Ivan", 1);

            Assert.IsInstanceOf<CollectEnergyCommand>(result);
        }

        [Test]
        public void CollectEnergyHandler_Should_Return_Null_When_NearbyEnergy_Is_Below_Threshold()
        {
            var handler = new CollectEnergyHandler();
            var robot = new Robot.Common.Robot { Position = new Position(0, 0), Energy = 100, OwnerName = "Ivan" };
            var robots = new List<Robot.Common.Robot>();

            var map = new Map();
            map.Stations.Add(new EnergyStation { Position = new Position(1, 1), Energy = 100 });

            var result = handler.Handle(robot, robots, map, "Ivan", 1);

            Assert.IsNull(result);
        }


        [Test]
        public void CreateNewRobotHandler_Should_Return_CreateNewRobotCommand_When_Energy_Is_Above_Min()
        {
            var handler = new CreateNewRobotHandler();
            var robot = new Robot.Common.Robot { Energy = 300, OwnerName = "Ivan" };
            var robots = new List<Robot.Common.Robot>();
            var map = new Map();
            int currentRound = 1;

            var result = handler.Handle(robot, robots, map, "Ivan", currentRound);

            Assert.IsInstanceOf<CreateNewRobotCommand>(result);
        }

        [Test]
        public void CreateNewRobotHandler_Should_Return_Null_When_Energy_Is_Below_Min()
        {
            var handler = new CreateNewRobotHandler();
            var robot = new Robot.Common.Robot { Energy = 200, OwnerName = "Ivan" };
            var robots = new List<Robot.Common.Robot>();
            var map = new Map();
            int currentRound = 1;

            var result = handler.Handle(robot, robots, map, "Ivan", currentRound);

            Assert.IsNull(result);
        }


        [Test]
        public void AttackHandler_Should_Return_Null_When_No_Targets_Available()
        {
            var handler = new AttackHandler();
            var robot = new Robot.Common.Robot { Position = new Position(0, 0), Energy = 100, OwnerName = "Ivan" };
            var robots = new List<Robot.Common.Robot>();
            var map = new Map();
            int currentRound = 1;

            var result = handler.Handle(robot, robots, map, "Ivan", currentRound);

            Assert.IsNull(result);
        }
    }
}
