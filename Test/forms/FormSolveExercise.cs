using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Test.boolean_function;

namespace Test.forms
{
   
        
    public partial class FormSolveExercise : Form
    {
        private static TrainingService trainingService;
        private Random _rnd = new Random(DateTime.Now.Millisecond);
        private int _position = 30;
        private int _maxheight = 0;
        private int _maxwidth = 0;
     
        private bool _waitAnswer = true;
        private bool _waitChoice;

        public FormSolveExercise()
        {
            this.InitializeComponent();
            trainingService = new TrainingService();
            UpdateListExercises();
        }

        private void UpdateListExercises()
        {
            listBoxExercises.Items.Clear();
            foreach (ExerciseWithPaths ex in trainingService.ExercisesWithPaths)
            {
                listBoxExercises.Items.Add(ex.Type);
            }
        }
        private void BtnChoosePathOnClick(object sender, EventArgs eventArgs)
        {
            var panels = mainPanel.Controls;
            var panel = panels[panels.Count - 1];
            bool SomethingChecked = false;
            //цикл по radiobutton
            for (int i = 1; i < panel.Controls.Count && !SomethingChecked; i++)
            {
                RadioButton rb = (RadioButton)panel.Controls[i];
                if (rb.Checked)
                {
                    trainingService.IndexOfNextStruct = i - 1;
                    SomethingChecked = true;
                    _waitChoice = false;
                }
            }
            if (!SomethingChecked)
                MessageBox.Show("Не выбран ни один вариант");
        }
        private void createAskPathElement( int step)
        {
            var panel = new Panel { BorderStyle = BorderStyle.FixedSingle, AutoSize = false };
            var position = 0;
            var label = new Label { Text = "Что вы хотите найти дальше?", AutoSize = true, Location = new Point(5, position) };
            position += label.Height;
            panel.Controls.Add(label);
            List<string> uniqueAlternative = new List<string>();
            for (int i = 0; i < trainingService.Alternatives.Count; i++)
            {
                string structForStep = trainingService.Alternatives[i].Structs[step];
                if (!uniqueAlternative.Contains(structForStep))
                {
                    var radioButton = new RadioButton { Text = structForStep, AutoSize = true, Location = new Point(5, position) };
                    position += radioButton.Height;
                    panel.Controls.Add(radioButton);
                    uniqueAlternative.Add(structForStep);
                }

            }
            if (uniqueAlternative.Count == 1)
            {
                _waitChoice = false;
                trainingService.IndexOfNextStruct = 0;
                return;
            }
            var btn = new Button { Text = "ОК", Location = new Point(5, position) };
            btn.Click += BtnChoosePathOnClick;
            panel.Controls.Add(btn);
            panel.Height = panel.PreferredSize.Height;
            panel.Width = panel.PreferredSize.Width;
            panel.Location = new Point(5, _position);
            _position += panel.Height + 5;
            mainPanel.Invoke(new Action(() => mainPanel.Controls.Add(panel)));
        }
        private void createOpenQuestionElement(string type)
        {
            var panel = new Panel { BorderStyle = BorderStyle.FixedSingle, AutoSize = false };
            var position = 0;
            var label = new Label { Text = "Введите "+ type, AutoSize = true, Location = new Point(5, position) };
            position += label.Height;
            panel.Controls.Add(label);
            var classes = Program.MainOntology.FindConnectedClasses(type, "is_a");
            if (classes.Contains("формула"))
            {
                var fbox = new FormulaBox { AutoSize = true };
                fbox.Tag = type;
                fbox.Location = new Point(5, position);
                position += fbox.Height;
                panel.Controls.Add(fbox);
            }
            else
            {
                var tbox = new TextBox { AutoSize = true };
                tbox.Tag = "*";
                tbox.Location = new Point(5, position);
                position += tbox.Height;
                panel.Controls.Add(tbox);
            }
       
            var btn = new Button { Text = "ОК", Location = new Point(5, position) };
            btn.Click += BtnOnClick;
            panel.Controls.Add(btn);
            
            if (_maxheight < panel.PreferredSize.Height)
            {
                _maxheight = panel.PreferredSize.Height;
            }
            if (_maxwidth < panel.PreferredSize.Width)
            {
                _maxwidth = panel.PreferredSize.Width;
            }
            panel.Width = _maxwidth;
            panel.Height = _maxheight;
            panel.Location = new Point(5, _position);
            _position += _maxheight + 5;
            mainPanel.Invoke(new Action(() => mainPanel.Controls.Add(panel)));
           
        }

     
        private void rightAnswered(Control panel)
        {
            panel.BackColor = Color.LimeGreen;
            panel.Enabled = false;
            _waitAnswer = false;
            trainingService.rightAnswer();
        }
        private void wrongAnswered(Control panel)
        {
            panel.BackColor = Color.Red;
            MessageBox.Show("Ошибка! Попробуй еще раз!");
            trainingService.wrongAnswer();
        }
        private void BtnOnClick(object sender, EventArgs eventArgs)
        {
            var panels = mainPanel.Controls;
            var panel = panels[panels.Count - 1];
            var tbox = panel.Controls[1];
            if (tbox.Tag.ToString() == "*")
            {
                if (tbox.Text == trainingService.RightAnswerForStep)
                {
                    rightAnswered(panel);
                }
                else
                {
                    wrongAnswered(panel);
                }
            }
            else
            {
                var userAnswer = new Formula(tbox.Text);
                var rightAnswer = new Formula(trainingService.RightAnswerForStep);
                if (userAnswer.CheckFormula(tbox.Tag.ToString()))
                {
                    if (userAnswer.IsEqual(rightAnswer))
                    {
                        rightAnswered(panel);
                    }
                    else
                    {
                        wrongAnswered(panel);
                    }
                }
                else
                {
                    panel.BackColor = Color.Yellow;
                }
            }
        }

        private void PrepareExForSolve(string typeEx)
        {
            if (!trainingService.prepareExForSolve(typeEx))
                return;

            //то, что выше, оернуть в метод сервиса добавить в сервис firststring и laststring
            mainPanel.Invoke(new Action(() => mainPanel.Controls.Add(new Label() { Text = typeEx + " " +  trainingService.Firststring, AutoSize = true, Location = new Point(5, 5) })));
            //перенести alyernatives в сервис
            startSolveExercise();
          
        }
        private void startSolveExercise( )
        {
            int curStep = 0;
            while (!trainingService.IsFinalAnswer())
            {
                _waitChoice = true;
                createAskPathElement(curStep);
                while (_waitChoice)
                {
                        Thread.Sleep(100);
                }
                createOpenQuestionElement(trainingService.nextStep(curStep));
              
                while (_waitAnswer)
                {
                    Thread.Sleep(100);
                }
                _waitAnswer = true;


                curStep++;
            }
        }
        private Thread thread = null;
       

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBoxExercises.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Выберите тип задачи");
                return;
            }
            mainPanel.Controls.Clear();
            _position = 30;
            if (thread != null)
            {
                thread.Abort();
            }
            var task = listBoxExercises.SelectedItem.ToString();
            thread = new Thread(() => PrepareExForSolve( task));
            thread.Start();
        }

        private void FormSolveExercise_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
        }

        private void buttonAdvice_Click(object sender, EventArgs e)
        {
            List<ExerciseWithPaths> tasks;
            tasks = trainingService.PickTasks();
            if(tasks != null && tasks.Count != 0)
            {
                int rand_k = _rnd.Next(tasks.Count);
                if (rand_k == tasks.Count)
                {
                    rand_k--;
                }
                mainPanel.Controls.Clear();
                _position = 30;
                if (thread != null)
                {
                    thread.Abort();
                }
                thread = new Thread(() => PrepareExForSolve(tasks[rand_k].Type));
                thread.Start();
            }
        }
    }
}
