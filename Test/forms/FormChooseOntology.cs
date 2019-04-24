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

namespace Test.forms
{
    public partial class FormChooseOntology : Form
    {
        public List<string> files;
        public FormChooseOntology(string [] files)
        {
            InitializeComponent();
            foreach(string  f in files) {
                listBoxOntologyFiles.Items.Add(Path.GetFileNameWithoutExtension(f));
            }
            this.files = new List<string>();
            this.files.AddRange(files);
             }
        private string ontologyName;
        public string OntologyName
        {
            get
            {
                return ontologyName;
            }
            set
            {
                ontologyName = value;
            }
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (listBoxOntologyFiles.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите онтологию");
                return;
            }
            ontologyName = files[listBoxOntologyFiles.SelectedIndex];
            Program.MainOntology = new Ontology(ontologyName);
            
            this.Close();
        }

        private void FormChooseOntology_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ontologyName == null)
            {
                DialogResult dialogResult = MessageBox.Show("Вы не выбрали онтологию. \n Программа завершит свою работу, вы точно хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                } else
                {
                    Environment.Exit(0);
                }
            }
           
        }

        private void FormChooseOntology_Load(object sender, EventArgs e)
        {

        }
    }
}
