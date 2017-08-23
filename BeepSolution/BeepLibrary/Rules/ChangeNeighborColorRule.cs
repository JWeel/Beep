using System.Collections.Generic;
using System.Windows.Media;

namespace Beep.Rules {
    public class ChangeNeighborColorRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required: 
        //  -two colors in colorArguments
        //      -first color specifies the color that tiles must match
        //      -second color is given to tiles that neighbor the matching tile
        //  -one int [0 < i < 6] in intArguments specifying number of tiles to change
        //  -one bool argument specifying whether to not affect neigbhors that are also matchers

        public ChangeNeighborColorRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles) {
            MatchColor = colorArguments[0];
            TargetColor = colorArguments[1];
            IgnoreColor = colorArguments[2];
            AmountAffectedNeighbors = intArguments[0];
            ColorNeighboringMatchers = boolArguments[0];
        }

        public override string RuleName { get => RULE_CHANGE_NEIGHBOR_COLOR; }

        public Color MatchColor { get; set; }
        public Color TargetColor { get; set; }
        public Color IgnoreColor { get; set; } // TODO list, and listbox in usercontrol
        public int AmountAffectedNeighbors { get; set; }
        public bool ColorNeighboringMatchers { get; set; }

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);

            foreach (Tile t in tiles.Values) {
                if (t.Color == MatchColor) {
                    int amountColorsChanged = 0;
                    foreach (Point p in t.Neighbors) {

                        if (ColorNeighboringMatchers && tiles[p].Color == MatchColor) continue;
                        if (tiles[p].Color == IgnoreColor) continue;
                        // TODO? bool foundIgnore; foreach(Color c in IgnoreColors) if (tiles[p].Color == c)  { foundIgnore = true; break; } if(foundIgnore) continue;

                        if (amountColorsChanged >= AmountAffectedNeighbors) break;
                        amountColorsChanged++;

                        alteredTiles[p].Color = TargetColor;
                    }
                }
            }
            return alteredTiles;
        }
    }
}
