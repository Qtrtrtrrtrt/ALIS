using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Test.boolean_function
{
    abstract public class BFType
    {
        public virtual object Generate()
        {
            return null;
        }

        public virtual object Generate(List<object> parameters)
        {
            return null;
        }

        public virtual string Print(object obj)
        {
            return obj.ToString();
        }
    }


    //sets

    class Cardinality:BFType
    {
        public int Data;

        public override object Generate(List<object> parameters)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            return rnd.Next(15);
        }
        
    }

    class Set:  BFType
    {
        public List<int> Data;

        public override object Generate(List<object> parameters)
        {
            int n;
            if (parameters.Count > 0)
            {
                n = Convert.ToInt32(parameters[0]);
            }
            else
            {
                n = 10;
            }
            var set = new CustomSet(n);
            return set.getSet();
        }

        public override string Print(object obj)
        {
            Data = obj as List<int>;
            return "{" + String.Join("; ", Data) + "}";
        }
        
    }

    class ListOfSets: BFType
    {
        public List<CustomSet> Data;

        public override object Generate(List<object> parameters)
        {
            int n;
            if (parameters.Count > 0)
            {
                n = Convert.ToInt32(parameters[0]);
            }
            else
            {
                n = 2;
            }
            var sets = new List<CustomSet>();
            for (int i = 0; i< n; i++)
            {
                sets.Add(new CustomSet());
            }
          
            return sets;
        }

        public override string Print(object obj)
        {
            Data = obj as List<CustomSet>;
            var stringSets = new List<string>();
            foreach(CustomSet set in Data)
            {
                stringSets.Add(set.getString());
            }
            return String.Join(",  ", stringSets);
        }
        
    }


    //bf
    class Formula : BFType
    {
        public string Data;

        public Formula()
        {

        }
        public Formula(string str)
        {
            Data = str;
        }

        public override string ToString()
        {
            return Data;
        }

        public override string Print(object obj)
        {
            return obj.ToString();
        }
       
    }

    class IntArray : BFType
    {
        public int[] Data;

        public IntArray()
        {

        }
        public IntArray(int[] ar)
        {
            Data = ar;
        }

        public override object Generate(List<object> parameters)
        {
            int n;
            if (parameters.Count > 0)
            {
                n = Convert.ToInt32(parameters[0]);
            }
            else
            {
                n = 3;
            }
            var f = new BooleanFunction(n);
            return f.Vector;
        }

        public override string Print(object obj)
        {
            Data = obj as int[];
            var s = "(";
            foreach (var a in Data)
                s += a;
            s += ")";
            return s;
        }

        
    }

    class IntArray8 : BFType
    {
        public int[] Data;

        public IntArray8()
        {

        }
        public IntArray8(int[] ar)
        {
            Data = ar;
        }

        public override object Generate(List<object> parameters)
        {
            var f = new BooleanFunction(3);
            return f.Vector;
        }

        public override string Print(object obj)
        {
            Data = obj as int[];
            var s = "(";
            foreach (var a in Data)
                s += a;
            s += ")";
            return s;
        }
       
    }

    class IntArray3 : BFType
    {
        public int[] Data;

        public IntArray3()
        {
            Data = new int[3];
        }
        public IntArray3(int[] ar)
        {
            Data = ar;
        }


        public override object Generate(List<object> parameters)
        {
            Data = new int[3];
            var rnd = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 3; i++)
                Data[i] = rnd.Next(0, 2);
            return Data;
        }

        public override string Print(object obj)
        {
            var ar = obj as int[];
            var s = "(";
            foreach (var a in ar)
                s += a;
            s += ")";
            return s;
            //Data = obj as int[];
            //var f = "";
            //var s = "";
            //for (var i = 0; i < 3; i++)
            //{
            //    if (Data[i] == 0)
            //    {
            //        if (f != "")
            //            f += ", ";
            //        f += "x" + (i + 1);
            //    }
            //    else
            //    {
            //        if (s != "")
            //            s += ", ";
            //        s += "x" + (i + 1);
            //    }
            //}
            //var str = "";
            //if (s != "")
            //{
            //    str += "Существенные - " + s;
            //    if (f != "")
            //        str += "; ";
            //}
            //if (f != "")
            //    str += "Фиктивные - " + f;
            //return str;
        }
        
    }

    class IntArray5 : BFType
    {
        public int[] Data;

        public IntArray5()
        {

        }
        public IntArray5(int[] ar)
        {
            Data = ar;
        }

        public override object Generate(List<object> parameters)
        {
            var res = new int[5];
            var rnd = new Random(DateTime.Now.Millisecond);
            BooleanFunction f;
            if (rnd.Next(50) < 40)
            {
                f = new BooleanFunction(FunctionClass.Linear, 3);
            } else
            {
                f = new BooleanFunction(rnd.Next(256), 3);
            }
              res = f.FindProperties();
       
            return res;
        }

        public override string Print(object obj)
        {
            var ar = obj as int[];
            var s = "(";
            foreach (var a in ar)
                s += a;
            s += ")";
            return s;
        }

    }

    class FunctionSystem : BFType
    {
        public List<BooleanFunction> Data;

        public FunctionSystem()
        {

        }

        private Boolean ContainsFunction(List<BooleanFunction> list, int func, int nargs)
        {
            foreach (var f in list)
            {
                if (f.Function == func && f.NumberOfArguments == nargs)
                    return true;
                if (func == 0 && f.Function == 0)
                    return true;
                if (func == Math.Pow(2, Math.Pow(2, nargs)) - 1 && f.Function == Math.Pow(2, f.NumberOfBytes) - 1)
                    return true;
            }
            return false;
        }
        public override object Generate(List<object> parameters)
        {
            var fs = new List<BooleanFunction>();
            var rnd = new Random();
            var nfunc = rnd.Next(3, 6);
            var n = rnd.Next(2, 4);

            for (var i = 0; i < nfunc; i++)
            {
                var n1 = rnd.Next(1, n + 1);
                var f = rnd.Next(Convert.ToInt32(Math.Pow(2, Math.Pow(2, n1))));
                while (ContainsFunction(fs, f, n1))
                    f = rnd.Next(Convert.ToInt32(Math.Pow(2, Math.Pow(2, n1))));
                fs.Add(new BooleanFunction(f, n1));

            }
            return fs;
        }

        public override string Print(object obj)
        {
            Data = obj as List<BooleanFunction>;
            var res = "{";
            foreach (var f in Data)
            {
                if (res != "{")
                    res += " , ";
                res += f.VectorString;
            }
            return res + "}";
        }
        
    }

    class SetOfSystems : BFType
    {
        public List<List<BooleanFunction>> Data;


        public override object Generate(List<object> parameters)
        {
            return new List<List<BooleanFunction>>();
        }

        public override string Print(object obj)
        {
            Data = obj as List<List<BooleanFunction>>;
            if (Data.Count == 0)
                return "Нет ни одной функционально полной системы";
            var res = "";
            foreach (var sys in Data)
            {
                if (res != "")
                    res += " ; ";
                res += new FunctionSystem().Print(sys);
            }
            return res;
        }

       
    }

}
