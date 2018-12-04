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
            int distance = int.MaxValue;
            int pick0 = -1, pick1 = -1;
            for (int id0 = 0; id0 < data.Length; id0++) {
                for (int id1 = id0 + 1; id1 < data.Length; id1++) {
                    int currDistance = Utils.LevenshteinDistance(data[id0], data[id1]);
                    if (currDistance < distance) {
                        pick0 = id0; pick1 = id1;
                        distance = currDistance;
                    }
                }
            }
            string s0 = data[pick0], s1 = data[pick1];
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < s0.Length; i++) {
                if (s0[i] == s1[i]) {
                    result.Append(s0[i]);
                }
            }
            return result.ToString();
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
