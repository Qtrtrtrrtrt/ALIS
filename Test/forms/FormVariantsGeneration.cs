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
    public partial class FormVariantsGeneration : Form
    {
        public FormVariantsGeneration()
        {
            InitializeComponent();
            comboBoxCount.Text = "1";
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (comboBoxCount.Text == "")
            {
                MessageBox.Show("Введите или выберите из предложенного списка количество вариантов");
                return;
            }
            int counts;
            if (!int.TryParse(comboBoxCount.Text,out counts) || counts<=0)
            {
                MessageBox.Show("Количество вариантов должно быть целым положительным числом");
                return;
            }
            Program.testsService.VariantCountsForGen = counts;
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
