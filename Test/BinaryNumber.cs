using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    /// <summary>
    /// Операции с числами в двоичной системе счисления.
    /// </summary>
    public static class BinaryNumber
    {
        /// <summary>
        /// Переводит целое число из двоичной системы счисления в десятичную.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int DecimalFromBinary(int[] number)
        {
            var d = 0;
            for (var i = 0; i < number.Count(); i++)
            {
                d += Convert.ToInt32(number[i]*Math.Pow(2, i));
            }
            return d;
        }


        /// <summary>
        /// Переводит целое число из десятичной системы счисления в двоичную c n разрядами.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int[] BinaryFromDecimal(int number, int n)
        {
            var res = new int[n];
            if (n == 1)
                res[0] = n % 2;
            else
              
            while (number > 0)
            {
                var p = MaxPow(number);
                    
                res[p] = 1;
                number -= Convert.ToInt32(Math.Pow(2, p));
            }
            return res;
        }

        /// <summary>
        /// Возвращает максимальную степень двойки, меньшую заданного числа.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static int MaxPow(int number)
        {
            var x = 1;
            var p = 0;
            while (number >= x*2)
            {
                p ++;
                x *= 2;
            }
            return p;
        }
    }
}
