using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2018 {
    class Day03 : Day {
        const string regex = @"(?>#(?'id'\d+))\s+@\s+(?>(?'x'\d+),(?'y'\d+)):\s+(?>(?'w'\d+)x(?'h'\d+))";

        public override string Name => "--- Day 3: No Matter How You Slice It ---";

        public override string Input => InputResource.DAY03;

        Rect[] data;
        int[,] grid;
        

        public override void ParseInput() {
            RegexOptions options = RegexOptions.Multiline;
            data = Regex.Matches(Input, regex, options).Cast<Match>().Select(m => new Rect() {
                x = int.Parse(m.Groups["x"].Value),
                y = int.Parse(m.Groups["y"].Value),
                h = int.Parse(m.Groups["h"].Value),
                w = int.Parse(m.Groups["w"].Value), }).ToArray();
            Console.ReadKey();
            CreateGrid(ref grid);
        }

        public override string PartOne() {
            int overlappingCells = grid.Cast<int>().Where(i => i > 1).Count();
            return overlappingCells.ToString();
        }

        public override string PartTwo() {
            return "n/a";
        }

        void CreateGrid(ref int[,] grid) {
            grid = new int[data.Select(d => d.x + d.w).Max(),
                data.Select(d => d.y + d.h).Max()];
            foreach (Rect rect in data) {
                for (int x = rect.x; x < rect.x + rect.w; x++) {
                    for (int y = rect.y; y < rect.y + rect.h; y++) {
                        grid[x, y]++;
                    }
                }
            }
        }

        struct Rect {
            public int x, y, w, h;
        }
    }
}
