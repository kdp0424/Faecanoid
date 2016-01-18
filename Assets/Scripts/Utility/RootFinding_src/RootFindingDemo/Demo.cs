using System;

namespace DemoApplication
{
    class Demo
    {
        // Note that this function is increasing for x < 1 and decreasing for x > 1.
        static double f(double x)
        {
            return x*Math.Exp(-x);
        }

        // Derivative of f
        static double fprime(double x)
        {
            return (1.0 - x) * Math.Exp(-x);
        }

        static void Main(string[] args)
        {
            const double tolerance = 1e-7;
            double expected, computed, target;



            // Test root finder on range where f is increasing.
            // f(-1) = target, so -1 is the exact answer.
            target = -2.71828182845905;
            expected = -1.0;

            Console.WriteLine("Test root-finding on region where function is increasing.\n"); 

            computed = RootFinding.Bisect(new FunctionOfOneVariable(f), -4, 1, tolerance, target);
            PrintResults("Bisect", computed, expected);

            computed = RootFinding.Brent(new FunctionOfOneVariable(f), -4, 1, tolerance, target);
            PrintResults("Brent ", computed, expected);

            computed = RootFinding.Newton(new FunctionOfOneVariable(f), fprime, 0.0, tolerance, target);
            PrintResults("Newton", computed, expected);


            // Test root finder on range where f is decreasing.
            // f(5) = target, so 5 is the exact answer.
            target = 0.0336897349954273;
            expected = 5.0;

            Console.WriteLine("Test root-finding on region where function is increasing.\n");

            computed = RootFinding.Bisect(new FunctionOfOneVariable(f), 3, 7.1, tolerance, target);
            PrintResults("Bisect", computed, expected);

            computed = RootFinding.Brent(new FunctionOfOneVariable(f), 3, 7.1, tolerance, target);
            PrintResults("Brent ", computed, expected);

            computed = RootFinding.Newton(new FunctionOfOneVariable(f), fprime, 4.0, tolerance, target);
            PrintResults("Newton", computed, expected);


            // Demonstrate validation.
            Console.WriteLine("");
            try
            {
                // There is no solution in the interval since f(3) and f(7) are both positive.
                // This should raise an exception.
                //double x = RootFinding.Brent(new FunctionOfOneVariable(f), 3, 7, tolerance, 0.0);
                Console.WriteLine("WARNING: Missing validation logic. Should not have gotten here.");
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("Correctly threw an exception when given bad input.");
                Console.WriteLine(e.Message);
            }

            Console.WriteLine();

            // Demonstrate efficiency of each method.
            int iterationsUsed;
            double errorEstimate;

            // Solve f(x) = 0.2 to 10 decimal places
            RootFinding.Bisect(new FunctionOfOneVariable(f), 1.0, 5.0, 1e-10, 0.2, out iterationsUsed, out errorEstimate);
            PrintEfficiencyAndAccuracy("Bisect", iterationsUsed, errorEstimate);
            RootFinding.Brent(new FunctionOfOneVariable(f), 1.0, 5.0, 1e-10, 0.2, out iterationsUsed, out errorEstimate);
            PrintEfficiencyAndAccuracy("Brent ", iterationsUsed, errorEstimate);
            RootFinding.Newton(new FunctionOfOneVariable(f), fprime, 2.0, 1e-10, 0.2, out iterationsUsed, out errorEstimate);
            PrintEfficiencyAndAccuracy("Newton", iterationsUsed, errorEstimate);
        }

        static void PrintResults(string method, double computed, double expected)
        {
            Console.WriteLine("Method:   {0}\nexpected: {1}\ncomputed: {2}\nerror:    {3}\n", method, expected, computed, Math.Abs(expected - computed));
        }

        static void PrintEfficiencyAndAccuracy(string method, int iterations, double error)
        {
            Console.WriteLine("Method:   {0}\niterations: {1}\nerror:     {2}\n", method, iterations, error);
        }
    }
}