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

namespace Test.forms
{
    public partial class FormUserAuth : Form
    {
        public FormUserAuth()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            /*var cmd = new MySqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                Connection = Program.dataBaseConnection,
                CommandText = "authorization"
            };
            cmd.Parameters.AddWithValue("login", textBoxLogin.Text);
            cmd.Parameters.AddWithValue("passw", textBoxPassword.Text);
            var response = cmd.ExecuteReader();
            if (!response.HasRows)
            {
                MessageBox.Show("Неверный логин и/или пароль");
                response.Close();
                return;
            }
            response.Read();
            Program.AuthUserId = (int)response["id"];
            */
            if (radioStudent.Checked)
            {
                Program.studentForm.Show();
                this.Hide();
            }
            else
            {
                Program.mainForm.Show();
                this.Hide();
            }
            //response.Close();
        }

        private void FormUserAuth_VisibleChanged(object sender, EventArgs e)
        {
            textBoxLogin.Text = "";
            textBoxPassword.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var regForm = new FormUserRegistration();
            regForm.Show();
        }

        private void radioStudent_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
