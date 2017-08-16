﻿using System;
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
        //  -one int [0 < i < 6] in intArguments

        public ChangeNeighborColorRule(Dictionary<Point, Tile> tiles, List<Brush> colorArguments = null, List<int> intArguments = null) : base(tiles, colorArguments, intArguments) {
        }

        public override string RuleName { get => "Change Neighbor Color"; }

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);

            foreach (Tile t in tiles.Values) {
                if (t.Color == colorArguments[0]) {
                    foreach (Point p in t.Neighbors) {
                        alteredTiles[p].Color = colorArguments[1];
                    }
                }
            }
            return alteredTiles;
        }
    }
}
