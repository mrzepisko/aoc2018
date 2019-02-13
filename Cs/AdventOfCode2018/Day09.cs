using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2018 {
    class Day09 : Day {
        const string PATTERN = @"(\d+).+?(\d+)";
        public override string Name => "--- Day 9: Marble Mania ---";

        public override string Input => InputResource.DAY09;


        Game game;

        public override void ParseInput(string input) {
            var m = Regex.Matches(input, PATTERN)[0];
            var playerCount = int.Parse(m.Groups[1].Value);
            var marbleCount = int.Parse(m.Groups[2].Value);
            game = new Game(playerCount, marbleCount);
        }

        public override string PartOne() {
            return string.Format("{0}", game.EvaluateGame().score);
        }

        public override string PartTwo() {
            return "n/a";
        }

        class Game {
            int playerCount, marbleCount, currentPlayer;
            int[] playerScore;
            MarblePlayground playground;



            public Game(int playerCount, int marbleCount) {
                this.playerCount = playerCount;
                this.marbleCount = marbleCount;
                playerScore = new int[playerCount];
                playground = new MarblePlayground();
            }

            public PlayerScore EvaluateGame() {
                playground.Add(new Marble(0));
                int currentPlayer = 0;
                for (int currentMarble = 1; currentMarble <= marbleCount; currentMarble++) {
                    playerScore[currentPlayer] += PlaceMarble(currentMarble);
                    currentPlayer = NextPlayer(currentPlayer);
                }
                var max = playerScore.Max();
                var maxId = Array.IndexOf(playerScore, max);
                return new PlayerScore() {
                    playerId = maxId, score = max,
                };
            }

            int NextPlayer(int currentPlayer) {
                return (currentPlayer + 1) % playerCount;
            }

            int PlaceMarble(int currentMarble) {
                return playground.Add(new Marble(currentMarble));
            }

            class MarblePlayground {
                const int INSERT_AFTER = 1;
                const int JOKER_REMOVES = 7;
                int activeMarbleIndex;
                List<Marble> marbles = new List<Marble>();

                /// <summary>
                /// Adds marble to playground
                /// </summary>
                /// <param name="marble">marble to add</param>
                /// <returns>points</returns>
                public int Add(Marble marble) {
                    int score = 0;
                    if (marble.IsJoker) {
                        score += marble.Score;
                        score += Remove(JOKER_REMOVES, Dir.CCW);
                        SetActive(JOKER_REMOVES - 1, Dir.CCW); //7th was removed
                    } else {
                        marbles.Insert(GetIndex(activeMarbleIndex, INSERT_AFTER), marble);
                        SetActive(marble);
                    }
                    return score;
                }

                int GetIndex(int from, int increment, Dir direction = Dir.CW) {
                    if (marbles.Count == 0) return 0;
                    var clampedIncrement = increment % marbles.Count;
                    return (marbles.Count + (from + (Dir.CW.Equals(direction) ? 1 : -1) * clampedIncrement) + 1) % marbles.Count;
                }

                int Remove(int step, Dir direction) {
                    var index = GetIndex(activeMarbleIndex, step, direction);
                    var value = marbles[index].Score;
                    marbles.RemoveAt(index);
                    return value;
                }

                void SetActive(int step, Dir direction) {
                    activeMarbleIndex = GetIndex(activeMarbleIndex, step, direction);
                }

                void SetActive(Marble marble) {
                    activeMarbleIndex = marbles.IndexOf(marble);
                }

                enum Dir {
                    CW,
                    CCW,
                }
            }

            struct Marble {
                const int JOKER_MULTIPLY_OF = 23;
                int id;

                public Marble(int id) {
                    this.id = id;
                }

                public int Score => id;
                public bool IsJoker => id > 0 && (id % JOKER_MULTIPLY_OF) == 0;

                public override bool Equals(object obj) {
                    if (!(obj is Marble)) {
                        return false;
                    }

                    var marble = (Marble)obj;
                    return id == marble.id;
                }

                public override int GetHashCode() {
                    return 1877310944 + id.GetHashCode();
                }

                public override string ToString() {
                    return string.Format("Marble#{0}", id);
                }


            }
        }

        struct PlayerScore {
            public int playerId, score;
        }
    }
}
