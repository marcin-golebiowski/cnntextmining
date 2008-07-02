using System;
using System.Collections.Generic;
using System.Text;
using TextMining.Evaluation;
using TextMining.Model;
using TextMining.TextTools;

namespace TextMining.Clastering
{
    class Dbscan
    {
        private readonly IComparator comparator;
        private readonly WordsStats stats;
        private readonly int maxLen;
        private Vector[] newsVectors;

        private enum State { visited, unvisited, noise };

        public Dbscan(IComparator comparator, WordsStats stats, int maxLen)
        {
            this.comparator = comparator;
            this.stats = stats;
            this.maxLen = maxLen;
        }

        /// <summary>
        /// Dbscan clustering algorithm. (http://en.wikipedia.org/wiki/DBSCAN)
        /// </summary>
        /// <param name="news"></param>
        /// <param name="eps">minimal distance between news in same neighbourhood</param>
        /// <param name="minPts">minimal number of neighbour news, for news in that cluster</param>
        /// <returns></returns>
        public List<Group> Compute(Group news, double eps, int minPts)
        {
            var division = new List<Group>();
            var states = new State[news.Count];
            newsVectors = new Vector[news.Count];

            // Build the vectors and
            // Assign unvisited state to every news.
            for (int i = 0; i < states.Length; i++)
            {
                states[i] = State.unvisited;
                newsVectors[i] = new Vector(stats, news[i], maxLen);
                newsVectors[i].BuildVector();
            }

            // Process every unvisited news.
            for (int i = 0; i < news.Count; i++)
            {
                if (states[i] == State.unvisited)
                {
                    List<int> group = getGroup(i, news, states, eps, minPts);
                    //Console.WriteLine("g " + i + " count " + group.Count); 
                    // If not enough neighbours mark as noise.
                    if (group.Count == 0)
                    {
                        states[i] = State.noise;
                    }
                    else
                    {
                        Group linked = new Group("");
                        foreach (int linkedIndex in group)
                        {
                            linked.Add(news[linkedIndex]);
                            states[linkedIndex] = State.visited;
                        }

                        // Add current group to result.
                        division.Add(linked);
                    }
                }
            }

            return division;
        }


        private List<int> getGroup(int current, Group news, State[] states, double eps, int minPts)
        {
            // Result group.
            List<int> group = new List<int>();
            // Candidates for beeing in group.
            List<int> candidates = new List<int>();
            List<int> visited = new List<int>();         

            candidates.Add(current);
            visited.Add(current);

            while (candidates.Count > 0)
            {
                //Console.WriteLine(candidates.Count);
                if (states[candidates[0]] == State.unvisited)
                {
                    List<int> neighbours = getNeighbours(candidates[0], news, states, eps);
                    if (neighbours.Count >= minPts)
                    {
                        group.Add(candidates[0]);
                        foreach (int newCandid in neighbours)
                        {
                            if (!visited.Contains(newCandid))
                            {
                                candidates.Add(newCandid);
                                visited.Add(newCandid);
                            }
                        }
                    }
                }
                candidates.RemoveAt(0);
            }
            return group;
        }

        private List<int> getNeighbours(int current, Group news, State[] states, double eps)
        {
            // Result - indices of news that are neighbours of current news.
            List<int> neighbours = new List<int>();

            // Look through all possibly unvisited neighbours. Maybe can be done more effective.
            for (int i = 0; i < news.Count; i++ )
            {
                // Look only for unvisited states.
                if (i == current || states[i] == State.noise)
                    continue;

                // If the news is near enough. Add to neighbours.
                if (Math.Abs(comparator.Compare(newsVectors[current], newsVectors[i])) >= eps)
                {
                    neighbours.Add(i);
                }
            }

            return neighbours;
        }

    }
}
