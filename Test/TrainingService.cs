using Test.boolean_function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public class TrainingService
    {
        //все задачи со всеми путями  (либо с назначенными)
        private List<ExerciseWithPaths> exercisesWithPaths;

        private List<StudentsWeakPoint> weakPoints;

        private string curTaskMethod;

        //правильный ответ для текущего этапа решения
        private string rightAnswerForStep;

        //для отслеживания выбранного пути решения
        private int indexOfNextStruct;

        //варианты пути для задачи
        private List<PathWithSA> alternatives;

        private string firststring, laststring;
       

        public string Firststring { get { return firststring; } }
        public string Laststring { get { return laststring; } }
        public List<ExerciseWithPaths> ExercisesWithPaths { get { return exercisesWithPaths; } }
        public int IndexOfNextStruct { get { return indexOfNextStruct;  } set { indexOfNextStruct = value; } }
        public string RightAnswerForStep { get { return rightAnswerForStep;  } }
        public List<PathWithSA> Alternatives { get { return alternatives; } }
        public TrainingService()  
        {
            weakPoints = new List<StudentsWeakPoint>();
            exercisesWithPaths = new List<ExerciseWithPaths>();

            foreach (var x in Program.MainOntology.FindAllExercises())
            {
                if (Program.MainOntology.CheckExercise(x, true))
                {
                    ExerciseWithPaths ex = new ExerciseWithPaths(x,
                        Program.MainOntology.FindConnectedClasses(x, "input")[0],
                        Program.MainOntology.FindConnectedClasses(x, "output")[0]);
                    ex.findAllPaths();
                    exercisesWithPaths.Add(ex);
                }
            }
        }
        public bool IsFinalAnswer()
        {
            return rightAnswerForStep == laststring;
        }
        public void rightAnswer()
        {
            int weakPointIndex = weakPoints.FindIndex(x => x.Method == curTaskMethod);
            if (weakPointIndex >= 0)
            {
                weakPoints[weakPointIndex].RightAnswer();
                if (weakPoints[weakPointIndex].IsFixed())
                {
                    weakPoints.RemoveAt(weakPointIndex);
                }
            }
        }


        public void wrongAnswer()
        {
            int weakPointIndex = weakPoints.FindIndex(x => x.Method == curTaskMethod);
            if (weakPointIndex >= 0)
            {
                weakPoints[weakPointIndex].WrongAnswer();

            }
            else
            {
                StudentsWeakPoint wp = new StudentsWeakPoint(curTaskMethod);
                weakPoints.Add(wp);
            }
        }

        public string nextStep( int step)
        {
            rightAnswerForStep = alternatives[indexOfNextStruct].Answers[step];
            curTaskMethod = alternatives[indexOfNextStruct].Path[step];
            string curStruct = alternatives[indexOfNextStruct].Structs[step];
            cleanPaths(step, curStruct);
            indexOfNextStruct = alternatives.FindIndex((x) => x.Structs[step] == curStruct);
            return curStruct;
        }
        public bool prepareExForSolve(string typeEx)
        {
            ExerciseWithPaths ex = exercisesWithPaths.Find((item) => item.Type == typeEx);
            
           alternatives = new List<PathWithSA>();
            firststring = ""; laststring = "";
            getAlternatives(ex);
            return (alternatives != null && alternatives.Count != 0);
          
        }
        private void getAlternatives(ExerciseWithPaths ex)
        {
            string input = ex.Input;
            string output = ex.Output;

            object paramInput = null, paramOut  = null;
            alternatives = new List<PathWithSA>();
            Type inputT, outputT;
            if (ex.StraightPaths.Count != 0 && ex.StraightPaths[0].Path.Count != 0)
            {
                
               inputT = Type.GetType("Test.boolean_function." + Program.FindTypeName(input));
                if (inputT == null)
                {
                    MessageBox.Show("Ошибка в типах данных!", "Ошибка");
                    return;
                }
                var Obj = (BFType)Activator.CreateInstance(inputT);
                paramInput = Obj.Generate(new List<object>());
                firststring = Obj.Print(paramInput);

                foreach(PathWithStructs p in ex.StraightPaths)
                {
                    List<string> answers = p.getAnswersForPath( ref paramInput, ref paramOut, ref firststring, ref laststring );
                    if (answers == null)
                        return;
                    List<string> curStructs = new List<string>();
                    curStructs.AddRange(p.Structs);
                    alternatives.Add(new PathWithSA(p.Path, curStructs, answers));
                }
               

            }
            if (ex.ReversedPaths.Count != 0 && ex.ReversedPaths[0].Path.Count != 0)
            {
                outputT = Type.GetType("Test.boolean_function." + Program.FindTypeName(output));
                if (outputT == null)
                {
                    MessageBox.Show("Ошибка в типах данных!", "Ошибка");
                    return;
                }
                if (laststring == "")
                {
                    var Obj = (BFType)Activator.CreateInstance(outputT);
                    paramOut = Obj.Generate(new List<object>());

                    laststring = Obj.Print(paramOut);
                }

               foreach(PathWithStructs p in ex.ReversedPaths)
                {
                    List<string> answers = p.getAnswersForPath(ref paramInput, ref paramOut, ref firststring, ref laststring);
                    if (answers == null)
                        return;
                    answers.Reverse();
                    answers.Add(laststring);
                    List<string> curStructs = new List<string>();
                    curStructs.AddRange(p.Structs);
                    curStructs.Reverse();
                    curStructs.Add(output);
                    curStructs.RemoveAt(0);
                    List<string> _path = new List<string>();
                    _path.AddRange(p.Path);
                    _path.Reverse();
                    alternatives.Add(new PathWithSA(_path, curStructs, answers));
                }
            }
       }
        private void cleanPaths( int curStep, string curStruct)
        {
            List<int> needToRemove = new List<int>();
            for (int i = 0; i < alternatives.Count; i++)
            {
                if (alternatives[i].Structs[curStep] != curStruct)
                {
                    needToRemove.Add(i);
                }
            }

            foreach (int del in needToRemove)
            {
                alternatives.RemoveAt(del);
            }
        }

        public List<ExerciseWithPaths> PickTasks()
        {
            if (weakPoints.Count == 0)
            {
                MessageBox.Show("Вам пока нечего посоветовать");
                return null;
            }
            StudentsWeakPoint wp = weakPoints[0];
            foreach (StudentsWeakPoint ans in weakPoints)
            {
                if (ans.Coeff > wp.Coeff)
                {
                    wp = ans;
                }
            }
            List<ExerciseWithPaths> tasksWithMethod = new List<ExerciseWithPaths>();
            foreach (ExerciseWithPaths ex in exercisesWithPaths)
            {
                ExerciseWithPaths tmp = new ExerciseWithPaths(ex.Type, ex.Input, ex.Output);
                foreach (PathWithStructs p in ex.StraightPaths)
                {
                    if (p.Path.Contains(wp.Method))
                    {
                        tmp.StraightPaths.Add(new PathWithStructs(p.Path, p.Structs, p.IsStraight));
                    }
                }
                foreach (PathWithStructs p in ex.ReversedPaths)
                {
                    if (p.Path.Contains(wp.Method))
                    {
                        tmp.ReversedPaths.Add(new PathWithStructs(p.Path, p.Structs, p.IsStraight));
                    }
                }

                if (tmp.StraightPaths.Count != 0 || tmp.ReversedPaths.Count != 0)
                {
                    tasksWithMethod.Add(tmp);
                }
            }
            return tasksWithMethod;
        }
    }
}

