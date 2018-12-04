using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018 {
    class Day02 : Day {
        public override string Name => "--- Day 2: Inventory Management System ---";

        public override string Input => InputResource.DAY02;

        string[] data;

        public override void ParseInput() {
            data = Input.Split('\n');
        }

        public override string PartOne() {
            Duplicates[] significant = data.Select(d => CheckDuplicates(d)).Where(d => d.HasTriplets || d.HasDoublets).ToArray();
            int doublets = significant.Where(s => s.HasDoublets).Count();
            int triplets = significant.Where(s => s.HasTriplets).Count();
            return (doublets * triplets).ToString();

        }

        public override string PartTwo() {
            return "n/a";
        }

        Duplicates CheckDuplicates(string input) {
            int[] counts = input.GroupBy(c => c).Select(g => g.Count()).ToArray();
            return new Duplicates() {
                HasDoublets = counts.Contains(2),
                HasTriplets = counts.Contains(3),
            };
        }

        struct Duplicates {
            public bool HasDoublets, HasTriplets;
        }
    }
}
