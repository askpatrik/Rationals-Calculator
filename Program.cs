namespace Rationals
{
    internal class Program
    {
        static void Main(string[] args)
        {


            string input = String.Empty;
            string lowerInput = String.Empty;





            while (true)
            {
                input = Console.ReadLine();
                lowerInput = input.ToLower();

                if (lowerInput == "quit")
                    break;

                string syntax = Rational.CheckForSyntaxErrors(lowerInput);
                if (syntax == "Syntax Error")
                {
                    Console.WriteLine("Syntax Error");
                    continue;
                }

                string intOrDouble = Rational.IntOrDoublePrint(lowerInput);
                if (intOrDouble != null)
                {
                    Console.WriteLine(intOrDouble);
                    continue;
                }



                var a = Rational.Parse(input);
                var b = Rational.ReduceFraction(a);

                if (b.Denominator == 0)
                {
                    Console.WriteLine("Division by zero is undefined");
                    continue;
                }
                Console.WriteLine(b);
            }
            Environment.Exit(0);



        }
    }
}