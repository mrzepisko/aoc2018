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
                id = int.Parse(m.Groups["id"].Value),
                x = int.Parse(m.Groups["x"].Value),
                y = int.Parse(m.Groups["y"].Value),
                h = int.Parse(m.Groups["h"].Value),
                w = int.Parse(m.Groups["w"].Value), }).ToArray();
            Console.ReadKey();
            CreateGrid();
        }

        public override string PartOne() {
            int overlappingCells = grid.Cast<int>().Where(i => i > 1).Count();
            return overlappingCells.ToString();
        }

        public override string PartTwo() {
            foreach (Rect rect in data) {
                int areaSum = 0;
                IterateOverSurface(grid, rect, (id, x, y, h, w) => { areaSum += grid[x, y]; });
                if (areaSum == rect.h * rect.w) return rect.id.ToString();
            }
            return "-1";
        }

        void CreateGrid() {
            grid = new int[data.Select(d => d.x + d.w).Max(),
                data.Select(d => d.y + d.h).Max()];
            foreach (Rect rect in data) {
                IterateOverSurface(grid, rect, (id, x, y, h, w) => { grid[x, y]++; });
            }
        }

        private static void IterateOverSurface(int[,] grid, Rect rect, Rect.Action action) {
            for (int x = rect.x; x < rect.x + rect.w; x++) {
                for (int y = rect.y; y < rect.y + rect.h; y++) {
                    action(rect.id, x, y, rect.w, rect.h);
                }
            }
        }

        struct Rect {
            public int id, x, y, w, h;
            public delegate void Action(int id, int x, int y, int w, int h);
        }
    }
}
