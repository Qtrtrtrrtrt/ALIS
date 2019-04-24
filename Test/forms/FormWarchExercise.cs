using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class FormWarchExercise : Form
    {
        private Exercise exerсise;
        private int variantNumber;
        

        public FormWarchExercise(Exercise ex, int variantNumber)
        {
            InitializeComponent();
            exerсise = ex;
            
            buttonRegenerate.Enabled = Program.testsService.CorrectTemplateOpened;
            this.variantNumber = variantNumber;
            ShowEx(ex);
        }

        private void ShowEx(Exercise ex)
        {
            if (ex.Answers.Count > 1)
            {
                richTextBox1.Text = ex.Print(ExerciseType.MultipleChoiceWithAnswer);
            }
            else
            {
                richTextBox1.Text = ex.Print(ExerciseType.TestWithoutChoicesForTeacher);
            }
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRegenerate_Click(object sender, EventArgs e)
        {
            Program.testsService.regenerateExerciseInVariant(variantNumber, exerсise);
            this.Close();
        }
    }
}
