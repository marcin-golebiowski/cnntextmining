using System;
using System.Collections.Generic;
using System.Text;

namespace TextMining.TextTools
{
    class JaccardMetricCompartator : IComparator
    {
        public double Compare(Vector x, Vector y)
        {
            double result = 0;

            int xIy = 0, xSy = 0; //  x /\ y  and x \/ y (sum)

            foreach(string word in x.Items.Keys)
            {
                if (y.Items.ContainsKey(word))
                {
                    xIy++;
                }
                else
                {
                    xSy++;
                }
            }

            xSy += xIy;

            if (xSy == 0)
                return xIy;

            result = (double)xIy / (double)xSy;

            return result;
        }


    }
}
