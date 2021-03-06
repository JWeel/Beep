﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Beep.Rules {
    public class ChangeColorRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required: 
        //  -two colors in colorArguments
        //      -first color specifies the color that tiles must match
        //      -second color is what the matching tiles become

        // TODO possible extension: percentage reliability, percentage of tiles to change

        public ChangeColorRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles) {
            MatchColor = colorArguments[0];
            TargetColor = colorArguments[1];
        }

        public override string RuleName { get => RULE_CHANGE_COLOR; }

        public Color MatchColor { get; set; }
        public Color TargetColor { get; set; }


        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);

            foreach (Tile t in alteredTiles.Values) {
                if (t.Color == MatchColor) alteredTiles[t.Coordinates].Color = TargetColor;
            }
            return alteredTiles;
        }
    }
}
