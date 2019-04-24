using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.boolean_function;

namespace Test
{
    [Serializable]
    public class Variant
    {
        /// <summary>
        /// Номер варианта
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Название теста
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        ///Открытый тест или закрытый
        /// </summary>
        public bool WithOpenAnswer { get; set; }

        /// <summary>
        ///Список заданий варианта
        /// </summary>
        public List<Exercise> Exercises;

        public Variant()
        {
            Number = 1;
            Exercises = new List<Exercise>();
        }
        public Variant(int number, string test, bool withOpenAnswers)
        {
            Number = number;
            Exercises = new List<Exercise>();
            TestName = test;
            WithOpenAnswer = withOpenAnswers;
        }

        public void generateExercise(ExerciseTemplate template)
        {
            MethodsLibrary lib = new MethodsLibrary();
            Exercise ex = new Exercise();
            var input = template.First;
            var output = template.Last;
            //тип исходных данных
            var typename = Program.FindTypeName(template.First);
            var T = Type.GetType("Test.boolean_function." + typename);
            if (T == null)
            {
                MessageBox.Show("Ошибка в типах данных!", "Ошибка");
                return;
            }
            //экземпляр класса исходных типа данных

            var Obj = (BFType)Activator.CreateInstance(T);

            var firstGenerationData = Obj.Generate(template.ExParams);
            var parametr = firstGenerationData;

            var firststring = "";
            var laststring = "";
            if (input == template.First)
                firststring = Obj.Print(parametr);
            else
                laststring = Obj.Print(parametr);

            //вызовы всех методов
            foreach (var m in template.Path)
            {
                var l = m.Split(' ');
                var info = lib.GetType().GetMethod(l[1]);
                parametr = info.Invoke(lib, new[] { parametr, true });
            }

            //второй тип данных
            typename = Program.FindTypeName(template.Last);
            var T2 = Type.GetType("Test.boolean_function." + typename);

            if (T2 == null)
            {
                MessageBox.Show("Ошибка в типах данных!", "Ошибка");
                return;
            }
            //экземпляр класса второго типа данных
            var Obj2 = (BFType)Activator.CreateInstance(T2);

            if (output == template.Last)
                laststring = Obj2.Print(parametr);
            else
            {
                firststring = Obj2.Print(parametr);
            }

            int maxGenerate = 100;
            var answers = new List<string> { laststring };
            if (!WithOpenAnswer)
            {
                int[] wrongSteps;
                while (maxGenerate != 0 && answers.Count < 4)
                {
                    if (template.Path.Count == 1)
                    {
                        wrongSteps = new int[1] { 1 };
                    }
                    else
                    {
                        var rnd = new Random(DateTime.Now.Millisecond);
                        var wrongStepsValue = rnd.Next((int)Math.Pow(2, template.Path.Count));

                        wrongSteps = BinaryNumber.BinaryFromDecimal(wrongStepsValue, template.Path.Count);
                    }
                    var wrongAnswer = generateWrong(firstGenerationData, template.Path, Obj2, wrongSteps, lib);
                    if (answers.Contains(wrongAnswer))
                    {
                        maxGenerate--;
                        if (maxGenerate == 0)
                            MessageBox.Show("max generation limit");
                    }
                    else
                    {
                        answers.Add(wrongAnswer);
                    }
                }
            }
            ex = new Exercise(template.Type, template.Type + " " + firststring + ".\n", answers, laststring);
            Exercises.Add(ex);
        }

        private string generateWrong(object data, List<string> path, BFType objType, int[] wrongSteps, MethodsLibrary lib)
        {
            var parametr = data;
            //вызовы всех методов
            for (int i = 0; i < path.Count; i++)
            {
                var l = path[i].Split(' ');
                var info = lib.GetType().GetMethod(l[1]);
                parametr = info.Invoke(lib, new[] { parametr, wrongSteps[i] == 0 });
            }

            return objType.Print(parametr);
        }
      
    }
}
