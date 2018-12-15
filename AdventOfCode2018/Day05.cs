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

        public override string PartOne() {
            string result = input, newResult = input;
            int steps = 0;
            while (Step(result, out result)) {
                steps++;
            }
            return string.Format("Results ready, calculated in {0} steps, total length: => {1} <=", steps, result.Length);
        }


        public override string PartTwo() {
            return "n/a";
        }

        bool Step(string input, out string output) {
            StringBuilder sb = new StringBuilder();
            bool reduced = false;
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
    }
}
