using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FranchukIvan.RobotChallange
{
    internal class Functions
    {
        protected Functions() { }

        private const int NearbyRadius = 1;

        public static int GetDistanceCost(Position center, Position distant) =>
            (center.X - distant.X) * (center.X - distant.X) + (center.Y - distant.Y) * (center.Y - distant.Y);

        public static int GetAuthorRobotCount(IList<Robot.Common.Robot> robots, string author) =>
            robots.Count(robot => robot.OwnerName == author);

        public static int GetNearbyRobotCount(IList<Robot.Common.Robot> robots, Position position) =>
            robots.Count(robot => IsWithinRadius(position, robot.Position, NearbyRadius));

        public static bool IsWithinRadius(Position center, Position point, int radius) =>
            Math.Abs(center.X - point.X) <= radius && Math.Abs(center.Y - point.Y) <= radius;

        public static bool IsAvailablePosition(Map map, IList<Robot.Common.Robot> robots, Position position, string author)
        {
            if (position.X > 100 || position.X < 0 || position.Y > 100 || position.Y < 0)
                return false;

            return !robots.Any(robot => robot.Position.Equals(position) && robot.OwnerName == author);
        }

        public static List<KeyValuePair<int, Position>> BestPositions(Map map, Position center, int radius)
        {
            int length = 2 * radius + 1;
            int[,] energyGrid = new int[length, length];
            int topLeftY = center.Y - radius;
            int topLeftX = center.X - radius;

            // Заповнюємо енергетичну матрицю
            FillEnergyGrid(map, topLeftX, topLeftY, energyGrid, length);

            // Оцінюємо позиції з урахуванням витрат на переміщення
            var evaluatedPositions = EvaluatePositions(center, topLeftX, topLeftY, energyGrid, length);

            // Сортуємо позиції за вигідністю
            return SortPositionsByValue(evaluatedPositions);
        }

        // Заповнюємо енергетичну матрицю з урахуванням енергетичних станцій
        private static void FillEnergyGrid(Map map, int topLeftX, int topLeftY, int[,] energyGrid, int length)
        {
            map.Stations
                .Where(station => IsStationInRange(station, topLeftX, topLeftY, length))
                .ToList()
                .ForEach(station => AddStationEnergyToGrid(station, topLeftX, topLeftY, energyGrid, length));
        }

        // Перевіряємо, чи знаходиться станція в радіусі досліджуваної області
        private static bool IsStationInRange(EnergyStation station, int topLeftX, int topLeftY, int length)
        {
            return station.Position.X >= topLeftX - 2 && station.Position.X < topLeftX + length + 2 &&
                   station.Position.Y >= topLeftY - 2 && station.Position.Y < topLeftY + length + 2;
        }

        // Додаємо енергію станції до позицій в радіусі 2
        private static void AddStationEnergyToGrid(EnergyStation station, int topLeftX, int topLeftY, int[,] energyGrid, int length)
        {
            int stationOffsetX = station.Position.X - topLeftX;
            int stationOffsetY = station.Position.Y - topLeftY;

            for (int offsetY = -2; offsetY <= 2; ++offsetY)
            {
                for (int offsetX = -2; offsetX <= 2; ++offsetX)
                {
                    int gridX = stationOffsetX + offsetX;
                    int gridY = stationOffsetY + offsetY;

                    if (gridX >= 0 && gridX < length && gridY >= 0 && gridY < length)
                        energyGrid[gridY, gridX] += station.Energy;
                }
            }
        }

        // Оцінюємо всі позиції з урахуванням витрат на переміщення
        private static List<KeyValuePair<int, Position>> EvaluatePositions(Position center, int topLeftX, int topLeftY, int[,] energyGrid, int length)
        {
            return Enumerable.Range(0, length)
                .SelectMany(y => Enumerable.Range(0, length)
                    .Select(x => new KeyValuePair<int, Position>(
                        energyGrid[y, x] - GetDistanceCost(center, new Position(topLeftX + x, topLeftY + y)),
                        new Position(topLeftX + x, topLeftY + y))))
                .ToList();
        }

        // Сортуємо позиції за спаданням
        private static List<KeyValuePair<int, Position>> SortPositionsByValue(List<KeyValuePair<int, Position>> positions)
        {
            positions.Sort((pair1, pair2) => -pair1.Key.CompareTo(pair2.Key));
            return positions;
        }
    }
}