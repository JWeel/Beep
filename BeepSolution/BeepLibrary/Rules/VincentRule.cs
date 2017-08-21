using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace Beep.Rules {
    public class VincentRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required: 
        //  - TODO

        public VincentRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles) {
            MatchColor = colorArguments[0];
            TargetColor = colorArguments[1];
        }

        public override string RuleName { get => RULE_VINCENT; }

        public Color MatchColor { get; set; }
        public Color TargetColor { get; set; }

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);

            List<Tile> matchers = tiles.Values.Where(t => t.Color == MatchColor).ToList();
            Debug.WriteLine(string.Format("{0},{1}", matchers, matchers.Count));


            return alteredTiles;
        }
    }
}
