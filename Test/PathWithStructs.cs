using Test.boolean_function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    [Serializable]
    public class PathWithStructs
    {
        protected List<string> path;
        protected List<string> structs;
        protected  bool isStraight;

        public List<string> Path { get { return path; } set { path = value; } }
        public List<string> Structs { get { return structs; } set { structs = value; } }
        public bool IsStraight {  get { return isStraight;  } set { isStraight = value; } }
        public PathWithStructs( List<string> path, List<string> structs, bool isStraight)
        {
            this.path = new List<string>();
            this.structs = new List<string>();
            this.path.AddRange(path);
            this.structs.AddRange(structs);
            this.isStraight = isStraight;
        }

      
        //возвращает список ответов на каждый этап некоторого решения задачи
        public List<string> getAnswersForPath( ref object paramIn, ref object paramOut, ref string first, ref string last)
        {
            var lib = new MethodsLibrary();
            List<string> answers = new List<string>();
            var k = 0;
            object parametr;
            if (isStraight)
            {
                parametr = paramIn;
            }
            else
            {
                parametr = paramOut;
            }

            foreach (var m in path)
            {
                var tekStruct = structs[k];
                k++;
                var l = m.Split(' ');
                var info = lib.GetType().GetMethod(l[1]);
                parametr = info.Invoke(lib, new object[] { parametr, true });
                //второй тип данных
                var typename = Program.FindTypeName(tekStruct);
                var T2 = Type.GetType("Test.boolean_function." + typename);

                if (T2 == null)
                {
                    MessageBox.Show("Ошибка в типах данных: " + typename, "Ошибка");
                    return null;
                }
                //экземпляр класса второго типа данных
                var Obj2 = (BFType)Activator.CreateInstance(T2);
                if (m == path[path.Count - 1])
                {
                    if (isStraight)
                    {
                        if (last == "")
                        {
                            last = Obj2.Print(parametr);
                            paramOut = parametr;
                            answers.Add(Obj2.Print(parametr));
                        }
                        else
                        {
                            answers.Add(last);
                        }
                    }
                    else
                    {
                        if (first == "")
                        {
                            first = Obj2.Print(parametr);
                            paramIn = parametr;

                        }

                    }
                }
                else
                {
                    answers.Add(Obj2.Print(parametr));
                }
            }
            return answers;
        }
    }
}
