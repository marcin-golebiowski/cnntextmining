using System;
using TextMining.Evaluation;
using TextMining.Evaluation.Experiments;

namespace TextMining
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("CNN TextMining v.1.0.0 - Authors: Marcin Go��biowski, Marek Chru�ciel");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Usage: TextMining.exe (<numer of topics>|<list of topics>) <kmeans iteration> <numer of releated> <metric>");
                Console.WriteLine("   <numer of topics>:  positive integer");
                Console.WriteLine("   <number of kmeans iteration>:  positive integer");
                Console.WriteLine("   <numer of releated>: positeve integer");
                Console.WriteLine("   <metric>:  1 - Cosinus, 2 - Eucklides, 3 - Jaccard ");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Examples: TextMining.exe 10 5 2 1 1");
                Console.WriteLine("          TextMining.exe \"http://topics.edition.cnn.com/topics/software;http://topics.edition.cnn.com/topics/vacations\" 5 2 1");
                return;
            }

            uint a, b, c;
            ushort d;

            if (!UInt32.TryParse(args[1], out b))
            {
                Console.WriteLine("Second argument is not a positive number");
                return;
            }

            if (!UInt32.TryParse(args[2], out c))
            {
                Console.WriteLine("Third argument is not a positive number");
                return;
            }

            if (!ushort.TryParse(args[3], out d))
            {
                Console.WriteLine("Third argument is not 1, 2 or 3");
                return;
            }


            IExperiment exp;

            if (!UInt32.TryParse(args[0], out a))
            {
                exp = new FinalExperiment(args[0].Split(';'), b, c, d);
            }
            else
            {
                exp = new FinalExperiment(a, b, c, d);
            }

            exp.Run();
        }
    }
}

