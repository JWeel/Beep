using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace Beep.Rules {
    public class LifeRule : BeepRule {

        // this rule simulates conway's game of life
        // required: 
        //  -two colors in colorArguments
        //      -first color is for living cells in the game of life
        //      -second color is for dead cells
        // -one boolean argument specifying whether to color unrelated cells (tiles not life or dead)

        public LifeRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles) {
            LifeColor = colorArguments[0];
            DeadColor = colorArguments[1];
            ConvertUnrelatedCells = boolArguments[0];
        }

        public override string RuleName { get => RULE_LIFE; }

        public Color LifeColor { get; set; }
        public Color DeadColor { get; set; }
        public bool ConvertUnrelatedCells { get; set; }

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);
            foreach(Tile t in tiles.Values) {
                List<Point> neighborCoordinates = t.Neighbors;
                int lifeNeighborsCount = 0;
                foreach(Point p in neighborCoordinates) {
                    if (tiles[p].Color == LifeColor) lifeNeighborsCount++;
                }
                if (t.Color == LifeColor && (lifeNeighborsCount == 3 || lifeNeighborsCount == 4)) ;
                else if (t.Color == LifeColor) alteredTiles[t.Coordinates].Color = DeadColor;
                else if (t.Color == DeadColor && lifeNeighborsCount == 2) alteredTiles[t.Coordinates].Color = LifeColor;
                else if (ConvertUnrelatedCells) alteredTiles[t.Coordinates].Color = DeadColor;
            }

            return alteredTiles;
        }
    }
}
