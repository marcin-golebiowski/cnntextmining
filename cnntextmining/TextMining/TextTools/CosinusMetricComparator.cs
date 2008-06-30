namespace TextMining.TextTools
{
    public class CosinusMetricComparator : IComparator
    {
        private readonly double val;

        public CosinusMetricComparator()
        {
            
        }

        public CosinusMetricComparator(double val)
        {
            this.val = val;
        }



        private int GetCommonLinks(Vector x, Vector y)
        {
            int result = 0;
            for (int i = 0; i < x.VectorNews.links.Count; i++)
            {
                for (int j = 0; j < y.VectorNews.links.Count; j++)
                {
                    if (x.VectorNews.links[i] == y.VectorNews.links[j])
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public double Compare(Vector x, Vector y)
        {
            double N = 0;

            foreach (string word in x.Items.Keys)
            {
                if (y.Items.ContainsKey(word))
                {
                    N += x.Items[word]*y.Items[word];
                }
            }

            double M = x.GetLength()*y.GetLength();

            if (M == 0)
            {
                return 0;
            }

            double result = N/M;

            if (val != 0)
            {
                int common = GetCommonLinks(x, y);
                if (common != 0)
                {
                    result *= val;
                }

            }

            return result;

        }
    }
}
