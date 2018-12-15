using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018 {
    class Day05 : Day {
        const int caseDiff = 'a' - 'A';
        public override string Name => "--- Day 5: Alchemical Reduction ---";

        public override string Input => InputResource.DAY05;

        public override void ParseInput(string input) {
            this.input = input;
        }

        string input;
        Result result;

        public override string PartOne() {
            string result = input, newResult = input;
            this.result = Reduce(result);
            return string.Format("Steps: {0}, total length: => {1} <=", this.result.steps, this.result.Length);
        }

        private Result Reduce(string input) {
            int steps = 0;
            string result = input;
            while (Step(result, out result)) {
                steps++;
            }
            return new Result() { output = result, steps = steps, };
        }

        public override string PartTwo() {
            string input = result.output;
            char[] unique = result.output.ToLower().Distinct().ToArray();
            int[] results = new int[unique.Length];
            var x = Parallel.For(0, unique.Length, i => {
                char c = unique[i];
                string newInput = input.Replace(c.ToString(), string.Empty).Replace(((char)((int)c - caseDiff)).ToString(), string.Empty);
                Result r = Reduce(newInput);
                results[i] = r.Length;
            });
            while (!x.IsCompleted) {}
            int shortestId = Array.IndexOf(results, results.Min());
            return string.Format("Unique units: {0}, shortest length: {1} for unit {2}", unique.Length, results[shortestId], unique[shortestId]);
        }

        bool Step(string input, out string output) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++) {
                if (Compare(i, i + 1, ref input)) {
                    sb.Append(input.Substring(i + 2));
                    output = sb.ToString();
                    return true;
                }
                sb.Append(input[i]);
            }
            output = sb.ToString();
            return false;
        }

        bool Compare(int idxC0, int idxC1, ref string input) {
            if (idxC0 < 0 || idxC1 < 0 || idxC0 >= input.Length || idxC1 >= input.Length) return false;
            else return Compare(input[idxC0], input[idxC1]);
        }
        bool Compare(char c0, char c1) {
            return EqLow(c0, c1) && !Eq(c0, c1);
        }

        bool Eq(char c0, char c1) {
            return c0 == c1;
        }

        bool EqLow(char c0, char c1) {
            return Max(c0, c1) - Min(c0, c1) - caseDiff == 0;
        }

        T Max<T>(T t0, T t1) where T : IComparable {
            return t0.CompareTo(t1) > 0 ? t0 : t1;
        }

        T Min<T>(T t0, T t1) where T : IComparable {
            return t0.CompareTo(t1) < 0 ? t0 : t1;
        }

        struct Result {
            public int steps;
            public string output;

            public int Length => output.Length;
        }
    }
}
