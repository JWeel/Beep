﻿using System;
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
        public const string RULE_VINCENT = "Vincent";
        public const string RULE_LIFE = "Life";

        public abstract string RuleName { get; }

        protected Dictionary<Point, Tile> tiles;
        protected List<Color> colorArguments;
        protected List<int> intArguments;
        protected List<bool> boolArguments;
        
        public BeepRule(Dictionary<Point, Tile> tiles) {
            this.tiles = tiles;
        }
        
        //
        public static BeepRule Create(string type, Dictionary<Point, Tile> bwTiles) {
            switch (type) {
                case RULE_CHANGE_COLOR:
                    return new ChangeColorRule(bwTiles,
                        new List<Color> { (Color)ColorConverter.ConvertFromString("#FFFFA500"), (Color)ColorConverter.ConvertFromString("#FFF0FFFF") }
                    );
                case RULE_CHANGE_NEIGHBOR_COLOR:
                    return new ChangeNeighborColorRule(bwTiles,
                        new List<Color> { (Color)ColorConverter.ConvertFromString("#FFFFA500"), (Color)ColorConverter.ConvertFromString("#FFF0FFFF"), (Color)ColorConverter.ConvertFromString("#FFFFFFFF") },
                        new List<int> { 6 },
                        new List<bool> { true }
                    );
                case RULE_RANDOM_CHANGE:
                    return new RandomChangeRule(bwTiles,
                        new List<Color> { (Color)ColorConverter.ConvertFromString("#FFFFA500") },
                        new List<int> { 5 },
                        new List<bool> { true, false }
                    );
                case RULE_VIRUS:
                    return new VirusRule(bwTiles, 
                        colorArguments: new List<Color> { (Color)ColorConverter.ConvertFromString("#FFFFA500"), (Color)ColorConverter.ConvertFromString("#FFF0FFFF"), (Color)ColorConverter.ConvertFromString("#FFF05E1C") },
                        intArguments: new List<int> { 1 }, boolArguments: new List<bool> { true, false });
                case RULE_VINCENT:
                    return new VincentRule(bwTiles,
                        colorArguments: new List<Color> { (Color)ColorConverter.ConvertFromString("#FFFFA500"), (Color)ColorConverter.ConvertFromString("#FFABCDEF"), (Color)ColorConverter.ConvertFromString("#FFFFFFFF") }
                    );
                case RULE_LIFE:
                    return new LifeRule(bwTiles,
                        colorArguments: new List<Color> { (Color)ColorConverter.ConvertFromString("#FFFFFFFF"), (Color)ColorConverter.ConvertFromString("#FF000000") },
                        boolArguments: new List<bool> { true }
                    );
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
