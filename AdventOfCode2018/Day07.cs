using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace AdventOfCode2018 {
    internal class Day07 : Day {
        private const string regex = @"Step (?'first'\w) must be finished before step (?'second'\w) can begin\.";

        public override string Name => "--- Day 7: The Sum of Its Parts ---";

        public override string Input => InputResource.DAY07;

        const string PATTERN = @"^.+\s(\w)\s.+\s(\w)\s.+$";

        Dictionary<char, Step> steps;

        public override void ParseInput(string input) {
            throw new NotSupportedException();
            steps = new Dictionary<char, Step>();
            RegexOptions options = RegexOptions.Multiline;
            foreach (Match m in Regex.Matches(input, PATTERN, options)) {
                char step = m.Groups[1].Value[0];
                char blocks = m.Groups[2].Value[0];

                Step st;
                if (steps.TryGetValue(step, out st)) {
                    st.BlockStep(blocks);
                    steps.Remove(step);
                    steps.Add(step, st);
                } else {
                    st = new Step() { id = step, };
                    st.BlockStep(blocks);
                    steps.Add(step, st);
                }
            }
        }

        public override string PartOne() {
            List<char> allSteps = steps.Keys.ToList();
            foreach (var c in steps.Values.SelectMany(s => s.blocks)) {
                allSteps.Remove(c);
            }
            allSteps.Sort();
            return "";
        }

        char CalculateStep(List<char> allSteps) {
            foreach (var c in steps.Values.SelectMany(s => s.blocks)) {
                allSteps.Remove(c);
            }
            allSteps.Sort();
            return '0';
        }

        public override string PartTwo() {
            return "";
        }

        struct Step {
            public char id;
            public List<char> blocks;

            public void BlockStep(char c) {
                if (blocks == null) {
                    blocks = new List<char>();
                }
                blocks.Add(c);
            }
        }
    }
}