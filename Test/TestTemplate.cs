using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Test
{
    //класс для описания шаблона теста, на основе которого генерируются варианты
    [Serializable]
    public class TestTemplate
    {
        /// <summary>
        /// Тип ответов для заданий теста - закрытые (false), открытые - (true)
        /// </summary>
        public bool WithOpenAnswers { get; set; }

        /// <summary>
        /// Название теста
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Задания в тесте и их количество в тесте
        /// </summary>
        public XmlSerializableDictionary<ExerciseTemplate, int> ExercisesT { get; set; }

        /// <summary>
        /// К какой онтологии относится этот шаблон
        /// </summary>
        public string Ontology { get; set; }


        public TestTemplate()
        {
            Name = "";
            WithOpenAnswers = true;
            ExercisesT = new XmlSerializableDictionary<ExerciseTemplate, int>();
        }
        public TestTemplate(bool withOpenAnswers, string name, string ontology){
            this.Name = name;
            this.WithOpenAnswers = withOpenAnswers;
            this.ExercisesT = new XmlSerializableDictionary<ExerciseTemplate, int>();
            this.Ontology = ontology;
        }

      
     
        public void generateExerciseT(string task, List<object> paramValues)
        {
            //входные и выходные данные 
            var input = Program.MainOntology.FindConnectedClasses(task, "input")[0];
            var output = Program.MainOntology.FindConnectedClasses(task, "output")[0];
            string first = input, last = output;
            List<string> path;
            LinkPath ex = Program.RecommendedPaths.Find((y) => y.Type == task);
            if (ex != null)
            {
                path = new List<string>();
                path.AddRange(ex.Path.Path);
                if (!ex.Path.IsStraight)
                {
                    first = output;
                    last = input;
                }
            }
            else
            {
                path = Program.MainOntology.FindMethods(input, output);

                if (path == null || path.Count == 0)
                {
                    path = Program.MainOntology.FindMethods(output, input);
                    first = output;
                    last = input;
                    if (path == null || path.Count == 0)
                    {
                        MessageBox.Show("Невозможно сгенерировать задачу!", "Ошибка");
                        return;
                    }
                }
            }
            ExerciseTemplate extemp = new ExerciseTemplate(task, first, last, path);
            foreach (object p in paramValues)
            {
                extemp.AddParam(p);
            }



            bool check = false;
            foreach (KeyValuePair< ExerciseTemplate, int> d in ExercisesT)
            {
                if (d.Key.Type == extemp.Type)
                {
                    bool f = true;
                    for (int i = 0; i < extemp.ExParams.Count; i++)
                    {

                        if (((IComparable)extemp.ExParams[i]).CompareTo(d.Key.ExParams[i]) != 0)
                        {
                            f = false;
                        }

                    }
                    if (f)
                    {
                        ExercisesT[d.Key]++;
                        check = true;
                        break;
                    }
                }
                
            }
            
            if (!check)
            {
                ExercisesT.Add(extemp, 1);
            }
            
        }
        public void DeleteExercise(string type)
        {
            ExerciseTemplate key = null;
            foreach (KeyValuePair<ExerciseTemplate, int > ex in ExercisesT)
            {
                if (ex.Key.Type == type)
                {
                    key = ex.Key;
                }
            }
            if (ExercisesT[key] > 1)
            {
                ExercisesT[key]--;
            }
            else
            {
                ExercisesT.Remove(key);
            }
        }
        
    }
}
