using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018 {
    class Day01 : Day {
        public override string Name => "--- Day 1: Chronal Calibration ---";
        public override string Input => InputResource.DAY1;

        int[] data;

        public override void ParseInput(string input) {
            data = input.Split('\n').Select(s => int.Parse(s)).ToArray();
        }

        public override string PartOne() {
            return data.Sum().ToString();
        }

        public override string PartTwo() {
            HashSet<int> set = new HashSet<int>();
            int total = 0;
            int i = 0;
            do {
                set.Add(total);
                int df = data[i++ % data.Length];
                total += df;
            } while (!set.Contains(total));
            return total.ToString();
        }
    }
}
