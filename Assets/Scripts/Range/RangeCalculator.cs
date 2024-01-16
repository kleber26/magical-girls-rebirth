using System;
using System.Collections.Generic;
using System.Linq;
using Skills.View;
using World;

namespace Range
{
    public class RangeCalculator
    {
        private MapController mapController;
        Dictionary<SkillArea, Delegate> rangeMethods;

        public RangeCalculator(MapController mapController)
        {
            this.mapController = mapController;
            rangeMethods = CreateRangeDict();
        }

        private Dictionary<SkillArea, Delegate> CreateRangeDict()
        {
            rangeMethods = new Dictionary<SkillArea, Delegate>();
            rangeMethods[SkillArea.X] = new Func<(int, int), int, List<(int, int)>>(GetXRange);
            rangeMethods[SkillArea.Cross] = new Func<(int, int), int, List<(int, int)>>(GetCrossRange);
            rangeMethods[SkillArea.Square] = new Func<(int, int), int, List<(int, int)>>(GetSquareRange);
            rangeMethods[SkillArea.HorizontalLine] = new Func<(int, int), int, List<(int, int)>>(GetHorizontalLineRange);
            rangeMethods[SkillArea.VerticalLine] = new Func<(int, int), int, List<(int, int)>>(GetVerticalLineRange);

            return rangeMethods;
        }

        public List<(int, int)> GetRange(SkillArea skillArea, int range, (int, int) startPoint)
        {
            return (List<(int, int)>) rangeMethods[skillArea].DynamicInvoke(startPoint, range);
        }

                public List<(int, int)> GetCrossRange((int, int) startPoint, int range)
        {
            List<(int, int)> crossRange = new List<(int, int)>();

            int startI = startPoint.Item1;
            int startJ = startPoint.Item2;

            if (range < 0 || mapController.IsTileVoid(startPoint))
            {
                return new List<(int, int)>();
            }

            crossRange.Add(startPoint);

            if (startI > 0)
            {
                crossRange = crossRange.Concat(GetCrossRange((startI - 1, startJ), range - 1)).ToList();
            }

            if (startI < (mapController.MapHeight() - 1))
            {
                crossRange = crossRange.Concat(GetCrossRange((startI + 1, startJ), range - 1)).ToList();
            }

            if (startJ > 0)
            {
                crossRange = crossRange.Concat(GetCrossRange((startI, startJ - 1), range - 1)).ToList();
            }

            if (startJ < (mapController.MapWidth() - 1))
            {
                crossRange = crossRange.Concat(GetCrossRange((startI, startJ + 1), range - 1)).ToList();
            }

            return crossRange.Distinct().ToList();
        }

        public List<(int, int)> GetRectangularRange((int, int) startPoint, int verticalRange, int horizontalRange)
        {
            List<(int, int)> retangularRange = new List<(int, int)>();

            int startI = startPoint.Item1;
            int startJ = startPoint.Item2;

            for (int i = (startI - verticalRange); i <= (startI + verticalRange); i++)
            {
                if (i < 0 || i >= mapController.MapHeight())
                {
                    continue;
                }

                for (int j = (startJ - horizontalRange); j <= (startJ + horizontalRange); j++)
                {
                    if (j < 0 || j >= mapController.MapWidth() || mapController.IsTileVoid((i, j)))
                    {
                        continue;
                    }

                    retangularRange.Add((i, j));

                }
            }

            return retangularRange;
        }

        public List<(int, int)> GetSquareRange((int, int) startPoint, int range)
        {
            return GetRectangularRange(startPoint, range, range);
        }

        public List<(int, int)> GetVerticalLineRange((int, int) startPoint, int range)
        {
            return GetRectangularRange(startPoint, range, 0);
        }

        public List<(int, int)> GetHorizontalLineRange((int, int) startPoint, int range)
        {
            return GetRectangularRange(startPoint, 0, range);
        }

        public List<(int, int)> GetXRange((int, int) startPoint, int range)
        {
            List<(int, int)> xRange = new List<(int, int)>();

            int startI = startPoint.Item1;
            int startJ = startPoint.Item2;

            if (range < 0)
            {
                return new List<(int, int)>();
            }

            if ((startI - range) >= 0 && (startJ - range) >= 0)
            {
                xRange.Add((startI - range, startJ - range));
            }

            if ((startI - range) >= 0 && (startJ + range) <= (mapController.MapWidth() - 1))
            {
                xRange.Add((startI - range, startJ + range));
            }

            if ((startI + range) <= (mapController.MapHeight() - 1) && (startJ + range) <= (mapController.MapWidth() - 1))
            {
                xRange.Add((startI + range, startJ + range));
            }

            if ((startI + range) <= (mapController.MapHeight() - 1) && (startJ - range) >= 0)
            {
                xRange.Add((startI + range, startJ - range));
            }

            xRange = xRange.Concat(GetXRange(startPoint, range - 1)).ToList();

            return xRange.Distinct().Where(r => !mapController.IsTileVoid(r)).ToList();
        }
    }
}
