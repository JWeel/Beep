﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Beep {
    public class VirusRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required: 
        //  -? TODO

        public VirusRule(Dictionary<Point, Tile> tiles, List<Brush> colorArguments = null, List<int> intArguments = null) : base(tiles, colorArguments, intArguments) {
        }

        public override string RuleName { get => "Virus"; }

        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);
            return alteredTiles;
        }
    }
}
