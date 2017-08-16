using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Beep.Rules {
    public class ChangeNeighborColorRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required: 
        //  -two colors in colorArguments
        //      -first color specifies matches tiles
        //      -second color is given to tiles that neighbor the matching tile
        //  -one int [0 < i < 6] in intArguments specifying number of tiles to change TODO
        //  -one bool argument specifying whether to not affect neigbhors that are also matchers

        private Brush matchColor;
        private Brush targetColor;
        private bool colorNeighboringMatchers;

        public ChangeNeighborColorRule(Dictionary<Point, Tile> tiles, List<Brush> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles, colorArguments, intArguments, boolArguments) {
            matchColor = colorArguments[0];
            targetColor = colorArguments[1];
            colorNeighboringMatchers = boolArguments[0];
        }

        public override string RuleName { get => BeepRule.RULE_CHANGE_NEIGHBOR_COLOR; }

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);

            foreach (Tile t in tiles.Values) {
                if (t.Color == matchColor) {
                    foreach (Point p in t.Neighbors) {

                        if (colorNeighboringMatchers && tiles[p].Color == matchColor) continue;

                        alteredTiles[p].Color = targetColor;
                    }
                }
            }
            return alteredTiles;
        }
    }
}
