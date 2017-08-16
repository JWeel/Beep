﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Beep {
    public class RandomChangeRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required: 
        //  - TODO

        public RandomChangeRule(Dictionary<Point, Tile> tiles, List<Brush> colorArguments = null, List<int> intArguments = null) : base(tiles, colorArguments, intArguments) {
        }

        public override string RuleName { get => "Random Change Color"; }

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);

            return alteredTiles;
        }
    }
}
