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
    public partial class FormLinkPathsToOntology : Form
    {
        private List<ExerciseWithPaths> exercisesWithPaths;
   
        public FormLinkPathsToOntology()
        {
            InitializeComponent();

        
            exercisesWithPaths = new List<ExerciseWithPaths>();


            foreach (var x in Program.MainOntology.FindAllExercises())
            {
                if (Program.MainOntology.CheckExercise(x, true))
                {
                    if ( Program.RecommendedPaths.Find((y) => y.Type == x) == null)
                    {
                        listBoxExercises.Items.Add(x);
                    }
                    ExerciseWithPaths ex = new ExerciseWithPaths(x,
                           Program.MainOntology.FindConnectedClasses(x, "input")[0],
                           Program.MainOntology.FindConnectedClasses(x, "output")[0]);
                    ex.findAllPaths();
                
                    exercisesWithPaths.Add(ex);
                }
                UpdateRecemmendedView();
               
            }
            listBoxExercises.SelectedIndex = 0;
        }

        private void UpdateRecemmendedView()
        {
            listView.Items.Clear();
            if (Program.RecommendedPaths == null)
                return;
            foreach (LinkPath recEx in Program.RecommendedPaths)
            {
                string direction = "Прямой";
                if (!recEx.Path.IsStraight)
                    direction = "Обратный";
                var item = new ListViewItem(recEx.Type);
                item.SubItems.Add(PathToStr(recEx.Path.Structs));
                item.SubItems.Add(direction);
                listView.Items.Add(item);
            }
            // listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
           
        }
        private string PathToStr (List<string> path)
        {
            string res = "";
            for (int i=0; i< path.Count-1; i++)
                res += path[i] + " -> ";
            res += path[path.Count-1];
            return res;
        }
        private void UpdatePathsView()
        {
            listViewPaths.Items.Clear();
            ExerciseWithPaths ex = exercisesWithPaths.Find((y) => y.Type == (string)listBoxExercises.SelectedItem);
            foreach (PathWithStructs straight in ex.StraightPaths)
            {
                var item = new ListViewItem(PathToStr(straight.Structs));
                item.SubItems.Add("Прямой");
                listViewPaths.Items.Add(item);
            }
            foreach (PathWithStructs reverse in ex.ReversedPaths)
            {
                var item = new ListViewItem(PathToStr(reverse.Structs));
                item.SubItems.Add("Обратный");
                listViewPaths.Items.Add(item);
            }
           listViewPaths.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
           
        }
        private void listBoxExercises_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePathsView();
        }
        private void UpdateExerciseBox()
        {
            listBoxExercises.Items.Clear();
            foreach (ExerciseWithPaths ex in exercisesWithPaths)
            {
                if (Program.RecommendedPaths.Find((y) => y.Type == ex.Type) == null)
                {
                    listBoxExercises.Items.Add(ex.Type);
                }
            }
        }
     
        private void buttonLink_Click(object sender, EventArgs e)
        {
            if (listBoxExercises.SelectedIndex <0 || listViewPaths.SelectedItems.Count < 1)
            {
                MessageBox.Show("Для привязки вы должны выбрать задачу и путь");
                return;
            }
            string selectedExercise = (string)listBoxExercises.SelectedItem;
            string pathFromView = listViewPaths.SelectedItems[0].SubItems[0].Text;
            string direction = listViewPaths.SelectedItems[0].SubItems[1].Text;
            bool isStraight = true;
            if (direction == "Обратный")
                isStraight = false;
            ExerciseWithPaths ex = exercisesWithPaths.Find((y) => y.Type == selectedExercise);
            List<string> pathForLink = new List<string>() ;
            List<string> structsForLink = new List<string>();
            if (isStraight)
            {
                int index = ex.StraightPaths.FindIndex((y) => pathFromView == PathToStr(y.Structs));
                pathForLink.AddRange(ex.StraightPaths[index].Path);
                structsForLink.AddRange(ex.StraightPaths[index].Structs);
            }
            else
            {
                int index = ex.ReversedPaths.FindIndex((y) => pathFromView == PathToStr(y.Structs));
                pathForLink.AddRange(ex.ReversedPaths[index].Path);
                structsForLink.AddRange(ex.ReversedPaths[index].Structs);
            }
          
            LinkPath newLink = new LinkPath(ex.Type,pathForLink, structsForLink,isStraight);
            Program.RecommendedPaths.Add(newLink);
     
            var item = new ListViewItem(ex.Type);
            item.SubItems.Add(pathFromView);
            item.SubItems.Add(direction);
            listView.Items.Add(item);
            UpdateExerciseBox();
        }

        private void buttonUnlink_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count < 1)
            {
                MessageBox.Show("Выберете путь, который вы хотите отвязать");
                return;
            }
           
            string selectedExercise = listView.SelectedItems[0].SubItems[0].Text;
    
            ExerciseWithPaths ex = exercisesWithPaths.Find((y) => y.Type == selectedExercise);
            int index = Program.RecommendedPaths.FindIndex((y) => selectedExercise == y.Type);
            Program.RecommendedPaths.RemoveAt(index);

            UpdateRecemmendedView();
            UpdateExerciseBox();
        }
    }
}
