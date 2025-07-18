﻿using FranchukIvan.RobotChallange.Utilities;
using Robot.Common;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange
{
    public class AttackHandler : ICommandHandler
    {
        public RobotCommand Handle(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, Map map, string author, int currentRound)
        {
            var attackTargets = AttackUtility.GetRobotsToNearAttack(robot.Position, robots, author, currentRound);
            var adjacentTarget = attackTargets.FirstOrDefault(target => DistanceUtility.IsAdjacent(robot.Position, target.Value));

            if (adjacentTarget.Value != null)
            {
                return new MoveCommand { NewPosition = adjacentTarget.Value };
            }

            if (attackTargets.Count > 0)
            {
                var bestTarget = attackTargets.First();
                if (robot.Energy > DistanceUtility.GetDistanceCost(robot.Position, bestTarget.Value))
                {
                    return new MoveCommand { NewPosition = bestTarget.Value };
                }
            }

            return null;
        }
    }
}