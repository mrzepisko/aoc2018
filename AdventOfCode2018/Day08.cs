using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2018 {
    class Day08 : Day {
        const string PATTERN = @"(\d+)";

        public override string Name => "--- Day 8: Memory Maneuver ---";

        public override string Input => InputResource.DAY08;

        int[] data;
        Node root;

        

        public override void ParseInput(string input) {
            var matchCollection = Regex.Matches(input, PATTERN);
            data = new int[matchCollection.Count];
            for (int i = 0; i < data.Length; i++) {
                data[i] = int.Parse(matchCollection[i].Groups[1].Value);
            }


            IEnumerator<int> enumerator = DataEnumerator();
            enumerator.MoveNext();
            root = EvaluateChild(ref enumerator);
        }

        private Node EvaluateChild(ref IEnumerator<int> enumerator) {
            Node node = new Node();
            node.childCount = enumerator.Current;
            enumerator.MoveNext();
            node.metaCount = enumerator.Current;
            if (node.childCount > 0) {
                for (int i = 0; i < node.childCount; i++) {
                    enumerator.MoveNext();
                    var child = EvaluateChild(ref enumerator);
                    node.children.Add(child);
                }
            } else {
                node.meta = new int[node.metaCount];
                for (int i = node.metaCount; i > 0; i--) {
                    enumerator.MoveNext();
                    node.meta[node.metaCount - i] = enumerator.Current;
                }
            }
            return node;
        }

        public override string PartOne() {
            return string.Format("{0}", root.SumMeta());
        }

        public override string PartTwo() {
            throw new NotImplementedException();
        }

        IEnumerator<int> DataEnumerator() {
            for (int i = 0; i < data.Length; i++) {
                yield return data[i];
            }
        }

        class Node {
            public int id, childCount, metaCount;
            public List<Node> children = new List<Node>();
            public int[] meta;

            public Node() {

            }

            public Node(int id) {
                this.id = id;
            }

            public int SumMeta() {
                int result = 0;
                if (meta != null) {
                    result += meta.Sum();
                }
                foreach (Node child in children) {
                    result += child.SumMeta();
                }
                return result;
            }
        }
    }
}
