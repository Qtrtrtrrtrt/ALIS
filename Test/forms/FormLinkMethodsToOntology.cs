using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Test.forms
{
    public partial class FormLinkMethodsToOntology : Form
    {
        private List<string> AllArcs;
        private List<string> NotUsedArcs;
        private List<string> NotUsedMethods;


        private List<string> methods; 
        
        public FormLinkMethodsToOntology()
        {
            InitializeComponent();

            methods = new List<string>();
            NotUsedArcs = new List<string>();
            AllArcs = new List<string>();
            foreach (var c in Program.MainOntology.ClassesList)
            {
                foreach (var x in c.ConnectedClasses)
                {
                    if (x.Contains("method:"))
                    {
                        var from = c.Name;
                        var to = x.Split(':')[1];
                        AllArcs.Add(from + " → " + to);
                        if (!Program.LinksList.Exists(ex=> ex.NodeFrom==from && ex.NodeTo==to))
                            NotUsedArcs.Add(from + " → " + to);
                    }
                }
            }
            UpdateLeftListBox();


            NotUsedMethods = MethodReflectInfo(new MethodsLibrary());
            UpdateRightListBox();

            UpdateLinked();
        }

        private void UpdateLeftListBox()
        {
            listBoxLinks.Items.Clear();
            foreach (var x in NotUsedArcs)
            {
                listBoxLinks.Items.Add(x);
            }
        }

        public  List<string> MethodReflectInfo<T>(T obj) where T : class
        {
            var res = new List<string>();

            var t = typeof(T);
            // Получаем коллекцию методов
            var MArr = t.GetMethods();
            
            // Вывести методы
            foreach (MethodInfo m in MArr)
            {
                
                if (m.IsPublic && !m.IsVirtual && m.Name!="GetType")
                {
                    //Console.Write(" --> " + m.ReturnType.Name + " \t" + m.Name + "(");
                    var str = m.ReturnType.Name.ToLower() + " " + m.Name + " (";
                    // Вывести параметры методов
                    ParameterInfo[] p = m.GetParameters();
                    for (int i = 0; i < p.Length; i++)
                    {
                        //Console.Write(p[i].ParameterType.Name + " " + p[i].Name);
                        str += p[i].ParameterType.Name + " " + p[i].Name;
                        if (i + 1 < p.Length)
                            //Console.Write(", ");
                            str += ", ";
                    }
                    // Console.Write(")\n");
                    str += ")\n";
                    methods.Add(str);
                    //if (!Program.LinksList.Exists(ex => ex.MethodName==str))
                        res.Add(str);
                }
                
            }
            return res;
        }

        private void UpdateRightListBox()
        {
            listBoxMethods.Items.Clear();
            foreach (var x in NotUsedMethods)
            {
                listBoxMethods.Items.Add(x);
            }
            
        }
        
        private void UpdateLinked()
        {
            listView.Items.Clear();
            foreach (var l in Program.LinksList)
            {
                var item = new ListViewItem(l.Arc);
                item.SubItems.Add(l.MethodName);

                var k = Program.MainOntology.FindOntologyClass(l.NodeFrom);

                if (k==null || !k.ConnectedClasses.Contains("method:" + l.NodeTo) || !methods.Contains(l.MethodName))
                {
                    item.BackColor = Color.Tomato;
                }


                
                listView.Items.Add(item);
            }
        }

        
        private void buttonLink_Click(object sender, EventArgs e)
        {
            if (listBoxLinks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите дугу!");
                return;
            }
            if (listBoxMethods.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите метод!");
                return;
            }

            var l = new LinkBetweenOntologyClasses(listBoxLinks.SelectedItem.ToString(),listBoxMethods.SelectedItem.ToString());
            NotUsedArcs.Remove(listBoxLinks.SelectedItem.ToString());
            UpdateLeftListBox();

            //NotUsedMethods.Remove(listBoxMethods.SelectedItem.ToString());
            //UpdateRightListBox();

            Program.LinksList.Add(l);
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
                if (AllArcs.Contains(item.Text))
                    NotUsedArcs.Add(item.Text);
                //if (methods.Contains(item.SubItems[1].Text))
                //    NotUsedMethods.Add(item.SubItems[1].Text);
                
                UpdateLeftListBox();
                UpdateRightListBox();

                Program.LinksList.RemoveAt(listView.SelectedIndices[0]);
                UpdateLinked();
            }

        }
    }
}
