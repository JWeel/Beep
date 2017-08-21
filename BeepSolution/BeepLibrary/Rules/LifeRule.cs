using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace Beep.Rules {
    public class LifeRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required: 
        //  - TODO

        public LifeRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles) {
            LifeColor = colorArguments[0];
            DeadColor = colorArguments[1];
        }

        public override string RuleName { get => RULE_LIFE; }

        public Color LifeColor { get; set; }
        public Color DeadColor { get; set; }

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);
            
            foreach(Tile t in tiles.Values) {
                List<Point> neighborCoordinates = t.Neighbors;
                int lifeNeighborsCount = 0;
                foreach(Point p in neighborCoordinates) {
                    if (tiles[p].Color == LifeColor) lifeNeighborsCount++;
                }
                if (t.Color == LifeColor && (lifeNeighborsCount == 3 || lifeNeighborsCount == 4)) ;
                else if (t.Color == DeadColor && lifeNeighborsCount == 2) alteredTiles[t.Coordinates].Color = LifeColor;
                else alteredTiles[t.Coordinates].Color = DeadColor;
            }

            return alteredTiles;
        }
    }
}
