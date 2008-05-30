using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab.Utility
{
    public static class NumberToWords
    {
        public static string Convert(int number)
        {
            string result = "";
            if (number == 0)
            {
                return "zero";
            }
            if (number >= 100)
            {
                result += GetLessThan100(number / 100);
                result += " hundred ";
                number %= 100;
            }
            result += GetLessThan100(number);
            return result;
        }

        private static string GetLessThan100(int number)
        {
            System.Diagnostics.Debug.Assert(number < 100);

            string result = "";

            if (number >= 10 && number <= 19)
            {
                switch (number)
                {
                    case 10:
                        return "ten";
                    case 11:
                        return "eleven";
                    case 12:
                        return "twelve";
                    case 13:
                        return "thirteen";
                    case 14:
                        return "fourteen";
                    case 15:
                        return "fifteen";
                    case 16:
                        return "sixteen";
                    case 17:
                        return "seventeen";
                    case 18:
                        return "eighteen";
                    case 19:
                        return "nineteen";
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }
            }
            else
            {
                if (number >= 20)
                {
                    switch (number / 10)
                    {
                        case 2:
                            result += "twenty ";
                            break;
                        case 3:
                            result += "thirty ";
                            break;
                        case 4:
                            result += "fourty ";
                            break;
                        case 5:
                            result += "fifty ";
                            break;
                        case 6:
                            result += "sixty ";
                            break;
                        case 7:
                            result += "seventy ";
                            break;
                        case 8:
                            result += "eighty ";
                            break;
                        case 9:
                            result += "ninety ";
                            break;
                    }

                    number %= 10;
                }

                switch (number)
                {
                    case 0:
                        break;
                    case 1:
                        result += "one";
                        break;
                    case 2:
                        result += "two";
                        break;
                    case 3:
                        result += "three";
                        break;
                    case 4:
                        result += "four";
                        break;
                    case 5:
                        result += "five";
                        break;
                    case 6:
                        result += "six";
                        break;
                    case 7:
                        result += "seven";
                        break;
                    case 8:
                        result += "eight";
                        break;
                    case 9:
                        result += "nine";
                        break;
                }
            }
            return result;
        }
    }
}
