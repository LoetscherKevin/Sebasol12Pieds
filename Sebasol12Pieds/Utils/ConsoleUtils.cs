using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public static class ConsoleUtils
    {
        public static int ReadInt(int minValue, int maxValue)
        {
            int value;
            bool isValid;
            Console.Write("Choisir entre " + minValue + " et " + maxValue + " : ");
            do
            {
                string stringValue = Console.ReadLine();
                if (int.TryParse(stringValue, out value) && value >= minValue && value <= maxValue)
                {
                    isValid = true;
                }
                else
                {
                    Console.Write("Choisir un nombre entre " + minValue + " et " + maxValue + " : ");
                    isValid = false;
                }
            } while (!isValid);
            return value;
        }

        public static bool ReadYesNo(string demande)
        {
            bool isValid;
            bool exit = false;
            char c;
            do
            {
                Console.Write(demande);
                if (char.TryParse(Console.ReadLine(), out c) && (c == 'o' || c == 'n' || c == 'O' || c == 'N'))
                {
                    if (c == 'o' || c == 'O')
                        exit = true;
                    else
                        exit = false;

                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            } while (!isValid);

            return exit;
        }

        public static double ReadPositiveReal(string request)
        {
            double value;
            bool isValid;
            Console.Write(request);
            do
            {
                string stringValue = Console.ReadLine();
                if (double.TryParse(stringValue, out value) && value >= 0)
                {
                    isValid = true;
                }
                else
                {
                    Console.Write("Choisir une valeur positive : ");
                    isValid = false;
                }
            } while (!isValid);
            return value;
        }

        public static double ReadReal(string request)
        {
            double value;
            bool isValid;
            Console.Write(request);
            do
            {
                string stringValue = Console.ReadLine();
                if (double.TryParse(stringValue, out value))
                {
                    isValid = true;
                }
                else
                {
                    Console.Write("Choisir une valeur : ");
                    isValid = false;
                }
            } while (!isValid);
            return value;
        }
    }
}
