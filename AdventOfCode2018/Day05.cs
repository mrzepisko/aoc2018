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

        public override void ParseInput() { }

        int[] reactions;

        public override string PartOne() {
            reactions = new int[Input.Length];
            ProcessReactions(0, 1, Input, ref reactions);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < reactions.Length; i++) {
                if (reactions[i] == 0) {
                    sb.Append(Input[i]);
                }
            }
            return sb.ToString();
        }

        public override string PartTwo() {
            return "n/a";
        }

        void ProcessReactions(int idxC0, int idxC1, string input, ref int[] reactions) {
            if (idxC1 > input.Length) return;
            bool compare = Compare(input[idxC0], input[idxC1]);
            if (compare) reactions[idxC0] = reactions[idxC1] = 1;

            int dC0 = DeltaC0(compare, idxC0, ref reactions);
            if (dC0 == 0) return; //end
            int dC1 = DeltaC1(compare, dC0);

            ProcessReactions(idxC0 + dC0, idxC1 + dC1, input, ref reactions);
        }

        int Prev(int idxC0, ref int[] reactions, int dir = -1) {
            if (idxC0 + dir < 0) dir = 1;
            if (idxC0 + dir >= reactions.Length) return 0;
            if (reactions[idxC0 + dir] == 0) return dir;
            return Prev(idxC0 + dir + Math.Sign(dir), ref reactions, dir);
        }

        int DeltaC0(bool compareResult, int idxC0, ref int[] reactions) {
            return !compareResult ? 1 : Prev(idxC0, ref reactions);
        }

        int DeltaC1(bool compareResult, int DeltaC0) {
            return !compareResult ? 1 : (DeltaC0 > 0 ? 2 : 1);
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
