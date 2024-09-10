using Robot.Common;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange
{
    public class IvanFranchukAlgorithm : IRobotAlgorithm
    {
        private int Fri = -1;
        private int round = 0;

        public string Author => "Ivan Franchuk";

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            if (Fri == -1)
                Fri = robotToMoveIndex;

            if (Fri == robotToMoveIndex)
                ++round;

            if (round == 51)
                return new CollectEnergyCommand();

            Robot.Common.Robot robot = robots[robotToMoveIndex];

            if (round % 10 == 5)
            {
                var bestPosition = Functions.BestPositions(map, robot.Position, 100)
                    .ToList()
                    .Find(p => Functions.IsAvailablePosition(map, robots, p.Value, Author) &&
                               robot.Energy > Functions.GetDistanceCost(robot.Position, p.Value));

                if (bestPosition.Value != null)
                {
                    return new MoveCommand { NewPosition = bestPosition.Value };
                }
            }

            if (Functions.GetAuthorRobotCount(robots, Author) < 100 && robot.Energy > 300)
                return new CreateNewRobotCommand();

            IList<EnergyStation> nearbyResources = map.GetNearbyResources(robot.Position, 2);
            if (nearbyResources.Any(energyStation => round < 5 || energyStation.Energy > 100))
            {
                return new CollectEnergyCommand();
            }

            var nextBestPosition = Functions.BestPositions(map, robot.Position, 5)
                .ToList()
                .Find(p => Functions.IsAvailablePosition(map, robots, p.Value, Author) &&
                           robot.Energy > Functions.GetDistanceCost(robot.Position, p.Value));

            if (nextBestPosition.Value != null)
            {
                return new MoveCommand { NewPosition = nextBestPosition.Value };
            }

            return new CollectEnergyCommand();
        }

        public int Round { get; set; }

        public IvanFranchukAlgorithm() => Logger.OnLogRound += LogRound;

        private void LogRound(object sender, LogRoundEventArgs e) => ++Round;
    }
}