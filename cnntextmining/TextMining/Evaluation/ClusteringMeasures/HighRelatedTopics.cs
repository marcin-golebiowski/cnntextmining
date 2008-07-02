using System;
using System.Collections.Generic;
using System.Text;
using TextMining.DataLoading;

namespace TextMining.Evaluation.ClusteringMeasures
{
    class HighRelatedTopics
    {
        private List<Group> clustering;
        private double[,] distances;
        private List<Pair> relatedTopics;
        private List<string> topicsInDB;

        public HighRelatedTopics(List<Group> clustering)
        {
            this.clustering = clustering;
            topicsInDB = DataStore.Instance.GetTopics();
            computeDistances();
            computeRelatedPairs();
        }

        // Tablica 2-elementowa, 2 topici blisko zwiazane ze soba
        public List<string[]> getHighRelatedTopics(uint count)
        {
            List<string[]> result = new List<string[]>();

            for (int i = 0; i < count; i++)
            {
                result.Add(new string[] { relatedTopics[i].topic1, relatedTopics[i].topic2 });
                Console.WriteLine(relatedTopics[i].topic1 + "\n" + relatedTopics[i].topic2 + "\ndist = " + relatedTopics[i].distance + "\n");
            }

            return result;
        }

        public List<string[]> getHighRelatedTopics()
        {
            return getHighRelatedTopics((uint)relatedTopics.Count);
        }



        private void computeDistances()
        {
            distances = new double[topicsInDB.Count, topicsInDB.Count];

            for (int i = 0; i < topicsInDB.Count; i++)
            {
                string topicI = topicsInDB[i];

                for (int j = 0; j < i; j++)
                {
                    string topicJ = topicsInDB[j];

                    foreach (Group g in clustering)
                    {
                        int s1 = Util.topicCountInGroup(topicI, g);
                        int s2 = Util.topicCountInGroup(topicJ, g);
                        distances[i, j] += Math.Min(s1, s2);
                    }
                }
            }
        }

        private void computeRelatedPairs()
        {
            relatedTopics = new List<Pair>();

            for (int i = 0; i < topicsInDB.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    Pair p = new Pair();
                    p.topic1 = topicsInDB[i];
                    p.topic2 = topicsInDB[j];
                    p.distance = distances[i,j];
                    if(p.distance != 0)
                        relatedTopics.Add(p);
                }
            }
            relatedTopics.Sort();
        }


        private class Pair : IComparable
        {
            public string topic1;
            public string topic2;

            public double distance;

            #region IComparable Members

            public int CompareTo(object obj)
            {
                return (int)( ((Pair)obj).distance - distance);
            }

            #endregion
        }

    }
}
