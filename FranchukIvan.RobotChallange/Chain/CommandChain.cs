using System.Collections.Generic;
using Robot.Common;

namespace FranchukIvan.RobotChallange
{
    public class CommandChain
    {
        private readonly List<ICommandHandler> _handlers = new List<ICommandHandler>();

        public CommandChain()
        {
            _handlers.Add(new CreateNewRobotHandler());
            _handlers.Add(new CollectEnergyHandler());
            _handlers.Add(new AttackHandler());
            _handlers.Add(new MoveToBestPositionHandler());
        }

        public RobotCommand Execute(Robot.Common.Robot robot, IList<Robot.Common.Robot> robots, Map map, string author, int currentRound)
        {
            foreach (var handler in _handlers)
            {
                var result = handler.Handle(robot, robots, map, author, currentRound);
                if (result != null)
                {
                    return result;
                }
            }

            return new CollectEnergyCommand();
        }
    }
}