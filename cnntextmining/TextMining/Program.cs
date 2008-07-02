using System;
using TextMining.Evaluation.Experiments;

namespace TextMining
{
    class Program
    {

        static void Main(string[] args)
        {
         
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: TextMining.exe  <numer of topics> <number of kmeans iteration> <numer of releated topic to print> ");
                return;
            }

            int a, b, c;

            if (Int32.TryParse(args[0], out a))
            {
                Console.WriteLine("First argument is not a number");
                return;
            }

            if (Int32.TryParse(args[1], out b))
            {
                Console.WriteLine("Second argument is not a number");
                return;
            }

             if (Int32.TryParse(args[2], out c))
            {
                Console.WriteLine("Third argument is not a number");
                return;
            }

            var expr = new FinalExperiment(a, b, c);
            expr.Run();
        }
    }
}

