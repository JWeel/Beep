using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Beep.Rules {
    public class RandomChangeRule : BeepRule {

        // this rule randomly selects tiles and assigns them a defined color
        // required: 
        // -one color argument specifying what color the random tiles are changed to
        // -one int argument specifying what permillage (1/1000) of tiles is affected
        //  -two bools in boolArguments
        //      -first bool specifies whether to use a fixed target color
        //      -second bool specifies if the random color should be random for every affected tile

        public RandomChangeRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles) {
            TargetColor = colorArguments[0];
            PermillageAffected = intArguments[0];
            UseFixedColor = boolArguments[0];
            FullyRandomizeColors = boolArguments[1];
            random = new Random();
        }

        public override string RuleName { get => RULE_RANDOM_CHANGE; }

        public Color TargetColor { get; set; }
        public int PermillageAffected { get; set; }
        public bool UseFixedColor { get; set; }
        public bool FullyRandomizeColors { get; set; }

        private Random random;

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);
            Color randomColor = Color.FromRgb(0,0,0);
            if (!UseFixedColor && !FullyRandomizeColors) {
                string colorString = String.Format("#{0:X6}", random.Next(0x1000000));
                randomColor = (Color)ColorConverter.ConvertFromString(colorString);
            }
            foreach (Tile t in tiles.Values) {
                if (random.Next(1000) < PermillageAffected) {
                    if (!UseFixedColor) {
                        if (FullyRandomizeColors) {
                            string colorString = String.Format("#{0:X6}", random.Next(0x1000000));
                            alteredTiles[t.Coordinates].Color = (Color)ColorConverter.ConvertFromString(colorString);
                        }
                        else alteredTiles[t.Coordinates].Color = randomColor;
                    }
                    else alteredTiles[t.Coordinates].Color = TargetColor;
                }
            }
            return alteredTiles;
        }
    }
}
