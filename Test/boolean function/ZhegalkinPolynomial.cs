using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    /// <summary>
    /// Полином Жегалкина
    /// </summary>
    public class ZhegalkinPolynomial
    {
        public BooleanFunction Function { get; set; }
        private Random rnd = new Random(DateTime.Now.Millisecond);
        public int[] Coefficients 
        { 
            get { return _coefficients; }
            set { }
        }
        private int[] _coefficients;

        private int _numberOfArguments;
        private int _numberOfBytes;

        
        public ZhegalkinPolynomial(int[] coefficients, int nargs)
        {
            _coefficients = (int [])coefficients.Clone();
            _numberOfBytes = coefficients.Length;
            _numberOfArguments = nargs;
            Function = FindFunction();
        }


        public ZhegalkinPolynomial(BooleanFunction func)
        {
            _numberOfBytes = func.NumberOfBytes;
            _numberOfArguments = func.NumberOfArguments;
            Function = func;
            _coefficients = new int[_numberOfBytes];
            FindCoefficients();

        }

        public ZhegalkinPolynomial(int[] func)
        {
            _numberOfBytes = func.Length;
            _numberOfArguments = 3;
            if (_numberOfBytes == 16)
                _numberOfArguments = 4;
            Function = new BooleanFunction(BinaryNumber.DecimalFromBinary(func), _numberOfArguments);
            _coefficients = new int[_numberOfBytes];
            FindCoefficients();

        }

        public ZhegalkinPolynomial(int nargs, Boolean linear)
        {
            _numberOfBytes = Convert.ToInt32(Math.Pow(2,nargs));
            _numberOfArguments = nargs;
            if (linear)
                _coefficients = GenerateLinearC(nargs);
            else
                _coefficients = GenerateC(nargs);
            Function = FindFunction();

        }


        private void CountC(int[] pset)
        {
            var f = BinaryNumber.BinaryFromDecimal(Function.Function, _numberOfBytes);

            for (var i = 0; i < _numberOfArguments; i++)
            {
                if (pset[i] == 0)
                {
                    var set = new int[_numberOfArguments];
                    for (var l = 0; l < _numberOfArguments; l++)
                        set[l] = pset[l];
                    set[i] = 1;

                    var k = BinaryNumber.DecimalFromBinary(set);

                    var sum = _coefficients[0];

                    if (pset.Sum() == 0)
                    {
                        _coefficients[k] = Convert.ToInt32(f[k] != sum);
                        
                    }
                    else
                    {
                        for (var j = 0; j < _numberOfArguments; j++)
                        {
                            if (set[j] == 1)
                            {
                                var xset = new int[_numberOfArguments];
                                xset[j] = 1;
                                sum += _coefficients[BinaryNumber.DecimalFromBinary(xset)]; //добавили в сумму а1,а2,а3
                            }
                        }

                        if (pset.Sum() == 1)
                        {
                            sum = sum % 2;
                            _coefficients[k] = Convert.ToInt32(f[k] != sum);
                            
                        }
                        else
                        {
                            for (var j = 0; j < _numberOfArguments; j++)
                            {
                                if (set[j] == 1)
                                {
                                    var xset = new int[_numberOfArguments];
                                    for (var l = 0; l < _numberOfArguments; l++)
                                        xset[l] = set[l];
                                    xset[j] = 0;
                                    sum += _coefficients[BinaryNumber.DecimalFromBinary(xset)]; //добавили в сумму а12,а23,а13
                                }
                            }


                            sum = sum % 2;
                            _coefficients[k] = Convert.ToInt32(f[k] != sum);
                        }
                    }
                }
            }
        }



        private void FindCoefficients()
        {
            var f = BinaryNumber.BinaryFromDecimal(Function.Function, _numberOfBytes);

            //значения коэффициентов неизвестны
            for (var i = 0; i < _numberOfBytes; i++)
                _coefficients[i] = -100;

            //а0
            _coefficients[0] = f[0];

            //найдем а1,а2,а3 (а4)
            CountC(BinaryNumber.BinaryFromDecimal(0,_numberOfArguments));
            //нашли

            //найдем а12,а13,а23...
            for (var i = 0; i < _numberOfArguments; i++)
            {
                var set = BinaryNumber.BinaryFromDecimal(0, _numberOfArguments);
                set[i] = 1;
                CountC(set);
            }

            if (_numberOfArguments == 4)
            {
                //найдем а123, а124
                for (var j = 0; j < _numberOfArguments; j++)
                {
                    var pset = BinaryNumber.BinaryFromDecimal(0, _numberOfArguments);
                    pset[j] = 1;
                    for (var i = 0; i < _numberOfArguments; i++)
                    {
                        if (pset[i] == 0)
                        {
                            var set = new int[_numberOfArguments];
                            for (var l = 0; l < _numberOfArguments; l++)
                                set[l] = pset[l];
                            set[i] = 1;
                            CountC(set);
                        }
                    }
                }
            }
            
            
            //найдем а1234
            var summa = 0;
            for (var i = 0; i < _numberOfBytes - 1; i++)
                summa += _coefficients[i];
            summa = summa % 2;
            _coefficients[_numberOfBytes - 1] = Convert.ToInt32(f[_numberOfBytes - 1] != summa);


        }
        private int[] GenerateLinearC(int nargs)
        {
       
            var x = BinaryNumber.BinaryFromDecimal(rnd.Next(Convert.ToInt32(Math.Pow(2,nargs+1))),nargs+1);

            var res = new int[_numberOfBytes];

            res[0] = x[0];
            var k = 0;
            while (k < nargs)
            {
                res[Convert.ToInt32(Math.Pow(2, k))] = x[k + 1];
                k++;
            }

            if (res.Sum() < 1)
                return GenerateLinearC(nargs);
            return res;
        }

        private int[] GenerateC(int nargs)
        {
            

            var n = rnd.Next(7);
            var res = new int[_numberOfBytes];

            for (var i = 0; i < n; i++)
            {
                res[rnd.Next(_numberOfBytes - 1)] = 1;
            }

            if (res.Sum() < 1)
                return GenerateC(nargs);
            return res;
        }

        private int Conjuct(int index, int[] set)
        {
            var ind = BinaryNumber.BinaryFromDecimal(index, _numberOfArguments);

            var res = 1;

            for (var i=0; i< ind.Length; i++)
            {
                if (ind[i] != 0)
                    res *= set[i];
            }

            return res;
        }

        private int CountFunction(int set)
        {
            var args = BinaryNumber.BinaryFromDecimal(set, _numberOfArguments);
             var list = new List<int>();
            for (var i=0; i<_numberOfBytes; i++)
            {
                if (_coefficients[i] == 1)
                {
                    list.Add(Conjuct(i, args));
                }
            }

            var sum = list.Sum();
            return sum%2;
        }
        private BooleanFunction FindFunction()
        {
            var f = new int[_numberOfBytes];

            for (var i = 0; i < _numberOfBytes; i++)
            {
                f[i] = CountFunction(i);
            }

            return new BooleanFunction(BinaryNumber.DecimalFromBinary(f), _numberOfArguments);
        }

        

        public string Formula()
        {
            var variables = "xyz";
            var str = "";
            var conjs = new List<string>(); 
            for (var i = 0; i < _numberOfBytes; i++)
            {
                if (_coefficients[i] == 1)
                {
                    var args = BinaryNumber.BinaryFromDecimal(i, _numberOfArguments);
                    var conj = "";
                    for (var j = 0; j < _numberOfArguments; j++)
                    {
                        if (args[_numberOfArguments-j-1] == 1)
                        {
                            if (conj != "")
                                conj += "&";
                            conj += variables[j].ToString();
                        }
                    } 
                    if (conj == "")
                        conj = "1";
                    if (!conjs.Contains(conj))
                        conjs.Add(conj);
                    
                } 
            }

            conjs = conjs.OrderBy(x => x.Length).ThenBy(y=>y).ToList();
            foreach (var c in conjs)
            {
                if (str != "")
                    str += " ⊕ ";
                str += c;
            }

            return str;
        }


        public string WrongFormula(int index)
        {
            var coeff = new int[_coefficients.Length];
            switch (index)
            {
                //обратный порядок коэффициентов
                case 0:
                    coeff = _coefficients.Reverse().ToArray();
                    break;
                //противоположные значения
                case 1:
                    for (var i = 0; i < _coefficients.Length; i++)
                    {
                        if (_coefficients[i] == 0)
                            coeff[i] = 1;
                        else
                            coeff[i] = 0;
                    }
                    break;
                //поменяли 2 случайных значения на противоположные
                default:
                    
                    var i1 = rnd.Next(_numberOfBytes);
                    var i2 = rnd.Next(_numberOfBytes);
                    for (var i = 0; i < _coefficients.Length; i++)
                    {
                        if (i == i1 || i == i2)
                        {
                            if (_coefficients[i] == 0)
                                coeff[i] = 1;
                            else
                                coeff[i] = 0;
                        }
                        else
                        {
                            coeff[i] = _coefficients[i];
                        }  
                    }
                    break;
            }

            var zh = new ZhegalkinPolynomial(coeff, _numberOfArguments);
            return zh.Formula();

        }

        public Boolean IsLinear
        {
            get
            {
                for (var i = 0; i < _numberOfBytes; i++)
                {
                    if (i != 0 && i != 1 && i != 2 && i != 4 && i != 8 && _coefficients[i] == 1)
                        return false;
                }
                return true;
            }
        }


    }
}
