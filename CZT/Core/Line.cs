using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZT.Core
{
    public class Line
    {
        public int Length { get => Points.Count; }
        public List<Point> Points = new List<Point>();
        public readonly int Id;

        public Line(params Point[] points)
        {
            foreach (var p in points)
            {
                Id = p.Id;
                Points.Add(p);
            }
        }

        public static void Connect(Player player, Level level, Point p)
        {
            var neighbours = GetNeighbourPoints(level, p);
            if (IsIncorrectIEnumerable(neighbours))
            {
                var line = new Line(p);
                p.Lines.Add(line);
                player.Lines.Add(line);
                return;
            }
            var pair = FindPairNeighbour(neighbours);
            foreach (var point in neighbours)
            {
                var correctLines = FindCorrectLineInPoint(point);
                if (IsIncorrectIEnumerable(correctLines))
                {
                    var line = new Line(p, point);
                    p.Lines.Add(line);
                    player.Lines.Add(line);
                }//do something
                foreach (var line in correctLines)
                    line.Points.Add(point); 
            }
        }

        private static IEnumerable<Point> FindPairNeighbour(IEnumerable<Point> neighbour, Point center)
        {
            for (var dy = 0; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dy++)
        }

        private static bool IsIncorrectIEnumerable<T>(IEnumerable<T> ienum) 
            => !ienum.Any() || ienum == null;

        private static IEnumerable<Line> FindCorrectLineInPoint(Point point)
            => point.Lines
            .Where(line => line.Points
            .All(p => IsCorrectLine(point, p)));

        private static bool IsCorrectLine(Point p1, Point p2)
        {
            if (p1.X == p2.X) return true;
            if (p1.Y == p2.Y) return true;
            return Math.Abs(p1.X - p2.X) == 1 && Math.Abs(p1.Y - p2.Y) == 1;
        }

        private static IEnumerable<Point> GetNeighbourPoints(Level level, Point point)
        {
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;
                    if ((dx < 0 || dx >= level.width) && (dy < 0 || dy >= level.height)) continue;
                    var curPoint = new Point(point.X + dx, point.Y + dy, point.Id);
                    if (level.settedPoints.Contains(curPoint))
                        yield return curPoint;
                }
        }
    }
}
