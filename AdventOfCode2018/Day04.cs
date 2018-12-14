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
        Dictionary<int, Guard> guards = new Dictionary<int, Guard>();
        public override void ParseInput(string input) {
            int lastGuard = -1;
            RegexOptions options = RegexOptions.Multiline;
            foreach (Match m in Regex.Matches(input, regex, options)) {
                DateTime time = DateTime.Parse(m.Groups["t"].Value);
                int guardId = m.Groups["b"].Success ? int.Parse(m.Groups["b"].Value) : lastGuard;
                TimedEvent.Type type = !m.Groups["b"].Success ? (m.Groups["w"].Success ? TimedEvent.Type.WakeUp : TimedEvent.Type.FallAsleep) : TimedEvent.Type.BeginShift;
                events.Add(new TimedEvent() { time = time, guardId = guardId, eventType = type, });
                lastGuard = guardId;
                if (!guards.ContainsKey(guardId)) {
                    guards.Add(guardId, new Guard() { id = guardId, });
                }
            }
            events.Sort((i1, i2) => i1.time.CompareTo(i2.time));
            CreateNapRaport();
        }

        void CreateNapRaport() {
            Guard lastGuard = null;
            DateTime fallingAsleep = default(DateTime);
            foreach (TimedEvent evt in events) {
                if (lastGuard == null) {
                    lastGuard = guards[evt.guardId];
                }
                if (TimedEvent.Type.FallAsleep.Equals(evt.eventType)) {
                    fallingAsleep = evt.time;
                } else if (TimedEvent.Type.WakeUp.Equals(evt.eventType)) {
                    lastGuard.naps.Add(new Nap(fallingAsleep, evt.time));
                }
            }
        }

        public override string PartOne() {
            Guard sleeper = guards.Values.Max();
            Dictionary<int, int> minutesAsleep = new Dictionary<int, int>();
            foreach (Nap nap in sleeper.naps) {
                for (int i = nap.start; i < nap.stop; i++) {
                    if (!minutesAsleep.ContainsKey(i)) {
                        minutesAsleep.Add(i, 1);
                    } else {
                        minutesAsleep[i]++;
                    }
                }
            }
            int favouriteMinute = minutesAsleep.OrderByDescending(kv => kv.Value).First().Key;
            return (favouriteMinute * sleeper.id).ToString();
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

        class Guard : IComparable<Guard> {
            public int id;
            public List<Nap> naps;

            public Guard() {
                naps = new List<Nap>();
            }

            public int TotalTimeSleeping { get { return naps.Sum(n => n.Time); } }

            public int CompareTo(Guard other) {
                return this.TotalTimeSleeping.CompareTo(other.TotalTimeSleeping);
            }
        }

        struct Nap {
            public int start, stop;
            public Nap(int start, int stop) {
                this.start = start;
                this.stop = stop;
            }

            public int Time { get { return stop - start; } }

            public Nap(DateTime start, DateTime stop) : this(start.Minute, stop.Minute - 1) {}
        }

    }
}
