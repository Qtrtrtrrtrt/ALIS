using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Test.boolean_function;


namespace Test
{

    //sets
    public class MethodsLibrary
    {
        private Random _rnd = new Random(DateTime.Now.Millisecond);

        private List<int> union(List<CustomSet> sets)
        {
            var result = sets[0].getSet();
            for (int i = 1; i < sets.Count; i++)
            {
                foreach (int elem in sets[i].getSet())
                {
                    if (!result.Contains(elem))
                    {
                        result.Add(elem);
                    }
                }
            }
            result.Sort();
            return result;
        }

        private List<int> intersection(List<CustomSet> sets)
        {
            var result = sets[0].getSet();
            for (int i = 1; i < sets.Count; i++)
            {
                var currentSet = sets[i].getSet();
                result = result.Intersect(currentSet).ToList();
            }
            result.Sort();
            return result;
        }


        public List<int> GetUnion(List<CustomSet> sets, Boolean isCorrect)
        {
            if (isCorrect)
            {
                //правильный ответ
                return union(sets);
            }
            var chance = _rnd.Next(100);
            if (chance < 70)
            {
                //не убирает дубликаты 
                var result = sets[0].getSet();
                for (int i = 1; i < sets.Count; i++)
                {
                    //с какой-то вероятнотью не делает объединение если множеств больше 2
                    if (sets.Count == 2 || chance < 40)
                    {
                        result.AddRange(sets[i].getSet());
                    }
                }
                if (sets.Count == 2 && chance < 30)
                {
                    var remo = result[_rnd.Next(result.Count)];
                    result.Remove(remo);
                }
                result.Sort();
                return result;
            }
            else
            {
                //пересечение вместо объединения
                return intersection(sets);
            }
    
        }

        public List<int> GetIntersection(List<CustomSet> sets, Boolean isCorrect)
        {
            if (isCorrect)
            {
                //правильный ответ
                return intersection(sets);
            }
            var chance = _rnd.Next(100);
            if (chance < 85)
            {
                int rnd_First_set = _rnd.Next(sets.Count);
                //что-то непонятное делается
                var result = sets[rnd_First_set].getSet();
                for (int i = 0; i < sets.Count; i++)
                {
                    if (i == rnd_First_set)
                    {
                        continue;
                    }
                    chance = _rnd.Next(100);
                    var currentSet = sets[i].getSet();
                    if (chance % 3 == 0)
                    {
                        result = result.Intersect(currentSet).ToList();
                    } 
                    if (chance % 2 == 0)
                    {
                        bool flag = true;
                        int maxlimit = 30;
                        while (flag && maxlimit>0 )
                        {
                            int rand_k = _rnd.Next(currentSet.Count );
                            if (result.Count == 0 || !result.Contains(currentSet[rand_k]))
                            {
                                result.Add(currentSet[rand_k]);
                                flag = false;
                            }
                            maxlimit--;
                        }
                    }
                    
                   
                }
                chance = _rnd.Next(100);
                if (chance % 2 == 0 & result.Count > 1)
                {
                    int rand_k = _rnd.Next(result.Count);
                    result.RemoveAt(rand_k);
                }
                    result.Sort();
                return result;
            }
            else
            {
                // объединение вместо пересечения
                return union(sets);
            }
        }

        public int getCardinality(List<int> set, Boolean isCorrect)
        {
            if (isCorrect)
            {
                return set.Count;
            }
            var chance = _rnd.Next(100);
            if (set.Count > 0)
            {
                if (chance < 30)
                    return set.Max();
                if (chance < 60)
                    return set.Count - 1;
            }
            return set.Count + 1;
        }


        //boolean function

        public int[] VariablesTypes(int[] function, Boolean isCorrect)
        {
            var res = new int[3];
            //если вес вектора функции нечетен, то все существенные
            if (function.Sum() % 2 != 0)
            {
                res = new[] { 1, 1, 1 };
            } else
            {
                for (var i = 0; i < 4; i++)
                    if (function[i] != function[i + 4])
                        res[0] = 1;

                for (var i = 0; i <= 1; i++)
                    if (function[i] != function[i + 2])
                        res[1] = 1;
                for (var i = 4; i <= 5; i++)
                    if (function[i] != function[i + 2])
                        res[1] = 1;

                for (var i = 0; i < 8; i += 2)
                    if (function[i] != function[i + 1])
                        res[2] = 1;
            }

            if (isCorrect)
            {
                return res;
            }
            var chance = _rnd.Next(100);
            if (chance < 30)
            {
                return res.Reverse().ToArray();
            }
            res[_rnd.Next(3)] = (res[_rnd.Next(3)] + 1) % 2;
            if (chance < 60)
            {
                return res;
            }
            return res.Reverse().ToArray();
        }


        public string PdnfFromVector(int[] function, Boolean isCorrect)
        {
            var f = new BooleanFunction(function);
            var nf = new NormalForm(f);
            if (isCorrect)
            {
                return nf.PDNF();
            }
            var chance = _rnd.Next(100);
            if (chance< 40)
            {
                return nf.WrongPDNF(chance % 3);
            }
            if (chance < 70)
            {
                return nf.PCNF();
            }
            return nf.WrongPCNF(chance % 3);
        }


        public string PcnfFromVector(int[] function, Boolean isCorrect)
        {
            var f = new BooleanFunction(function);
            var nf = new NormalForm(f);
            if (isCorrect)
            {
                return nf.PCNF();
            }
            var chance = _rnd.Next(100);
            if (chance < 40)
            {
                return nf.WrongPCNF(chance % 3);
            }
            if (chance < 70)
            {
                return nf.PDNF();
            }
            return nf.WrongPDNF(chance % 3);
        }


        public string ZhegalkinFromCoefficients(int[] coeff, Boolean isCorrect)
        {
            var f = new BooleanFunction(coeff);
            var zh = new ZhegalkinPolynomial(coeff, f.NumberOfArguments);
            if (isCorrect)
            {
                return zh.Formula();
            }
            var chance = _rnd.Next(100);
            return zh.WrongFormula(chance % 3);
        }

        public int[] ZhegalkinCoefficientsFromVector(int[] func, Boolean isCorrect)
        {
            var zh = new ZhegalkinPolynomial(func);
            if (isCorrect)
            {
                return zh.Coefficients;
            }
            var chance = _rnd.Next(100);
            if (chance < 30)
            {
                return zh.Coefficients.Reverse().ToArray();
            }
            var res = zh.Coefficients;
            res[_rnd.Next(res.Length)] = (res[_rnd.Next(3)] + 1) % 2;
            if (chance < 60)
            {
                return res;
            }
            if (chance %4 == 0)
            {
                res[_rnd.Next(res.Length)] = 0;
            }
            return res.Reverse().ToArray();
        }

        public int[] VectorFromZhegalkinCoefficients(int[] coeff, Boolean isCorrect)
        {
            var f = new BooleanFunction(coeff);
            var zh = new ZhegalkinPolynomial(coeff, f.NumberOfArguments);
            
            var fu = zh.Function.Function;
            if (isCorrect)
            {
                return BinaryNumber.BinaryFromDecimal(fu, zh.Function.NumberOfBytes);
            }
            fu = Math.Abs(fu + _rnd.Next(-10, 10)) % (int)Math.Pow(2, zh.Function.NumberOfBytes);
            return BinaryNumber.BinaryFromDecimal(fu, zh.Function.NumberOfBytes);
        }


        public int[] FunctionProperties(int[] func, Boolean isCorrect)
        {
            var f = new BooleanFunction(func);
            var props = f.FindProperties();
            if (isCorrect)
            {
                return props;
            }
            var index = _rnd.Next(5);
            props[index] = (props[index] + 1) % 2;
            return props;
        }

        public int[] FunctionFromProperties(int[] prop, Boolean isCorrect)
        {
            var nargs = 3;
            BooleanFunction f= new BooleanFunction(0,nargs);
            int all_funcs_count = (int)Math.Pow(2, Math.Pow(2, nargs));

            if (isCorrect)
            {
                bool flag_found = false;
                while (!flag_found && f.Function < all_funcs_count - 1)
                {
                    flag_found = CompareArrays(prop, f.FindProperties());
                    f.Function++;

                }
                if (!flag_found)
                    throw new Exception("not found");

            }
            else
            {
                do
                {
                    f.Function = _rnd.Next(all_funcs_count);
                } while (CompareArrays(prop, f.FindProperties()));

            }


            //if (prop[4] == 1)
            //    f = new BooleanFunction(FunctionClass.Linear, nargs);
            //else
            //{
            //    if (prop[3] == 1)
            //        f = new BooleanFunction(FunctionClass.Monotone, nargs);
            //    else
            //    {
            //        if (prop[2] == 1)
            //            f = new BooleanFunction(FunctionClass.SelfDual, nargs);
            //        else
            //        {
            //            if (prop[1] == 1)
            //                f = new BooleanFunction(FunctionClass.Save1, nargs);
            //            else
            //            {
            //                f = new BooleanFunction(FunctionClass.Save0, nargs);
            //            }
            //        }
            //    }
            //}


            return BinaryNumber.BinaryFromDecimal(f.Function, f.NumberOfBytes);
        }

        private Boolean CompareArrays(int[] a, int[] b)
        {
            for (var i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        private Boolean IsFullSystem(IEnumerable<BooleanFunction> fs)
        {
            var prop = new int[] { 1, 1, 1, 1, 1 };
            foreach (var f in fs)
            {
                var pr = f.FindProperties();
                for (var i = 0; i < 5; i++)
                    prop[i] *= pr[i];
            }
            return (prop.Sum() == 0);
        }

        private Boolean ContainsFunction(List<BooleanFunction> list, int func, int nargs, Boolean isCorrect)
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


        public List<List<BooleanFunction>> MinimalFullSystems(List<BooleanFunction> fs, Boolean isCorrect)
        {
            var fulls = FindFullSystems(fs, 1, isCorrect);
            var kf = 2;

            while (fulls.Count == 0 && kf < fs.Count)
            {
                fulls = FindFullSystems(fs, kf, isCorrect);
                kf++;
            }
            //если не нашлось подсистем, проверить систему целиком
            if (fulls.Count == 0 && IsFullSystem(fs))
                fulls.Add(fs);

            if (isCorrect)
            {
                return fulls;
            }

            var chance = _rnd.Next(100);
            
            if (fulls.Count != 0 && chance < 40)
                return new List<List<BooleanFunction>>();

            //если нашлась хотя бы одна отличная от исходной системы, добавить вариант, что только вся система - ф.п
            if (fulls.Count != 1 || fulls[0].Count != fs.Count && chance < 60)
                return new List<List<BooleanFunction>>() { fs };

            //нагенерировать оставшиеся неправильные варианты (либо 1, либо 2)
            var list = new List<BooleanFunction>();
            for (var i = 0; i < fs.Count - 1; i++)
            {
                if (chance < 50 && i % 2 == 1)
                {
                     list.Add(fs[i]);
                }
             
            }
            return new List<List<BooleanFunction>>() { list };

        }
        public List<List<BooleanFunction>> MinimalFromAllFullSystems(List<List<BooleanFunction>> fs, Boolean isCorrect)
        {
            var fulls = new List<List<BooleanFunction>>();
            if (fs.Count == 0)
                return fulls;
            var n = fs[0].Count;

            foreach (var f in fs)
            {
                if (f.Count == n && isCorrect)
                    fulls.Add(f);
                if ( !isCorrect)
                {
                   
                    if (_rnd.Next(10) > 6)
                        fulls.Add(f);
                }
             
            }
            return fulls;

        }
        public List<List<BooleanFunction>> AllFullSystems(List<BooleanFunction> fs, Boolean isCorrect)
        {
            var fulls = FindFullSystems(fs, 1, isCorrect);
            for (var i = 2; i < fs.Count; i++)
            {
                fulls.AddRange(FindFullSystems(fs, i,isCorrect));
            }
            if (IsFullSystem(fs) && fs.Count != 2 && !fulls.Exists(x => x.Count == 4) && isCorrect)
                fulls.Add(fs);
            if ( !isCorrect)
            {
                
                if (_rnd.Next(10) %3==0)
                    fulls.Add(fs);
            }


            return fulls;
        }
        private List<List<BooleanFunction>> FindFullSystems(List<BooleanFunction> fs, int n, bool isCorrect)
        {
            var res = new List<List<BooleanFunction>>();

            for (var i = 0; i < fs.Count - n + 1; i++)
            {
                for (var j = i + 1; j <= fs.Count - n + 1; j++)
                {
                    if (n == 2)
                    {
                        var list = new List<BooleanFunction> { fs[i], fs[j] };
                        if (IsFullSystem(list) && isCorrect)
                            res.Add(list);
                        if (!isCorrect)
                        {
                           
                            if (_rnd.Next(10) > 7)
                                res.Add(fs);
                        }
                    }
                    else
                    {
                        for (var k = j + 1; k < fs.Count - n + 2; k++)
                        {
                            if (n == 3)
                            {
                                var list = new List<BooleanFunction> { fs[i], fs[j], fs[k] };
                                if (IsFullSystem(list) && isCorrect)
                                    res.Add(list);
                                if (!isCorrect)
                                {
                                   
                                    if (_rnd.Next(10) > 7)
                                        res.Add(fs);
                                }
                            }
                            else
                            {
                                for (var l = k + 1; l < fs.Count; l++)
                                {
                                    var list = new List<BooleanFunction> { fs[i], fs[j], fs[k], fs[l] };
                                    if (IsFullSystem(list) && isCorrect)
                                        res.Add(list);
                                    if (!isCorrect)
                                    {
                                       
                                        if (_rnd.Next(10) > 7)
                                            res.Add(fs);
                                    }
                                }
                            }
                        }

                    }


                }
            }

            return res;
        }
    }

}
