using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Test
{

    public enum FunctionClass
    {
        Save0,
        Save1,
        SelfDual,
        Monotone,
        Linear
    }
    public class BooleanFunction
    {
        public int NumberOfArguments { get; set; } //
        public int Function { get; set; }

        public int NumberOfBytes { get; set; }

        private Random _rnd = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// 0, 1, S, M, L
        /// </summary>
        private int[] Properties = new int[5];

        public BooleanFunction(int[] func)
        {
          
            Function = BinaryNumber.DecimalFromBinary(func);
            NumberOfBytes = func.Length;
            var n = 1;
            while (Math.Pow(2, n) != func.Length)
            {
                n++;
            }
            NumberOfArguments = n;
        }
        public BooleanFunction(int func, int nargs)
        {
          
            Function = func;
            NumberOfArguments = nargs;
            NumberOfBytes = Convert.ToInt32(Math.Pow(2, nargs));
            
        }
        public BooleanFunction getCopy()
        {
            BooleanFunction newBF = new BooleanFunction(this.Function, this.NumberOfArguments);
            newBF.Properties = this.Properties;
            return newBF;
        }

        public int[] FindProperties()
        {
            Properties[0] = Convert.ToInt32(IsSaving0());
            Properties[1] = Convert.ToInt32(IsSaving1());
            Properties[2] = Convert.ToInt32(IsSelfDual());
            Properties[3] = Convert.ToInt32(IsMonotone());
            Properties[4] = Convert.ToInt32(IsLinear());
            if (Properties[4] == 1)
                Properties[4] = 1;
            return Properties;
        }
        public BooleanFunction( int nargs)
        {
           
            Function = _rnd.Next(0, Convert.ToInt32(Math.Pow(2, Math.Pow(2, nargs))));
            
            NumberOfArguments = nargs;
            NumberOfBytes = Convert.ToInt32(Math.Pow(2, nargs));
        }

        
        /// <summary>
        /// Строит функцию с заданным свойством ( 0, 1, S, M, L)
        /// </summary>
        /// <param name="properties"></param>
        public BooleanFunction(FunctionClass functionClass, int nargs)
        {
            NumberOfArguments = nargs;
            NumberOfBytes = Convert.ToInt32(Math.Pow(2, nargs));
           
            switch (functionClass)
            {
                case FunctionClass.Linear:
                    var zh = new ZhegalkinPolynomial(nargs, true);
                    Function = zh.Function.Function;
                    break;
                case FunctionClass.SelfDual:
                    var y = BinaryNumber.BinaryFromDecimal(_rnd.Next(Convert.ToInt32(Math.Pow(2, NumberOfBytes/2))),NumberOfBytes/2);
                    var f = new int[NumberOfBytes];
                    for (var i = 0; i < y.Length; i++)
                    {
                        f[i] = y[i];
                        f[NumberOfBytes - i - 1] = Math.Abs(y[i] - 1);
                    }
                    Function = BinaryNumber.DecimalFromBinary(f);
                    break;
                case FunctionClass.Monotone:
                    Function = BuildMonotoneFunction(0, 0, new int[NumberOfBytes]);
                    break;
                case FunctionClass.Save0:
                    var y2 = BinaryNumber.BinaryFromDecimal(_rnd.Next(Convert.ToInt32(Math.Pow(2, NumberOfBytes))), NumberOfBytes);
                    y2[0] = 0;
                    Function = BinaryNumber.DecimalFromBinary(y2);
                    break;
                case FunctionClass.Save1:
                    var y3 = BinaryNumber.BinaryFromDecimal(_rnd.Next(Convert.ToInt32(Math.Pow(2, NumberOfBytes))), NumberOfBytes);
                    y3[NumberOfBytes-1] = 1;
                    Function = BinaryNumber.DecimalFromBinary(y3);
                    break;

            }

        }

        
        private int BuildMonotoneFunction(int set, int value, int[]func)
        {
            var s = BinaryNumber.BinaryFromDecimal(set, NumberOfArguments);
            
            func[set] = value;
            for (var i = 0; i < NumberOfArguments; i++)
            {
                if (s[i] == 0)
                {
                    var newvalue = value;
                    if (value==0)
                        newvalue = _rnd.Next(100)%2;

                    var news = BinaryNumber.BinaryFromDecimal(set, NumberOfArguments);
                    news[i] = 1;
                    var newset = BinaryNumber.DecimalFromBinary(news);
                    BuildMonotoneFunction(newset, newvalue, func);
                }
            }

            return BinaryNumber.DecimalFromBinary(func);
        }

        /// <summary>
        /// Возвращает строку - вектор функции
        /// </summary>
        /// <returns></returns>
        public string VectorString
        {
            get
            {
                var res = "(";
                var f = BinaryNumber.BinaryFromDecimal(Function, NumberOfBytes);
                for (var i = 0; i < NumberOfBytes; i++)
                    res += f[i].ToString();
                res += ")";
                return res;
            }
            
        }

        public int[] Vector
        {
            get { return BinaryNumber.BinaryFromDecimal(Function, NumberOfBytes); }
        }


        /// <summary>
        /// Возвращает строку - вектор свойств (0,1,S,M,L)
        /// </summary>
        /// <returns></returns>
        public string PropertiesVector
        {
            get
            {
                FindProperties();
                var res = "(";
                for (var i = 0; i < 5; i++)
                    res += Properties[i];
                res +=")";

                return res;
            }

        }

        public string PropertiesString(int[] prop)
        {
            var str = "";
            if (prop[0]==1)
                str = "Сохраняет константу 0, ";
            else
                str = "Не сохраняет константу 0, ";
            if (prop[1]==1)
                str += "сохраняет константу 1, ";
            else
                str += "не сохраняет константу 1, ";
            if (prop[2] == 1)
                str += "самодвойственная, ";
            else
                str += "не самодвойственная, ";
            if (prop[3] == 1)
                str += "монотонная, ";
            else
                str += "не монотонная, ";
            if (prop[4] == 1)
                str += "линейная.";
            else
                str += "не линейная.";
            return str;
        }

        /// <summary>
        /// Сохраняющая константу 0
        /// </summary>
        public Boolean IsSaving0()
        {
                return BinaryNumber.BinaryFromDecimal(Function, NumberOfBytes)[0] == 0;
        }

        /// <summary>
        /// Сохраняющая константу 1
        /// </summary>
        public Boolean IsSaving1()
        {
            return BinaryNumber.BinaryFromDecimal(Function, NumberOfBytes)[NumberOfBytes - 1] == 1;
        }


        private Boolean CompareSets(int set1, int set2)
        {
            var s1 = BinaryNumber.BinaryFromDecimal(set1, NumberOfArguments);
            var s2 = BinaryNumber.BinaryFromDecimal(set2, NumberOfArguments);
            if (Math.Abs(s1.Sum() - s2.Sum()) != 1)
            {
                MessageBox.Show("Наборы не являются сравнимыми!", "Ошибка");
                return false;
            }
            var f = BinaryNumber.BinaryFromDecimal(Function, NumberOfBytes);
            if (f[set1] == 1 && f[set2] == 0)
                return false;
            return true;
            //if (f[set2] == 1)
            //    return true;
            //if (f[set1] == 0)
            //    return true;

        }

        //private Boolean Compare(int set)
        //{
        //    var s = BinaryNumber.BinaryFromDecimal(set, NumberOfArguments);
        //    var res = true;
        //    for (var i = 0; i < NumberOfArguments; i++)
        //    {
        //        if (s[i] == 0)
        //        {
        //            var news = BinaryNumber.BinaryFromDecimal(set, NumberOfArguments);
        //            news[i] = 1;
        //            var newset = BinaryNumber.DecimalFromBinary(news);
        //            res = res && CompareSets(set, newset) && Compare(newset); //зачем
        //        }
        //    }
        //    return res;
        //}



        /// <summary>
        /// Монотонная
        /// </summary>
        public Boolean IsMonotone()
        {
            // var res = Compare(0);
            // return res;
            int[] func = BinaryNumber.BinaryFromDecimal(Function, NumberOfBytes);
            for (int i = 0; i < NumberOfBytes; i++)
            {
                for (int j = i + 1; j < NumberOfBytes; j++)
                {

                    if ((i & j) == i && func[i] > func[j])
                        return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Самодвойственная
        /// </summary>
        public Boolean IsSelfDual()
        {

                var f = BinaryNumber.BinaryFromDecimal(Function, NumberOfBytes);
                for (var i = 0; i < NumberOfBytes/2; i++)
                {
                    if (f[i] == f[NumberOfBytes - i - 1])
                    {
                        return false;
                    }
                        
                }
                return true;
        }


        /// <summary>
        /// Линейная
        /// </summary>
        public Boolean IsLinear()
        {

                var zh = new ZhegalkinPolynomial(this);
                return zh.IsLinear;

        }



        
    }
}
