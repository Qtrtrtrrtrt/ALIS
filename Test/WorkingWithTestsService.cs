using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Test.boolean_function;

namespace Test
{
    public class WorkingWithTestsService
    {
       
        private bool correctTemplateOpened;
       
        private TestTemplate activeTemplate;
        private int variantCountForGen;
        private List<Variant> activeVariants;
       
        public bool CorrectTemplateOpened { get { return correctTemplateOpened; } set { correctTemplateOpened = value; } }
       
        public TestTemplate ActiveTemplate { get { return activeTemplate; } }
        public int VariantCountsForGen { get { return variantCountForGen; } set { variantCountForGen = value; } }
        public List<Variant> ActiveVariants { get { return activeVariants; } }

        public WorkingWithTestsService ()
        {
            
            correctTemplateOpened = false;
     
        }
        public bool isTemplateMatchVars()
        {
            bool ok =  activeTemplate != null && activeVariants != null && activeVariants.Count > 0 && activeVariants[0].TestName == activeTemplate.Name;
            correctTemplateOpened = ok;
            return ok;
        }
        public bool needToRegenerate ()
        {
            return correctTemplateOpened == true && activeVariants != null && activeVariants.Count != 0;
        }
        public string findTemplateForVariants()
        {
            correctTemplateOpened = false;
            string path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();
            var files = Directory.GetFiles(path + "\\шаблоны");
            string _templateFileName = "";
            for (int i = 0; i < files.Length; i++)
            {
                var formatter = new XmlSerializer(typeof(TestTemplate));
                var fStream = File.OpenRead(files[i]);

                TestTemplate tmp = (TestTemplate)formatter.Deserialize(fStream);
                if (tmp.Name == activeVariants[0].TestName)
                {
                    bool ok = true;
                    foreach (var ext in tmp.ExercisesT)
                    {
                        var exs = activeVariants[0].Exercises.FindAll((x) => x.Type == ext.Key.Type);
                        if (exs == null || exs.Count != ext.Value)
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        activeTemplate = tmp;
                        _templateFileName = files[i];
                        correctTemplateOpened = true;
                    }
                }

                fStream.Close();
            }
            return _templateFileName;
        }
        public void clearVariants()
        {
            
            if (activeVariants != null)
                {
                    activeVariants.Clear();
                }
        }
        public void removeVariant(Variant var)
        {
            if (activeVariants.Count == 0)
            {
                return;
            }

            activeVariants.Remove(var);
        }
        public void clearService()
        {
            if (activeVariants != null)
            {
                activeVariants.Clear();
            }
            activeTemplate = null;
            activeVariants = null;
            correctTemplateOpened = false;
        }
        public void addNewExerciseTemplate(string task, List<object> paramValues)
        {
            activeTemplate.generateExerciseT(task,paramValues);
        }
        public List<string> getParametersForExercise(string task)
        {

            //входные и выходные данные 
            var input = Program.MainOntology.FindConnectedClasses(task, "input")[0];
            var output = Program.MainOntology.FindConnectedClasses(task, "output")[0];
            var first = input;
            var last = output;

            //путь между вершинами
            var path = Program.MainOntology.FindMethods(input, output);

            if (path == null || path.Count == 0)
            {
                path = Program.MainOntology.FindMethods(output, input);
                first = output;
                last = input;
                if (path == null || path.Count == 0)
                {
                    MessageBox.Show("Невозможно сгенерировать задачу!", "Ошибка");
                    return null ;
                }
            }

            //параметры задачи
            var taskParameters = Program.MainOntology.FindConnectedClasses(task, "parameter");
            //параметры данных на входе генерации
            var firstParameters = Program.MainOntology.FindConnectedClasses(first, "has");
            var parameters = new List<string>();
            foreach (var p in taskParameters)
            {
                if (Program.MainOntology.FindConnectedClasses(p, "is_a").Contains("Параметр") && firstParameters.Contains(p))
                {
                    parameters.Add(p);
                }
            }
            return parameters;
        }
       
        public void deleteExerciseFromTemplate(string ex)
        {
            activeTemplate.DeleteExercise(ex);
        }
        public void saveVariants(string filename)
        {
            var formatter = new XmlSerializer(typeof(List<Variant>));
            var fStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(fStream, activeVariants);
            fStream.Close();
        }
        public void saveTemplate(string filename)
        {
            var formatter = new XmlSerializer(typeof(TestTemplate));
            var fStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(fStream, activeTemplate);
            fStream.Close();
        }
        public void generateVariants(int count)
        {
            clearVariants();
            activeVariants = new List<Variant>();
            for (int number = 1; number <= count; number++)
            {
                Variant var = new Variant(number, activeTemplate.Name, activeTemplate.WithOpenAnswers);
                foreach (KeyValuePair<ExerciseTemplate, int> template in activeTemplate.ExercisesT)
                {
                    for (int i = 0; i < template.Value; i++)
                    {
                        var.generateExercise(template.Key);
                    }

                }
                activeVariants.Add(var);
            }
                       
            correctTemplateOpened = true;
        }
        public void regenerateExerciseInVariant(int variantNumber, Exercise ex)
        {
           
            foreach (KeyValuePair<ExerciseTemplate, int> template in activeTemplate.ExercisesT)
            {
                if (template.Key.Type == ex.Type)
                {
                
                    for (int i = 0; i < activeVariants.Count; i++)
                    {
                        if (activeVariants[i].Number == variantNumber)
                        {
                            activeVariants[i].Exercises.Remove(ex);
                            activeVariants[i].generateExercise(template.Key);
                            Program.mainForm.needSaveVariants = true;
                            
                        }
                    }

                }

            }
        }
        public void setNewTemplate(bool open, string name)
        {
            activeTemplate = new TestTemplate(open, name, Program.MainOntology.Name);
        }
        public void loadVariants(string filename)
        {
            List<Variant> tmp;
            var formatter = new XmlSerializer(typeof(List<Variant>));
            
            var fStream = File.OpenRead(filename);
            try
            {
                tmp  = (List<Variant>)formatter.Deserialize(fStream);
            }catch(Exception e)
            {
                MessageBox.Show("Произошла ошибка при считывании варианта.\nОшибка: " + e.Message);
                fStream.Close();
                return;
            }
            fStream.Close();
          
            activeVariants = tmp;
           
            
        }
        public void loadTemplate(string filename)
        {
            TestTemplate tmp;
            var formatter = new XmlSerializer(typeof(TestTemplate));
            var fStream = File.OpenRead(filename);
            try
            {
                tmp = (TestTemplate)formatter.Deserialize(fStream);

            }
            catch (Exception excep)
            {
                MessageBox.Show("Ошибка при открытии шаблона теста:\n", excep.Message);
                fStream.Close();
                return;
            }
          
            if (tmp.Ontology == Program.MainOntology.Name)
            {
                activeTemplate = tmp;
            }
            else
            {
                MessageBox.Show("Этот шаблон теста относится к другой онтологии");
                return;
            }
            fStream.Close();
        }
        public void printVariants()

        {
            bool isOpen = activeVariants[0].WithOpenAnswer;
            if (activeVariants.Count == 0)
            {
                MessageBox.Show("Нет ни одной задачи!");
                return;
            }

            var app = new Microsoft.Office.Interop.Word.Application
            {
                Visible = true
            };
            var doc = app.Documents.Add();
            var k = 1;
            foreach (var variant in activeVariants)
            {
                doc.Content.Text += "Вариант " + variant.Number + "\n";

                foreach (var ex in variant.Exercises)
                {
                    doc.Content.Text += (k++) + ") " + (isOpen ? ex.Print(ExerciseType.TestWithoutChoices) : ex.Print(ExerciseType.MultipleСhoiceTest));
                }
                doc.Paragraphs[doc.Paragraphs.Count].Range.InsertBreak();
                doc.Content.Text += "ОТВЕТЫ\n";
                k = 1;
                foreach (var ex in variant.Exercises)
                    doc.Content.Text += (k++) + ") " + (isOpen ? ex.Print(ExerciseType.TestWithoutChoicesForTeacher) : ex.Print(ExerciseType.MultipleСhoiceTestForTeacher));
                doc.Paragraphs[doc.Paragraphs.Count].Range.InsertBreak();
            }
        }

        


    }


}
