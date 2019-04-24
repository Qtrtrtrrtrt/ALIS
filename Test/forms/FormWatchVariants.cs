using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test.forms
{
    public partial class FormWatchVariants : Form
    {
        Variant currentVariant;
        
       
        public FormWatchVariants()
        {
            InitializeComponent();
          
            //buttonDelete.Enabled = false;
          
            foreach (Variant number in Program.testsService.ActiveVariants)
            {
                comboBoxVar.Items.Add(number.Number.ToString());
            }
            comboBoxVar.SelectedIndex = 0;
            if (currentVariant.WithOpenAnswer)
            {
                listExercises.Columns.RemoveAt(listExercises.Columns.Count - 1);
            }
        }

        private void FormWatchVariants_Load(object sender, EventArgs e)
        {
        }

        public void UpdateListExercises()
        {
            listExercises.Items.Clear();
            int number = int.Parse(comboBoxVar.Text);
            currentVariant = Program.testsService.ActiveVariants.Find((item) => item.Number == number);
            int k = 1;
            foreach (var ex in currentVariant.Exercises)
                {
                    var item = new ListViewItem(k.ToString());
                    item.SubItems.Add(ex.Text);
                    item.Tag = ex.Type;
                    if (ex.Answers.Count > 1)
                    {
                    item.SubItems.Add(ex.RightAnswer.ToString());
                   
                        string answers = "";
                        for (int i = 0; i < ex.Answers.Count; i++)
                        {
                            answers += ex.Answers[i];
                            if (i != ex.Answers.Count - 1)
                            {
                                answers += "; ";
                            }
                        }
                    item.SubItems.Add(answers);
                }
                    else
                    {
                        item.SubItems.Add(ex.RightAnswer);
                    }
                    listExercises.Items.Add(item);
                    k++;
            }
             for (int i=0; i<listExercises.Columns.Count; i++)
            {
                listExercises.Columns[i].Width = -2;
            }
        }

        private void comboBoxVar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListExercises();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Program.testsService.removeVariant(currentVariant);
            Program.mainForm.needSaveVariants = true;
            foreach (Variant var in Program.testsService.ActiveVariants)
            {
                comboBoxVar.Items.Add(var.Number.ToString());
            }
            comboBoxVar.SelectedIndex = 0;
        }

      

        private void listExercises_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listExercises.SelectedItems.Count != 1)
                MessageBox.Show("Выберите одну задачу!");
            else
            {
                var f = new FormWarchExercise(currentVariant.Exercises[listExercises.SelectedIndices[0]], currentVariant.Number);
                f.ShowDialog();
                UpdateListExercises();
            }
        }
    }
    }

