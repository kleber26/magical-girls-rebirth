using System;
using System.Collections.Generic;
using World.View;

namespace World
{
    public static class MapOperations
    {
        public static List<CoordinateChanges> ShiftLineRight(Tile[][] map, int line)
        {
            List<CoordinateChanges> coordinateChanges = new List<CoordinateChanges>();

            for (int i = map[line].Length - 1; i > 0; i--)
            {
                int oldColumn = i - 1;
                coordinateChanges.Add(new CoordinateChanges((line, oldColumn), (line, i)));
                map[line][i] = map[line][oldColumn];
            }

            return coordinateChanges;
        }

        public static List<CoordinateChanges> ShiftLineLeft(Tile[][] map, int line)
        {
            List<CoordinateChanges> coordinateChanges = new List<CoordinateChanges>();

            for (int i = 0; i < map[line].Length - 1; i++)
            {
                int oldColumn = i + 1;
                coordinateChanges.Add(new CoordinateChanges((line, oldColumn), (line, i)));
                map[line][i] = map[line][oldColumn];
            }

            return coordinateChanges;
        }

        public static List<CoordinateChanges> ShiftColumnDown(Tile[][] map, int column)
        {
            List<CoordinateChanges> coordinateChanges = new List<CoordinateChanges>();

            for (int i = map.Length - 1; i > 0; i--)
            {
                int oldLine = i - 1;
                coordinateChanges.Add(new CoordinateChanges((oldLine, column), (i, column)));
                map[i][column] = map[oldLine][column];
            }

            return coordinateChanges;
        }

        public static List<CoordinateChanges> ShiftColumnUp(Tile[][] map, int column)
        {
            List<CoordinateChanges> coordinateChanges = new List<CoordinateChanges>();

            for (int i = 0; i < map.Length - 1; i++)
            {
                int oldLine = i + 1;
                coordinateChanges.Add(new CoordinateChanges((oldLine, column), (i, column)));
                map[i][column] = map[oldLine][column];
            }

            return coordinateChanges;
        }
//
//        public static List<CoordinateChanges> Swap(Tile[][] map, int firstLine, int firstColumn, int secondLine, int secondColumn)
//        {
//            List<CoordinateChanges> coordinateChanges = new List<CoordinateChanges>();
//            GameObject go1 = map[firstLine][firstColumn].Occupant;
//            GameObject go2 = map[secondLine][secondColumn].Occupant;
//
//            Tile aux = map[firstLine][firstColumn];
//            map[firstLine][firstColumn] = map[secondLine][secondColumn];
//            map[secondLine][secondColumn] = aux;
//
//            coordinateChanges.Add(new CoordinateChanges(go1, secondLine, secondColumn));
//            coordinateChanges.Add(new CoordinateChanges(go2, firstLine, firstColumn));
//
//            return coordinateChanges;
//        }

        public static List<CoordinateChanges> Carousel(Tile[][] map, (int, int) position, int carouselSize)
        {
            int line = position.Item1;
            int column = position.Item2;

            List<CoordinateChanges> coordinateChanges = new List<CoordinateChanges>();

            int degrees = 0;
            Tile previousValue = map[line][column + 1];
            (int, int) previousPosition = (line, column + 1);

            for (int i = 0; i < 4; i++)
            {
                double angle = Math.PI * degrees / 180.0;
                int xDirection = (int) Math.Cos(angle);
                int yDirection = (int) Math.Sin(angle);

                for (int j = 0; j < carouselSize - 1; j++)
                {
                    coordinateChanges.Add(new CoordinateChanges(previousPosition, (line, column)));
                    previousPosition = (line, column);

                    Tile auxPreviousValue = map[line][column];
                    map[line][column] = previousValue;
                    previousValue = auxPreviousValue;

                    line += xDirection;
                    column += yDirection;
                }

                degrees += 90;
            }

            return coordinateChanges;
        }
    }
}
