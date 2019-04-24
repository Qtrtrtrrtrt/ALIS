using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Test.forms;

namespace Test
{
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
          InitializeComponent();
             
        }


        public void UpdateListExercises()
        {
            listExercises.Items.Clear();
            var k = 1;
            if (Program.testingService == null || Program.testingService.CurrentVariant == null)
            {
                return;
            }
            foreach (var ex in Program.testingService.CurrentVariant.Exercises)
            {
                var item = new ListViewItem(k.ToString());
                item.SubItems.Add(ex.Text);
                listExercises.Items.Add(item);
                k++;
            }
        }
      
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormChooseTest();
            form.Show();
        }
  
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
                this.Close();
                Program.loginForm.Close();
        }  

      

        private void начатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Program.testingService == null || Program.testingService.CurrentVariant == null)
            {
                MessageBox.Show("Сначала откройте вариант");
                return;
            }
            var f = new FormExamination(!Program.testingService.CurrentVariant.WithOpenAnswer);
            f.Show();
        }

        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {       
                this.Hide();        
                UpdateListExercises();
                Program.loginForm.Show();          
        }

        private void StudentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.loginForm.Close();
        }

        private void обучениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormSolveExercise();
            f.Show();
        }

       
    }
}
