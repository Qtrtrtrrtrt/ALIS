using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace Test
{
    /// <summary>
    /// Конъюнктивная и Дизъюнктивная Нормальные Формы
    /// </summary>
    public class NormalForm
    {
        private string _variables = "xyztq";
        public BooleanFunction Function { get; set; }

        public NormalForm(BooleanFunction f)
        {
            Function = f;
        }


        /// <summary>
        /// Возвращает конъюнкт по номеру бинарного набора, отрицание по not
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string BuildConjunct(int i, int not)
        {
            var bset = BinaryNumber.BinaryFromDecimal(i, Function.NumberOfArguments);
            var conj = "(";
            for (var j = 0; j < Function.NumberOfArguments; j++)
            {

                if (conj != "(")
                    conj += "&";
                if (bset[Function.NumberOfArguments - j - 1] == not)
                    conj += "¬" + _variables[j];
                else
                    conj += _variables[j];
            }
            return conj+")";
        }


        /// <summary>
        /// Возвращает дизъюнкт по номеру бинарного набора, отрицание по not
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string BuildDisjunct(int i, int not)
        {
            var disj = "(";
            var bset = BinaryNumber.BinaryFromDecimal(i, Function.NumberOfArguments);
            for (var j = 0; j < Function.NumberOfArguments; j++)
            {
                if (disj != "(")
                    disj += "⋁";
                if (bset[Function.NumberOfArguments - j - 1] ==not)
                    disj += "¬" + _variables[j];
                else
                    disj += _variables[j];
            }
            disj += ")";
            return disj;
        }

        /// <summary>
        /// Возвращает строку, содержащую правильную СДНФ
        /// </summary>
        /// <returns></returns>
        public string PDNF()
        {
            var res = "";
            var f = BinaryNumber.BinaryFromDecimal(Function.Function, Function.NumberOfBytes);
            for (var i = 0; i < Function.NumberOfBytes; i++)
            {
                if (f[i] == 1)
                {
                    if (res != "")
                        res += "⋁";
                    res += BuildConjunct(i, 0);
                }
            }
            return res;
        }

        /// <summary>
        /// Возвращает неправильную СДНФ 
        ///  (0 - СДНФ составлена по нулевым наборам функции; 
        /// 1 - СДНФ составлена по единичным наборам, но отрицание в конъюнктах по 1; 
        /// 2 - СДНФ составлена по нулевым наборам и отрицание в конъюнктах по 0)
        /// </summary>
        /// <returns></returns>
        public string WrongPDNF(int var)
        {
            var res = "";
            var f = BinaryNumber.BinaryFromDecimal(Function.Function, Function.NumberOfBytes);
            
            var set = 0;
            var not = 1;
            switch (var)
            {
                case 0:
                    not = 0;
                    break;

                case 1:
                    set = 1;
                    break;

            }
            
            for (var i = 0; i < Function.NumberOfBytes; i++)
            {
                if (f[i] == set)
                {
                    if (res != "")
                        res += "⋁";
                    res += BuildConjunct(i,not);
                }
            }
            return res;
        }

        /// <summary>
        /// Возвращает строку, содержащую выражение для СКНФ
        /// </summary>
        /// <returns></returns>
        public string PCNF()
        {
            var res = "";
            var f = BinaryNumber.BinaryFromDecimal(Function.Function, Function.NumberOfBytes);
            for (var i = 0; i < Function.NumberOfBytes; i++)
            {
                if (f[i] == 0)
                {
                   
                    if (res != "")
                        res += "&";
                    res += BuildDisjunct(i, 1);
                }
            }
            return res;
        }

        /// <summary>
        /// Возвращает неправильную СКНФ 
        ///  (0 - СКНФ составлена по нулевым наборам функции, отрицание в дизъюнктах по 0; 
        /// 1 - СДКНФ составлена по единичным наборам, но отрицание в дизъюнктах по 0; 
        /// 2 - СКНФ составлена по единичным наборам и отрицание в дизъюнктах по 1)
        /// </summary>
        /// <returns></returns>
        public string WrongPCNF(int var)
        {
            var res = "";
            var f = BinaryNumber.BinaryFromDecimal(Function.Function, Function.NumberOfBytes);

            var set = 1;
            var not = 0;
            switch (var)
            {
                case 0:
                    set = 0;
                    break;

                case 2:
                    not = 1;
                    break;

            }

            for (var i = 0; i < Function.NumberOfBytes; i++)
            {
                if (f[i] == set)
                {

                    if (res != "")
                        res += "&";
                    res += BuildDisjunct(i, not);
                }
            }
            return res;
        }



    }
}
