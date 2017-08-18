﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Beep.Rules {
    public class RandomChangeRule : BeepRule {

        // this rule changes tiles matching first color in colorArguments to second color in colorArguments
        // required: 
        //  - TODO

        public RandomChangeRule(Dictionary<Point, Tile> tiles, List<Color> colorArguments = null, List<int> intArguments = null, List<bool> boolArguments = null) : base(tiles) {
            TargetColor = colorArguments[0];
            PermillageAffected = intArguments[0];
            random = new Random();
        }

        public override string RuleName { get => RULE_RANDOM_CHANGE; }

        public Color TargetColor { get; set; }
        public int PermillageAffected { get; set; }

        private Random random;

        // TODO randomized target color(s)
        public override Dictionary<Point, Tile> Run() {
            Dictionary<Point, Tile> alteredTiles = DeepCopyDict(tiles);
            foreach (Tile t in tiles.Values) {
                if (random.Next(1000) < PermillageAffected) alteredTiles[t.Coordinates].Color = TargetColor;
            }
            Debug.WriteLine("oi govna");
            return alteredTiles;
        }
    }
}
