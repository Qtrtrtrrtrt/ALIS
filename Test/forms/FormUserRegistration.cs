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
    public partial class FormUserRegistration : Form
    {
        public FormUserRegistration()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!radioButtonStudent.Checked && !radioButtonTeacher.Checked)
            {
                MessageBox.Show("Выберите тип пользователя");
                return;
            }
            if (textBoxLogin.Text == "")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (textBoxPassword.Text == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            if (textBoxPassword.Text == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }


            var role = 1;
            if (radioButtonStudent.Checked)
            {
                role = 2;
            }

         /*   var cmd = new MySqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                Connection = Program.dataBaseConnection,
                CommandText = "registration"
            };
            
            cmd.Parameters.AddWithValue("login", textBoxLogin.Text);
            cmd.Parameters.AddWithValue("passw", textBoxPassword.Text);
            cmd.Parameters.AddWithValue("role", role);

            var exception = false;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                exception = true;
                MessageBox.Show("Пользователь с таким логином уже существует");
            }
            */
           // if (!exception)
           // {
                this.Close();
           // }

        }

        private void textBoxPassword2_TextChanged(object sender, EventArgs e)
        {
             labelError.Visible = textBoxPassword2.Text != textBoxPassword.Text;
            
        }

    }
}
