namespace TextMining.TextTools
{
    public class CosinusMetricComparator : IComparator
    {
        public double Compare(Vector x, Vector y)
        {
            double N = 0;

            foreach (string word in x.Items.Keys)
            {
                if (y.Items.ContainsKey(word))
                {
                    N += x.Items[word] * y.Items[word];
                }
            }

            double M = x.GetLength() * y.GetLength();

            if (M == 0)
            {
                return 0;
            }

            return N/M;
        }
    }
}
