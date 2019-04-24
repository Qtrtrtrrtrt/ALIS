using MySql.Data.MySqlClient;
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
using System.Xml.Serialization;

namespace Test.forms
{
    public struct TestInfo
    {
        public int id;
        public string path;
        public string name;

        public TestInfo(int id, string path, string name)
        {
            this.id = id;
            this.path = path;
            this.name = name;
        }
    }

    public partial class FormChooseTest : Form
    {

        private List<TestInfo> tests = new List<TestInfo>();
        private List<Variant> curVars;

        
        public FormChooseTest()
        {
            InitializeComponent();


            string[] pathes = Directory.GetFiles
                (Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString() + "\\варианты", "*.xml");
            for (int i = 0; i < pathes.Length; i++)
            {
                string t_name = System.IO.Path.GetFileNameWithoutExtension(pathes[i]);
                var test = new TestInfo(i + 1, pathes[i], t_name);
                tests.Add(test);
                var item = test.id + ": ";
                if (test.name == "")
                {
                    item += test.path;
                }
                else
                {
                    item += test.name;
                }
                listBox1.Items.Add(item);
            }
            if (pathes.Length == 0)
                MessageBox.Show("Нет новых тестов");
            //  response.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (curVars == null || comboBoxVariants.Text == "")
            {
                MessageBox.Show("Выберите тест и вариант");
                return;
            }
            Program.testingService = new TestingService(curVars.Find((x) => x.Number == int.Parse(comboBoxVariants.Text)));
            Program.studentForm.UpdateListExercises();
            Close();
        }
        private void UpdateComboBox()
        {
            comboBoxVariants.Items.Clear();
            foreach (var v in curVars)
            {
                comboBoxVariants.Items.Add(v.Number);
            }
            comboBoxVariants.SelectedIndex = 0;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count != 0)
            {
                var selected = listBox1.SelectedItem.ToString().Split(':');
                var test = tests.Find(x => x.id.ToString() == selected[0]);
                try
                {
                  //  Program.studentForm.SelectedTest = test;
                    var formatter = new XmlSerializer(typeof(List<Variant>));
                    var fStream = File.OpenRead(test.path);
                    curVars = (List<Variant>)formatter.Deserialize(fStream);
                    fStream.Close();
                    UpdateComboBox();

                }

                catch (Exception exc)
                {
                    MessageBox.Show("Ошибка: " + exc.Message);
                }
            }

        }
    }
}
