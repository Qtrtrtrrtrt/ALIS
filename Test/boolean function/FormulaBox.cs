using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test.boolean_function
{
    public partial class FormulaBox : UserControl
    {
        public override string Text
        {
            get { return textBox1.Text; }
        }

        public FormulaBox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tb = (Button) sender;
            var start = textBox1.SelectionStart;
            textBox1.Text = textBox1.Text.Insert(start, tb.Text[0].ToString());
            textBox1.SelectionStart = start + 1;
            textBox1.SelectionLength = 0;
            textBox1.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char) Keys.Back || 
                        e.KeyChar == '0' || e.KeyChar == '1' || 
                        e.KeyChar == '(' || e.KeyChar == ')') ;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var start = textBox1.SelectionStart;
            var end = start + textBox1.SelectionLength + 1;
            textBox1.Text = textBox1.Text.Insert(start, "(");
            textBox1.Text = textBox1.Text.Insert(end, ")");
            textBox1.SelectionStart = start + 1;
            textBox1.SelectionLength = 0;
            textBox1.Focus();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            var start = textBox1.SelectionStart;
            textBox1.Text = textBox1.Text.Insert(start, "⊕");
            textBox1.SelectionStart = start + 1;
            textBox1.SelectionLength = 0;
            textBox1.Focus();
        }
    }
}
