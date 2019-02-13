using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2018 {
    class Day08 : Day {
        const int STACK_SAFE_SIZE = int.MaxValue / 2;
        const string PATTERN = @"(\d+)";

        public override string Name => "--- Day 8: Memory Maneuver ---";

        public override string Input => InputResource.DAY08;

        int[] data;
        Node root;

        int calls;
        Thread evaluationThread;



        public override void ParseInput(string input) {
            var matchCollection = Regex.Matches(input, PATTERN);
            data = new int[matchCollection.Count];
            for (int i = 0; i < data.Length; i++) {
                data[i] = int.Parse(matchCollection[i].Groups[1].Value);
            }

            evaluationThread = new Thread(StartEvaluation, STACK_SAFE_SIZE);
            evaluationThread.Start();
        }

        private void StartEvaluation() {
            IEnumerator<int> enumerator = DataEnumerator();
            calls = 0;
            try {
                root = EvaluateChildRecursive(ref enumerator);
            } catch (StackOverflowException ex) {
                Console.WriteLine("Thread interrupted at {0} calls", calls);
                throw ex;
            }
        }

        private Node EvaluateChildRecursive(ref IEnumerator<int> enumerator) {
            //Console.WriteLine("{0}", calls++);
            if (!enumerator.MoveNext()) {
                return null;
            }
            Node node = new Node();
            node.childCount = enumerator.Current;
            enumerator.MoveNext();
            node.metaCount = enumerator.Current;

            //child nodes
            if (node.childCount > 0) {
                for (int i = 0; i < node.childCount; i++) {
                    var child = EvaluateChildRecursive(ref enumerator);
                    if (child != null) {
                        node.children.Add(child);
                    }
                }
            }
            //meta
            node.meta = new int[node.metaCount];
            for (int i = node.metaCount; i > 0; i--) {
                enumerator.MoveNext();
                node.meta[node.metaCount - i] = enumerator.Current;
            }
            
            return node;
        }

        public override string PartOne() {
            while (evaluationThread != null && evaluationThread.IsAlive) {
                Thread.Sleep(100);
            }
            return string.Format("{0}", root.SumMeta());
        }

        public override string PartTwo() {
            return string.Format("{0}", root.NodeValue());
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

            public int NodeValue() {
                int result = 0;
                if (children.Count == 0) {
                    result += meta.Sum();
                } else {
                    foreach (int reference in meta) {
                        if (reference > 0 && reference <= childCount) {
                            result += children[reference - 1].NodeValue();
                        }
                    }
                }

                return result;
            }
        }
    }
}
