using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.boolean_function;

namespace Test.forms
{
    public partial class FormLinkTypesToOntology : Form
    {
        private List<string> AllTypes;
        private List<string> AllNodes;
        private List<string> NotUsedNodes; 
 
        public FormLinkTypesToOntology()
        {
            InitializeComponent();

            AllTypes = new List<string>();
            var ourtype = typeof(BFType); // Базовый тип

            var list = Assembly.GetAssembly(ourtype).GetTypes().Where(type => type.IsSubclassOf(ourtype));

            foreach (var itm in list)
            {
               AllTypes.Add(itm.Name);
               listBoxTypes.Items.Add(itm.Name);
            }

            AllNodes = new List<string>();
            NotUsedNodes = new List<string>();
            foreach (var c in Program.MainOntology.ClassesList)
            {
                foreach (var x in c.ConnectedClasses)
                {
                    if (x.Contains("method:"))
                    {
                        var from = c.Name;
                        var to = x.Split(':')[1];
                        if (!AllNodes.Contains(from))
                        {
                            AllNodes.Add(from);
                            if (!Program.TypesList.Exists(ex=>ex.Node==from))
                                NotUsedNodes.Add(from);
                        }

                        if (!AllNodes.Contains(to))
                        {
                            AllNodes.Add(to);
                            if (!Program.TypesList.Exists(ex => ex.Node == to))
                                NotUsedNodes.Add(to);
                        }
                    }
                }

                UpdateListBoxNodes();

                UpdateLinked();
            }
        }

        private void UpdateListBoxNodes()
        {
            listBoxNodes.Items.Clear();
            foreach (var x in NotUsedNodes)
            {
                listBoxNodes.Items.Add(x);
            }
        }

        private void UpdateLinked()
        {
            listView.Items.Clear();
            foreach (var l in Program.TypesList)
            {
                var item = new ListViewItem(l.Node);
                item.SubItems.Add(l.Type);

                if (!AllNodes.Contains(l.Node) || !AllTypes.Contains(l.Type))
                {
                    item.BackColor = Color.Tomato;
                }

                listView.Items.Add(item);
            }
        }

        private void buttonLink_Click(object sender, EventArgs e)
        {
            if (listBoxNodes.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите вершину!");
                return;
            }
            if (listBoxTypes.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите тип!");
                return;
            }

            var l = new LinkTypes(listBoxNodes.SelectedItem.ToString(), listBoxTypes.SelectedItem.ToString());
            NotUsedNodes.Remove(listBoxNodes.SelectedItem.ToString());
            UpdateListBoxNodes();

            Program.TypesList.Add(l);
            UpdateLinked();

        }

        private void buttonUnlink_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                return;
            }

            foreach (ListViewItem item in listView.SelectedItems)
            {
                if (AllNodes.Contains(item.Text))
                    NotUsedNodes.Add(item.Text);
                UpdateListBoxNodes();

                Program.TypesList.RemoveAt(listView.SelectedIndices[0]);
                UpdateLinked();
            }

        }
    }
}
