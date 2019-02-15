using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2018 {
    class Day11 : Day {
        public override string Name => "--- Day 11: Chronal Charge ---";
        public override string Input => InputResource.DAY11;

        private int serialNo;

        private PowerGrid[,] powerLevels;
        
        public override void ParseInput(string input) {
            this.serialNo = int.Parse(input);
        }

        public override string PartOne() {
            powerLevels = new PowerGrid[98,98];
            PowerGrid result = new PowerGrid();
            for (int x = 1; x <= 98; x++) {
                for (int y = 1; y <= 98; y++) {
                    powerLevels[x - 1, y - 1] = new PowerGrid() {
                        x = x, y = y,
                        power = GetGridPowerLevel(x, y),
                    };
                }
            }

            return Enumerable.Cast<PowerGrid>(powerLevels).Max().ToString();
        }

        public override string PartTwo() {
            return "";
        }

        int GetGridPowerLevel(int x, int y) {
            int result = 0;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    result += GetPowerLevel(x + i, y + j);
                }
            }

            return result;
        }
        int GetPowerLevel(int x, int y) {
            var rackId = x + 10;
            var step1 = rackId * y;
            var step2 = step1 + serialNo;
            var step3 = step2 * rackId;
            var step4 = (step3%1000) / 100;
            return step4 - 5;
        }

        struct PowerGrid : IComparable<PowerGrid> {
            public int x, y, power;
            public override string ToString() {
                return $"PowerGrid[{x},{y}]: {power}";
            }

            public int CompareTo(PowerGrid other) {
                return power.CompareTo(other.power);
            }
        }
    }
}
