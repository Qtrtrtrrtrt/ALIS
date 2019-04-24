using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.boolean_function;

namespace Test.forms
{
    public partial class FormAddExercise : Form
    {
       
        private int parametersCount;
        public FormAddExercise()
        {
            InitializeComponent();
             
            foreach (var x in Program.MainOntology.FindAllExercises())
            {
                if (Program.MainOntology.CheckExercise(x, Program.testsService.ActiveTemplate.WithOpenAnswers))
                    listBox1.Items.Add(x);
            }
        }

        private void addParametersInputs(List<string> parameters)
        {
            var panels = new List<Panel>();
            mainPanel.Controls.Clear();
            var maxheight = 0;
            var maxwidth = 0;
            foreach (var param in parameters)
            {
                var panel = createParameterPanel(param);
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

        private Panel createParameterPanel(string param)
        {
            var panel = new Panel { BorderStyle = BorderStyle.FixedSingle, AutoSize = true };
            var position = 0;
            var label = new Label { Text = param, AutoSize = true, Location = new Point(5, position) };
            position += label.Height;
            panel.Controls.Add(label);

            var tbox = new TextBox { Width = 280 };
            tbox.Location = new Point(5, position);
            panel.Controls.Add(tbox);

            return panel;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Выберите тип задачи");
                return;
            }

            var task = listBox1.SelectedItem.ToString();
            List<object> paramValues = new List<object>();
            if (parametersCount > 0)
            {
                
                var panels = mainPanel.Controls;
                foreach (Panel panel in panels)
                {
                    var param = panel.Controls[0].Text;
                    var tbox = panel.Controls[1];

                    if (tbox.Text != "")
                    {
                        var typeName = Program.MainOntology.FindConnectedClasses(param, "type")[0];
                        var type = Type.GetType(typeName);
                        var value = Convert.ChangeType(tbox.Text, type);
                        paramValues.Add(value);
                    }
                }
            }
            Program.testsService.addNewExerciseTemplate(task, paramValues);
            MessageBox.Show("Задача добавлена!");
            Program.mainForm.needSaveTemplate = true;
            Program.mainForm.UpdateListExercises();
            Program.mainForm.regenerateVariantsIfNeed();

        }

    

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {       
            var task = listBox1.SelectedItem.ToString();
            List<string> parameters = Program.testsService.getParametersForExercise(task);
            if (parameters != null)
            {
                addParametersInputs(parameters);
                parametersCount = parameters.Count;
            }

        }
    }
}
