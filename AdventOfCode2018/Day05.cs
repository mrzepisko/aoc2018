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
            int[] reactions = new int[input.Length];

            ProcessParams processParams = ProcessParams.Create(0, 1, true);
            int it = 0;
            do {
                do {
                    it++;
                    processParams = ProcessReactions(processParams.idxC0, processParams.idxC1, input, ref reactions);
                } while (processParams.doContinue);

            } while (reactions.Select(i => i == 0).Count() > 10);
            return CollectReaction(ref reactions);
        }

        private string CollectReaction(ref int[] reactions) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < reactions.Length; i++) {
                if (reactions[i] == 0) {
                    sb.Append(input[i]);
                }
            }
            return sb.ToString();
        }

        public override string PartTwo() {
            return "n/a";
        }

        ProcessParams ProcessReactions(int idxC0, int idxC1, string input, ref int[] reactions) {
            if (idxC1 < 0 || idxC1 >= input.Length) return ProcessParams.Create(0, 0, false); //end;
            bool compare = Compare(input[idxC0], input[idxC1]);
            if (compare) reactions[idxC0] = reactions[idxC1] = 1;

            idxC0 = NewC0(idxC0, compare, ref reactions);
            if (idxC0 < 0) return ProcessParams.Create(0, 0, false); //end
            idxC1 = NewC1(idxC0, ref reactions);
            if (idxC0 < 0) return ProcessParams.Create(0, 0, false); //end

            return ProcessParams.Create(idxC0, idxC1, true);
        }

        int Prev(int idxC0, ref int[] reactions, int dir = -1) {
            if (idxC0 + dir < 0) dir = 1;
            if (idxC0 + dir >= reactions.Length) return 0;
            if (reactions[idxC0 + dir] == 0) return dir;
            return Prev(idxC0 + dir + Math.Sign(dir), ref reactions, dir);
        }

        int NewC0(int idxC0, bool compare, ref int[] reactions) {
            if (compare) {
                int before = FindFirstBefore(idxC0 - 1, ref reactions);
                if (before >= 0) {
                    return before;
                }
            }
            int after = FindFirstAfter(idxC0 + 1, ref reactions);
            if (after > 0 || after < reactions.Length) {
                return after;
            }
            return -1;
        }

        int NewC1(int idxC0, ref int[] reactions) {
            return FindFirstAfter(idxC0 + 1, ref reactions);
        }

        int FindFirstBefore(int idx, ref int[] array) {
            if (idx < 0) return -1;
            if (array[idx] == 0) return idx;
            return FindFirstBefore(idx - 1, ref array);
        }

        int FindFirstAfter(int idx, ref int[] array) {
            if (idx >= array.Length) return -1;
            if (array[idx] == 0) return idx;
            return FindFirstAfter(idx + 1, ref array);
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

        struct ProcessParams {
            public int idxC0, idxC1, iteration;
            public bool doContinue;

            public static ProcessParams Create(int idxC0, int idxC1, bool doContinue) {
                return new ProcessParams() { idxC0 = idxC0, idxC1 = idxC1, doContinue = doContinue, iteration = 0, };
            }
        }
    }
}
