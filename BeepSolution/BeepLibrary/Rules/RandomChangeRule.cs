using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Beep.Rules {
    public class RandomChangeRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required: 
        //  - TODO

        public RandomChangeRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) {
            TargetColor = colorArguments[0];
            PercentageAffected = intArguments[0];
            random = new Random();
        }

        public override string RuleName { get => RULE_CHANGE_NEIGHBOR_COLOR; }

        public Color TargetColor { get; set; }
        public int PercentageAffected { get; set; }

        private Random random;

        // TODO randomized target color(s)
        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);
            foreach (Tile t in tiles.Values) {
                if (random.Next(100) < PercentageAffected) t.Color = TargetColor;
            }
            return alteredTiles;
        }
    }
}
