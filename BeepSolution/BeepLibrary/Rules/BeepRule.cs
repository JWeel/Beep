using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Diagnostics;
using System.Linq;

namespace Beep.Rules {

    // a repository of actions
    public abstract class BeepRule {

        public const string RULE_CHANGE_COLOR = "Change Color";
        public const string RULE_CHANGE_NEIGHBOR_COLOR = "Change Neighbor Color";
        public const string RULE_RANDOM_CHANGE = "Random Change";
        public const string RULE_VIRUS = "Virus";

        public abstract string RuleName { get; }

        protected Dictionary<Point, Tile> tiles;
        protected List<Color> colorArguments;
        protected List<int> intArguments;
        protected List<bool> boolArguments;
        // protected List<double> doubleArguments;

        public BeepRule() { }

        public BeepRule(Dictionary<Point, Tile> tiles,
                List<Color> colorArguments = null,
                List<int> intArguments = null,
                List<bool> boolArguments = null,
                List<double> doubleArguments = null) {
            this.tiles = tiles;
            this.colorArguments = colorArguments;
            this.intArguments = intArguments;
            this.boolArguments = boolArguments;
           // this.doubleArguments = doubleArguments;
        }

        //
        public static BeepRule CreateBeepRule(string type, Dictionary<Point, Tile> bwTiles,
            List<Color> colorArguments = null,
            List<int> intArguments = null,
            List<bool> boolArguments = null) {
            switch (type) {
                case RULE_CHANGE_COLOR:
                    return new ChangeColorRule(bwTiles, colorArguments, intArguments, boolArguments);
                case RULE_CHANGE_NEIGHBOR_COLOR:
                    return new ChangeNeighborColorRule(bwTiles, colorArguments, intArguments, boolArguments);
                case RULE_RANDOM_CHANGE:
                    return new RandomChangeRule(bwTiles, colorArguments, intArguments, boolArguments);
                case RULE_VIRUS:
                    return new VirusRule(bwTiles, colorArguments, intArguments, boolArguments);
                default:
                    return null;
            }
        }

        //
        public static BeepRule CreateBeepRule(string type, Dictionary<Point, Tile> bwTiles) {
            switch (type) {
                case RULE_CHANGE_COLOR:
                    return new ChangeColorRule(bwTiles,
                        new List<Color> { (Color)ColorConverter.ConvertFromString("#FFFFA500"), (Color)ColorConverter.ConvertFromString("#FFF0FFFF") },
                        null,
                        null
                    );
                case RULE_CHANGE_NEIGHBOR_COLOR:
                    return new ChangeNeighborColorRule(bwTiles,
                        new List<Color> { (Color)ColorConverter.ConvertFromString("#FFFFA500"), (Color)ColorConverter.ConvertFromString("#FFF0FFFF") },
                        new List<int> { 6 },
                        new List<bool> { true }
                    );
                case RULE_RANDOM_CHANGE:
                    return new RandomChangeRule(bwTiles,
                        new List<Color> { (Color)ColorConverter.ConvertFromString("#FFFFA500") },
                        new List<int> { 30 },
                        null
                    );
                case RULE_VIRUS:
                    return new VirusRule(bwTiles, null, null, null);
                default:
                    return null;
            }
        }

        public abstract Dictionary<Point, Tile> Run();

        // returns a deep copy of a tiles dictionary
        protected Dictionary<Point, Tile> DeepCopyDict(Dictionary<Point, Tile> sourceDict) {
            Dictionary<Point, Tile> copyDict = new Dictionary<Point, Tile>();
            foreach (var v in tiles) copyDict[v.Key] = new Tile(v.Value);
            return copyDict;
        }

        // updates the tiles dictionary reference
        public void Update(Dictionary<Point, Tile> updatedTiles) {
            tiles = updatedTiles;
        }
    }
}
