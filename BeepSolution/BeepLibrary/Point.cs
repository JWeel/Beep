using System.Text.RegularExpressions;
namespace Beep {
    /// <summary>
    /// Represents a pair of integers x and y that define a point in a two-dimensional plane
    /// </summary>
    public struct Point {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y) {
            X = x;
            Y = y;
        }
        // parse a point from string.
        public Point(string s) {
            X = int.Parse(Regex.Match(s.Split(',')[0], @"-?\d+").Value);
            Y = int.Parse(Regex.Match(s.Split(',')[1], @"-?\d+").Value);
        }
        // points are represented in strike as "(x,y)"
        public override string ToString() {
            return string.Format("({0},{1})", X, Y);
        }
    }
}
