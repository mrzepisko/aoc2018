using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2018 {
    class Day04 : Day {
        const string regex = @"(?>\[(?'t'\d+-\d+-\d+ \d+:\d+)\]).+(?>#(?'b'\d+)|(?'f'falls)|(?'w'wakes))";

        public override string Name => "--- Day 4: Repose Record ---";

        public override string Input => InputResource.DAY04;
        List<TimedEvent> events = new List<TimedEvent>();

        public override void ParseInput() {
            int lastGuard = -1;
            RegexOptions options = RegexOptions.Multiline;
            foreach (Match m in Regex.Matches(Input, regex, options)) {
                DateTime time = DateTime.Parse(m.Groups["t"].Value);
                int guardId = m.Groups["b"].Success ? int.Parse(m.Groups["b"].Value) : lastGuard;
                TimedEvent.Type type = !m.Groups["b"].Success ? (m.Groups["w"].Success ? TimedEvent.Type.WakeUp : TimedEvent.Type.FallAsleep) : TimedEvent.Type.BeginShift;
                events.Add(new TimedEvent() { time = time, guardId = guardId, eventType = type, });
                lastGuard = guardId;
            }
            events.Sort((i1, i2) => i1.time.CompareTo(i2.time));
        }

        public override string PartOne() {
            return "n/a";
        }

        public override string PartTwo() {
            return "n/a";
        }

        struct TimedEvent {
            public DateTime time;
            public int guardId;
            public Type eventType;

            public enum Type {
                BeginShift,
                WakeUp,
                FallAsleep,
            }
        }
    }
}
