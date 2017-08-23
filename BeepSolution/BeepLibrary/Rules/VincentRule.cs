using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace Beep.Rules {
    public class VincentRule : BeepRule {

        // this rule creates Van Gogh-esque lines between pairs of points
        // required: 
        //  -three colors in colorArguments
        //      -first color specifies the color that matches the point pairs
        //      -second color is given to tiles in a line between those points
        //      -third color is for tiles that are ignored despite being on the line

        public VincentRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles) {
            MatchColor = colorArguments[0];
            TargetColor = colorArguments[1];
            IgnoreColor = colorArguments[2];
        }

        public override string RuleName { get => RULE_VINCENT; }

        public Color MatchColor { get; set; }
        public Color TargetColor { get; set; }
        public Color IgnoreColor { get; set; }

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);

            List<Tile> matchers = tiles.Values.Where(t => t.Color == MatchColor).ToList();
            List<Tuple<Tile, Tile>> pairs = new List<Tuple<Tile, Tile>>();

            // find pairs
            while (matchers.Count >= 2) {
                pairs.Add(new Tuple<Tile, Tile>(matchers[0], matchers[1]));
                matchers.RemoveAt(1);
                matchers.RemoveAt(0);
            }
            // span a line between points in a pair, coloring all tiles on that line
            pairs.ForEach(pair => {
                int lineX1 = pair.Item1.Coordinates.X;
                int lineX2 = pair.Item2.Coordinates.X;
                int lineY1 = pair.Item1.Coordinates.Y;
                int lineY2 = pair.Item2.Coordinates.Y;

                int dx = Math.Abs(lineX1 - lineX2);
                int dy = Math.Abs(lineY1 - lineY2);
                int x = lineX1;
                int y = lineY1;
                int n = 1 + dx + dy;
                int x_inc = (lineX1 > lineX2) ? -1 : 1;
                int y_inc = (lineY1 > lineY2) ? -1 : 1;
                int error = dx - dy;
                dx *= 2;
                dy *= 2;

                for (; n > 0; --n) {
                    Point p = new Point(x, y);
                    if (alteredTiles.ContainsKey(p) && alteredTiles[p].Color != IgnoreColor) {
                        alteredTiles[p].Color = TargetColor;
                    }

                    if (error > 0) {
                        x += x_inc;
                        error -= dy;
                    }
                    else {
                        y += y_inc;
                        error += dx;
                    }
                }
            });

            return alteredTiles;
        }
    }
}
