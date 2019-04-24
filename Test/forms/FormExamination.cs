using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Test.boolean_function;

namespace Test.forms
{
    public partial class FormExamination : Form
    {
      //  StudentResults results;
        public FormExamination(Boolean isClose)
        {
            InitializeComponent();
            
            var panels = new List<Panel>();
        //    DateTime dateTime = DateTime.UtcNow.Date;
          //  results = new StudentResults(Program.TestVariant.TestName, Program.TestVariant.Number, dateTime.ToString("d"));
            var maxheight = 0;
            var maxwidth = 0;
            foreach (var exercise in Program.testingService.CurrentVariant.Exercises)
            {
                Panel panel;
                if (isClose)
                {
                    panel = createCloseQuestionElement(exercise);
                } else
                {
                    panel = createOpenQuestionElement(exercise);
                }
                panels.Add(panel);
                if (maxheight < panel.PreferredSize.Height)
                {
                    maxheight = panel.PreferredSize.Height;
                }
                if (maxwidth < panel.PreferredSize.Width)
                {
                    maxwidth = panel.PreferredSize.Width;
                }
            }
            var position = 5;
            foreach (var panel in panels)
            {
                panel.AutoSize = false;
                panel.Width = maxwidth;
                panel.Height = maxheight;
                panel.Location = new Point(5, position);
                position += maxheight + 5;
                mainPanel.Controls.Add(panel);
            }
        }

        private Panel createOpenQuestionElement(Exercise exercise)
        {
            var panel = new Panel {BorderStyle = BorderStyle.FixedSingle, AutoSize = true };
            panel.Tag = exercise.RightAnswer;
            var position = 0;
            var label = new Label { Text = exercise.Text, AutoSize = true, Location = new Point(5, position) };
            position += label.Height;
            panel.Controls.Add(label);
            var outputNode = Program.MainOntology.FindConnectedClasses(exercise.Type.Replace('_', ' '), "output")[0];
            var classes = Program.MainOntology.FindConnectedClasses(outputNode, "is_a");
            if (classes.Contains("формула"))
            {
                var fbox = new FormulaBox {AutoSize = true};
                fbox.Tag = outputNode;
                fbox.Location = new Point(5, position);
                panel.Controls.Add(fbox);
            }
            else
            {
                var tbox = new TextBox { AutoSize = true };
                tbox.Tag = "right";
                tbox.Location = new Point(5, position);
                panel.Controls.Add(tbox);
            }
            return panel;
        }

        private Panel createCloseQuestionElement(Exercise exercise)
        {
            var panel = new Panel {BorderStyle = BorderStyle.FixedSingle, AutoSize = true};
            var position = 0;
            var label = new Label {Text = exercise.Text, AutoSize = true, Location = new Point(5, position)};
            position += label.Height;
            panel.Controls.Add(label);
            foreach (var answer in exercise.Answers)
            {
                var radio = new RadioButton {Text = answer, AutoSize = true};
                if (answer == exercise.RightAnswer)
                {
                    radio.Tag = "right";
                }
                radio.Location = new Point(5, position);
                position += radio.Height;
                panel.Controls.Add(radio);
            }
            return panel;
        }

        private int countRightClose()
        {
            var correct = 0;
            var panels = mainPanel.Controls;
            foreach (Panel panel in panels)
            {
                var answers = new Control[panel.Controls.Count];
                panel.Controls.CopyTo(answers, 0);
                for (var i = 1; i < answers.Length; i++)
                {
                    var answer = (RadioButton)answers[i];
                    if (answer.Checked)
                    {
                        if (answer.Tag.ToString() == "right")
                        {
                            panel.BackColor = Color.LimeGreen;
                            correct++;
                        }
                        else
                        {
                            panel.BackColor = Color.Red;
                        }
                    }
                }
            }
            return correct;
        }

     
        private void checkAnswers()
        {
            var panels = mainPanel.Controls;
            foreach (Panel panel in panels)
            {
                if (Program.testingService.CurrentVariant.WithOpenAnswer)
                {
                    var tbox = panel.Controls[1];
                    if (tbox.Text == "")
                        throw new Exception("Есть не введенные поля");
                   if(tbox.Tag == null)
                    {

                        var userAnswer = new Formula(tbox.Text);
                        var rightAnswer = new Formula(panel.Tag.ToString());
                        if (userAnswer.CheckFormula(tbox.Tag.ToString()))
                        {
                            if (userAnswer.IsEqual(rightAnswer))
                            {
                                Program.testingService.AddGoodResult(tbox.Text);
                                panel.BackColor = Color.LimeGreen;
                            }
                            else
                            {
                                Program.testingService.AddBadResult(tbox.Text);
                                panel.BackColor = Color.Red;
                            }
                        }
                        else
                        {
                            Program.testingService.AddBadResult(tbox.Text);
                            panel.BackColor = Color.Yellow;
                        }
                    }
                    else if (tbox.Tag.ToString() == "right")
                    {
                        if (tbox.Text == panel.Tag.ToString())
                        {
                           
                            Program.testingService.AddGoodResult(tbox.Text);
                            panel.BackColor = Color.LimeGreen;
                        }
                        else
                        {
                            Program.testingService.AddBadResult(tbox.Text);
                            panel.BackColor = Color.Red;
                        }
                    }
                   
                }
                else
                {
                    var answers = new Control[panel.Controls.Count];
                    panel.Controls.CopyTo(answers, 0);
                    for (var i = 1; i < answers.Length; i++)
                    {
                        var answer = (RadioButton)answers[i];
                        if (answer.Checked)
                        {
                            if(answer.Tag==null)
                            {
                                panel.BackColor = Color.Red;
                                Program.testingService.AddBadResult(answer.Text);
                            }
                            else if (answer.Tag.ToString() == "right")
                            {
                                panel.BackColor = Color.LimeGreen;
                                Program.testingService.AddGoodResult(answer.Text);
                            }
                            
                        }
                    }
                }

            }
       
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {  
            checkAnswers();
            Program.testingService.PrintResults();
            Program.testingService.ClearService();
            this.Close();
        }


        private bool IsAllAnswered()
        {
            var panels = mainPanel.Controls;
            foreach (Panel panel in panels)
            {
                var answers = new Control[panel.Controls.Count];
                panel.Controls.CopyTo(answers, 0);
                var isAnswered = false;
                for (var i = 1; i < answers.Length; i++)
                {
                
                    var answer = (RadioButton)answers[i];
                    if (answer.Checked)
                    {
                        isAnswered = true;
                    }
                }
                if (!isAnswered)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
