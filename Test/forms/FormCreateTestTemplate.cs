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
    public partial class FormCreateTestTemplate : Form
    {
        public FormCreateTestTemplate()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
          
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.textBoxName.Text == "" || (!this.radioButtonOpen.Checked && !this.radioButtonClosed.Checked))
            {
                MessageBox.Show("Выбраны не все параметры");
                return;
            }
            Program.testsService.setNewTemplate(this.radioButtonOpen.Checked, this.textBoxName.Text);

            Program.mainForm.Text = "ALIS - " + this.textBoxName.Text;
            
            this.Close();
        }
    }
}
