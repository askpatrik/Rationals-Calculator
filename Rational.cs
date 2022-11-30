using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rationals
{
    public struct Rational
    {
        //Properties 
        public int Numerator;
        public int Denominator;

        //Constructor
        public Rational(int numerator, int denominator)
        {



            //Then return to program; new inputs

            Numerator = numerator;
            Denominator = denominator;
        }

        #region Aritmethic Operators 


        public static Rational operator *(Rational x, Rational y)
        {
            return new Rational(
                x.Numerator * y.Numerator,
                x.Denominator * y.Denominator
            );
        }
        public static Rational operator /(Rational x, Rational y)
        {
            return new Rational(
                x.Numerator * y.Denominator,
                x.Denominator * y.Numerator
            );
        }
        public static Rational operator +(Rational x, Rational y)
        {

            int newNumerator = (x.Numerator * y.Denominator) + (y.Numerator * x.Denominator);
            int newDenominator = x.Denominator * y.Denominator;

            return new Rational(
               newNumerator, newDenominator
           );
        }
        public static Rational operator -(Rational x, Rational y)
        {

            int newNumerator = (x.Numerator * y.Denominator) - (y.Numerator * x.Denominator);
            int newDenominator = x.Denominator * y.Denominator;

            return new Rational(
               newNumerator, newDenominator
           );
        }


        #region Comparison operators

        public static bool operator ==(Rational x, Rational y)
        {
            return x.Numerator * y.Denominator == x.Denominator * y.Numerator;
        }
        public static bool operator !=(Rational x, Rational y)
        {
            return x.Numerator * y.Denominator != x.Denominator * y.Numerator;
        }


        #endregion


        #endregion

        #region Parse()
        public static Rational Parse(string input)
        {
            Rational rational1, rational2;


            //If its only a rational number
            int counter = 0;
            bool containsMaxOneSlash = false;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '/')
                    counter += 1;
            }
            if (counter == 1 && !input.Contains(".") && !input.Contains('+') && !input.Contains('*') &&
            !input.Contains(':'))
            {
                var splittedString = input.Split('/');
                var intStringOne = int.Parse(splittedString[0]);
                var intStringTwo = int.Parse(splittedString[1]);
                return new Rational(intStringOne, intStringTwo);
            }

            //If its two numbers with *, : and +
            if (input.Contains('*') || input.Contains(':') || input.Contains('+'))
            {
                var strTrimmed = string.Join("", input.Split(' '));
                var strSplit = strTrimmed.Split('+', '*', ':');

                if (strSplit[0].Contains("."))
                    strSplit[0] = DecimalToFraction(strSplit[0]);
                if (strSplit[1].Contains("."))
                    strSplit[1] = DecimalToFraction(strSplit[1]);

                SplitToRational(strSplit[0], strSplit[1], out rational1, out rational2);

                if (input.Contains("*"))
                    return rational1 * rational2;
                else if (input.Contains("+"))
                    return rational1 + rational2;
                else if (input.Contains(":"))
                    return rational1 / rational2;
            }
            //If its minus oeprator


            input = input.Replace(" ", "");
            int indexOfSplit = -1;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '-')
                {
                    try
                    {
                        if (i == 0)
                            continue;
                        int.Parse(input[i - 1].ToString());
                        if (input[i + 1] == '-')
                        {
                            indexOfSplit = i;
                            break;
                        }
                        int.Parse(input[i + 1].ToString());
                        indexOfSplit = i;

                        break;

                    }
                    catch (Exception e) { }
                }

            }




            var splitString1 = input.Substring(0, indexOfSplit);
            var splitString2 = input.Substring(indexOfSplit + 1);

            if (splitString1.Contains("."))
                splitString1 = DecimalToFraction(splitString1);
            if (splitString2.Contains("."))
                splitString2 = DecimalToFraction(splitString2);

            SplitToRational(splitString1, splitString2, out rational1, out rational2);
            return rational1 - rational2;
        }


        // BEHÖVER EN FÖR "BARA" Rational numbers, förkorta
        private static void SplitToRational(string a, string b, out Rational rational1, out Rational rational2)
        {
            var leftSide = a.Split('/');
            var rightSide = b.Split('/');

            var numerator1 = int.Parse(leftSide[0]);
            var denominator1 = leftSide.Length == 2
            ? int.Parse(leftSide[1])
            : 1;
            var numerator2 = int.Parse(rightSide[0]);
            var denominator2 = rightSide.Length == 2
            ? int.Parse(rightSide[1])
            : 1;
            rational1 = new Rational(numerator1, denominator1);
            rational2 = new Rational(numerator2, denominator2);
        }
        #endregion
        #region ToString()
        public override string ToString()
        {
            if (Numerator == 0)
                return "0";

            if (Denominator == 1)
                return Numerator.ToString();
            if (Numerator == Denominator)
                return Numerator.ToString();

            return $"{Numerator}/{Denominator}";
        }
        #endregion
        #region Reduce()
        public static Rational ReduceFraction(Rational rational)
        {
            int a = rational.Numerator;
            int b = rational.Denominator;

            // Vi loopar tills b = 0 dvs remaindern är 0. 
            while (b != 0)
            {
                int Remainder = a % b;
                a = b;
                b = Remainder;

            }
            int GCD = a;




            if (a == 0)
            {
                Console.WriteLine("Cant divide shit: need to return here");

            }

            //If denominator is negative, move minus to Numerator! 







            int c = rational.Numerator / a;
            int d = rational.Denominator / a;

            string w = c.ToString();
            string q = d.ToString();

            if (q.Contains("-") && !w.Contains("-"))
            {
                string e = q.Substring(1);
                string f = $"-{w}";

                int t = int.Parse(f);
                int y = int.Parse(e);



                return new Rational(t, y);
            }





            var rat = new Rational(c, d);

            return rat;
            #endregion                          
        }


        #region DecimalToFraction

        public static string DecimalToFraction(string str)
        {
            // -0.5
            str = str.Replace('.', ',');
            double c = double.Parse(str);
            // Double -0,5

            double numerator = (c) * 1000;
            //-500
            double denominator = 1000;
            // -500/1000;


            string haha = "0.5";
            double kaka = double.Parse(haha);



            string newString = $"{(int)numerator}/{(int)denominator}";
            return newString;
        }
        public static string CheckForSyntaxErrors(string strin)
        {
            string Error = "Syntax Error";
            var str = strin.Replace(" ", "");


            if (strin == string.Empty)

                return Error;




            int errorCounter2 = 0;
            errorCounter2 = Regex.Matches(str, @"[*+:]").Count;
            int errorCounter3 = 0;
            errorCounter3 = Regex.Matches(str, @"[/.]").Count;
            string b = str[0].ToString();
            Regex regex = new Regex(@"[0-9-]");
            int errorCounter4 = 0;
            errorCounter4 = Regex.Matches(str, @"[-]").Count;
            int errorCounter5 = 0;
            errorCounter5 = Regex.Matches(str, @"[*+:]").Count;
            int errorCounter6 = 0;
            errorCounter6 = Regex.Matches(str, @"[/.]").Count;

            int errorCounter7 = 0;
            errorCounter7 = Regex.Matches(str, @"[*+:]").Count;




            int indexOfOperator = 0;
            int indexOfSlashDot1 = 0;
            int indexOfSlashDot2 = 0;
            int counter = 0;

            //Only valid inputs
            Regex regexBase = new Regex(@"[0-9-+*:/.]");
            if (!regexBase.IsMatch(str))
                return Error;
            else if (str.Contains('^'))
            {
                return Error;
            }

            else if (!char.IsDigit(str[str.Length - 1]))
            {
                return Error;
            }

            // Empty string
            else if (str == String.Empty)
                return Error;

            // Cant contain dot and slash. 0.5/2
            else if (errorCounter6 == 2 && errorCounter5 == 0 && !str.Contains('-'))
                return Error;

            // Max one standardoperator       
            else if (errorCounter2 > 1)
                return Error;

            // Max two of dot and slash           
            else if (errorCounter3 > 2)
                return Error;

            //First index - or int          
            else if (!regex.IsMatch(b))
                return Error;

            //Max 5 minuses           
            else if (errorCounter4 > 5)
                return Error;

            //Operators must have an int before and after
            else if ((errorCounter5 > 0 || errorCounter6 > 0) && str[0] != '-')
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '+' || str[i] == '*' || str[i] == ':'
                        || str[i] == '/' || str[i] == '.')
                    {

                        if (!char.IsDigit(str[i - 1]))
                            return Error;



                    }

                }

            }
            //'.' and '/' must be followed by +,*,: before written again // -5/9+9.5
            else if (errorCounter5 == 2 && errorCounter6 == 1)
            {
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '*' || str[j] == ':' || str[j] == '+')
                        indexOfOperator = j;
                    if (str[j] == '/' || str[j] == '.' && counter == 0)
                    {
                        indexOfSlashDot1 = j;
                        counter++;
                    }
                    if (str[j] == '/' && indexOfSlashDot1 != j)
                        indexOfSlashDot2 = j;
                }
                if (indexOfOperator! > indexOfSlashDot1 && indexOfOperator! < indexOfSlashDot2)
                {
                    return Error;
                }



            }





            return null;


        }

        public static string IntOrDoublePrint(string strin)
        {

            //isaninteger
            if (int.TryParse(strin, out int integer))
                return strin;

            //isadecimal
            string c = string.Empty;
            string z = string.Empty;
            var strDouble = strin.Replace('.', ',');
            if (double.TryParse(strDouble, out double dec))
            {
                c = dec.ToString();
                z = c.Replace(',', '.');
                return z;
            }
            return null;
        }



    }

}
#endregion