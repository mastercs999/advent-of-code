using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._22
{
    public class PathPoint
    {
        public Point Location { get; set; }
        public RegionType RegionType { get; set; }
        public Dictionary<Tool, Origin> ToolToOrigin { get; set; }

        public IEnumerable<PathPoint> Neighbours(Dictionary<Point, PathPoint> map)
        {
            foreach ((int dx, int dy) in new (int, int)[] { (0, -1), (-1, 0), (1, 0), (0, 1) })
            {
                Point neighbourLocation = new Point(Location.X + dx, Location.Y + dy);

                if (map.TryGetValue(neighbourLocation, out PathPoint neighbour))
                    yield return neighbour;
            }
        }
    }
}
