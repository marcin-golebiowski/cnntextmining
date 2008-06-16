using System;
using System.Collections.Generic;
using System.Text;

using TextMining.Model;
using TextMining.TextTools;

namespace TextMining.Clastering
{
    class Dbscan
    {
        private readonly INewsComparator comparator;
        private readonly WordsStats stats;
        private readonly int maxLen;

        private enum State { visited, unvisited, noise };

        public Dbscan(INewsComparator comparator, WordsStats stats, int maxLen)
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
        public List<List<News>> Compute(List<News> news, double eps, int minPts)
        {
            var division = new List<List<News>>();
            var states = new State[news.Count];

            // Assign unvisited state to every news.
            for (int i = 0; i < states.Length; i++)
            {
                states[i] = State.unvisited;
            }

            // Process every unvisited news.
            for (int i = 0; i < news.Count; i++)
            {
                if (states[i] == State.unvisited)
                {
                    List<int> group = getGroup(i, news, states, eps, minPts);
                    // If not enough neighbours mark as noise.
                    if (group.Count == 0)
                    {
                        states[i] = State.noise;
                    }
                    else
                    {
                        List<News> linked = new List<News>();
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


        private List<int> getGroup(int current, List<News> news, State[] states, double eps, int minPts)
        {
            List<int> group = new List<int>();

            List<int> candidates = new List<int>();
            candidates.Add(current);

            while (candidates.Count > 0)
            {
                if (states[candidates[0]] == State.unvisited)
                {
                    List<int> neighbours = getNeighbours(candidates[0], news, states, eps);
                    if (neighbours.Count >= minPts)
                    {
                        group.Add(candidates[0]);
                        states[candidates[0]] = State.visited;
                        candidates.AddRange(neighbours);
                    }
                    else
                    {
                        states[candidates[0]] = State.noise;
                    }
                }
                candidates.RemoveAt(0);
            }
            return group;
        }

        private List<int> getNeighbours(int current, List<News> news, State[] states, double eps)
        {
            // Result - indices of news that are neighbours of current news.
            List<int> neighbours = new List<int>();

            // The current news vector.
            Vector center = new Vector(stats, news[current], maxLen);

            Vector toCompare;

            // Look through all possibly unvisited neighbours. Maybe can be done more effective.
            for (int i = 0; i < news.Count; i++ )
            {
                // Look only for unvisited states.
                if (states[i] != State.unvisited || i == current)
                    continue;

                toCompare = new Vector(stats, news[i], maxLen);
                // If the news is near enough. Add to neighbours.
                if (Math.Abs(comparator.Compare(center, toCompare)) <= eps)
                {
                    neighbours.Add(i);
                }
            }
            return neighbours;
        }

    }
}
