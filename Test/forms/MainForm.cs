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
using Microsoft.Office.Interop.Word;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Test.forms;
using MySql.Data.MySqlClient;
using Test.boolean_function;

namespace Test
{
    public partial class MainForm : Form
    {
        private string _templateFileName;
        private string _variantsFileName;
        public bool needSaveTemplate;
        public bool needSaveVariants;
        
        public MainForm()
        {
            InitializeComponent();
            _variantsFileName = "";
            _templateFileName = "";
            Program.testsService = new WorkingWithTestsService();
            this.сохранитьToolStripMenuItem.Enabled = false;
            this.сохранитьКакToolStripMenuItem.Enabled = false;
            allOptionsForVariants(false);
        }

       

        public void UpdateListExercises()
        {
            listExercises.Items.Clear();
            var k = 1;
         
            if (Program.testsService.ActiveTemplate != null)
            {
                foreach (var ex in Program.testsService.ActiveTemplate.ExercisesT)
                {
                    var item = new ListViewItem(k.ToString());
                    item.SubItems.Add(ex.Key.Type);
                    string parameters = "";
                    foreach (object p in ex.Key.ExParams)
                    {
                        parameters += p + " ";
                    }
                    item.SubItems.Add(parameters);
                    item.SubItems.Add(ex.Value.ToString());
                    listExercises.Items.Add(item);
                    k++;
                }

                
            }
        }
        public void regenerateVariantsIfNeed()
        {
            if (Program.testsService.needToRegenerate())
            {
                DialogResult res = MessageBox.Show("Шаблон был изменен, хотите обновить открытые варианты?", "Шаблон теста обновлен", MessageBoxButtons.YesNo);
                if (DialogResult.No == res)
                {
                    allOptionsForVariants(false);
                    Program.testsService.CorrectTemplateOpened = false;
                }
                else
                {
                    Program.testsService.generateVariants(Program.testsService.ActiveVariants.Count);
                    needSaveVariants = true;
                    allOptionsForVariants(true);
                }
            }
        }
      
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listExercises.SelectedItems.Count == 0)
                MessageBox.Show("Не выбрана ни одна задача!");
            else
            {
                var res = MessageBox.Show("Вы уверены, что хотите удалить выбранные задачи?", "Удаление",
                    MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    foreach (ListViewItem item in listExercises.SelectedItems)
                    {        
                        Program.testsService.deleteExerciseFromTemplate(item.SubItems[1].Text);
                    }
                }
                needSaveTemplate = true;
                UpdateListExercises();
                //если открыты варианты, возможно их стоит тоже обновить в связи с изменением
                regenerateVariantsIfNeed();
            }
        }

        private bool AskSave()
        {
            if (needSaveTemplate && (_templateFileName != "" || (Program.testsService.ActiveTemplate != null)))
            {
                var res = MessageBox.Show("Сохранить текущий шаблон?", "", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    сохранитьToolStripMenuItem.PerformClick();
                    return true;
                }

                return res != DialogResult.Cancel; //пользователь нажал отмену - return false
            }
            return true;
        }
        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AskSave())
            {
                var f = new FormCreateTestTemplate();
                f.Show();
                _templateFileName = "";
         
                Program.testsService.clearVariants();
                UpdateListExercises();
            }
            this.сохранитьToolStripMenuItem.Enabled = true;
            this.сохранитьКакToolStripMenuItem.Enabled = true;

            needSaveTemplate = true;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //если пользователь нажал отмену, то открытие  не производится
            if (!AskSave())
                return;

            string path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();
            var dialog = new OpenFileDialog();
            dialog.Filter = "XML (*.xml)|*.xml";
            dialog.InitialDirectory = path + "\\шаблоны";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _templateFileName = dialog.FileName;
                
                LoadTestTemplate();
                this.сохранитьToolStripMenuItem.Enabled = true;
                this.сохранитьКакToolStripMenuItem.Enabled = true;

            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_templateFileName == "")
                сохранитьКакToolStripMenuItem.PerformClick();
            else
            {
                SaveTestTemplate();
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();
            var dialog = new SaveFileDialog();
            dialog.Filter = "XML (*.xml)|*.xml";
            dialog.InitialDirectory = path+"\\шаблоны";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _templateFileName = dialog.FileName;
                SaveTestTemplate();

            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AskSave())
            {
                this.Close();
                Program.loginForm.Close();
            }
               
        }


        private void SaveVariants()
        {
            Program.testsService.saveVariants(_variantsFileName);
            needSaveVariants = false;
            MessageBox.Show(_variantsFileName + " сохранен!", "Сохранение");
        }
        private void SaveTestTemplate()
        {
            Program.testsService.saveTemplate(_templateFileName);
            needSaveTemplate = false;
            MessageBox.Show(_templateFileName + " шаблон сохранен!", "Сохранение");
        }
     
        private void LoadVariants()
        {
            Program.testsService.loadVariants(_variantsFileName);
            if (!Program.testsService.isTemplateMatchVars())
            {
                findTemplateForVariants();
            }
            else
            {
                allOptionsForVariants(true);
            }
            needSaveVariants = false;
        }
        private void allOptionsForVariants (bool f)
        {
            сохранитьВариантыToolStripMenuItem.Enabled = f;
            сохранитьВариантыКакToolStripMenuItem.Enabled = f;
        }
        private void findTemplateForVariants()
        {
            string tmp = Program.testsService.findTemplateForVariants();
            if (tmp != "")
            {
                UpdateListExercises();
                MessageBox.Show("Варианты успешно загружены");
                allOptionsForVariants(true);
                _templateFileName = tmp;
            }
            else
            {
                allOptionsForVariants(false);

                MessageBox.Show("Не найден подходящий шаблон теста. Вы можете только просматривать варианты");
            }

        }
        private void LoadTestTemplate()
        {
            Program.testsService.loadTemplate(_templateFileName);
            UpdateListExercises();
         
            needSaveTemplate = false;
            if (Program.testsService.isTemplateMatchVars()) {
                allOptionsForVariants(true);
                
            } else
            {
                allOptionsForVariants(false);
                AskSaveVariants(false);
                Program.testsService.clearVariants();
                Program.testsService.VariantCountsForGen = 0;
            }
              
        }
      
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var formatter = new BinaryFormatter();
            
            var fStream = new FileStream(Program.MainOntology.Name + "_links.bin", FileMode.Create, FileAccess.Write, FileShare.None);
       
            formatter.Serialize(fStream, new List<object> { Program.LinksList, Program.TypesList, Program.RecommendedPaths });
           
            fStream.Close();

            if (!AskSave() || !AskSaveVariants(true))
            {
                e.Cancel = true;
            }
            else
            {
                Program.loginForm.Close();
            }
        }

        private void управлениеМетодамиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var f = new FormLinkMethodsToOntology();
            f.Show();
        }

        private void управлениеТипамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormLinkTypesToOntology();
            f.Show();
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.testsService.ActiveTemplate == null)
            {
                MessageBox.Show("Перед тем, как добавлять задания, надо создать шаблон теста");
                новыйToolStripMenuItem.PerformClick();
                return;
            }
            var f = new FormAddExercise();
            f.Show();
        }


        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
           if (AskSave())
             {
                 this.Hide();
                 _templateFileName = "";
                 Program.testsService.clearService();
                
                 UpdateListExercises();
                 Program.loginForm.Show();

             }
           
        }

        private void открытоеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormExamination(false);
            f.Show();
        }

        private void закрытоеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormExamination(true);
            f.Show();
        }

        private void скачатьРезультатыТестированияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("К сожалению эта функция не реализована. Нет подключения к бд");
        }


      
        private bool AskSaveVariants(bool needCancel)
        {
            if (!needSaveVariants || Program.testsService.ActiveVariants == null || Program.testsService.ActiveVariants.Count == 0)
            {
                return true;
            }
            DialogResult res = DialogResult.None;
            if (needCancel)
            {
                res = MessageBox.Show("Сохранить сгенерированные варианты?", "", MessageBoxButtons.YesNoCancel);
            }
            else
            {
                res = MessageBox.Show("Сохранить сгенерированные варианты?", "", MessageBoxButtons.YesNo);

            }
            if (res == DialogResult.Yes)
            {
                if (_variantsFileName == "")
                {
                    string path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();
                    var dialog = new SaveFileDialog();
                    dialog.Filter = "XML (*.xml)|*.xml";
                    dialog.InitialDirectory = path + "\\варианты";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _variantsFileName = dialog.FileName;
                        SaveVariants();

                    }
                } else
                {
                    SaveVariants();
                }
            }
            return res != DialogResult.Cancel;

        }
        private void генерацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _variantsFileName = "";
            if (AskSaveVariants(true))
            {
                if (Program.testsService.ActiveTemplate == null)
                {
                    MessageBox.Show("Шаблон теста еще не создан");
                    return;
                }
                if (Program.testsService.ActiveTemplate.ExercisesT.Count == 0)
                {
                    MessageBox.Show("В шаблоне теста нет ни одного задания");
                    return;
                }
              
               
                FormVariantsGeneration f = new FormVariantsGeneration();
                f.ShowDialog();
                if(Program.testsService.VariantCountsForGen == 0)
                {
                    Program.testsService.VariantCountsForGen = Program.testsService.ActiveVariants.Count;
                    return;
                }
                Program.testsService.generateVariants(Program.testsService.VariantCountsForGen);
                allOptionsForVariants(true);
                needSaveVariants = true;
                MessageBox.Show("Варианты успешно сгенерированы и готовы к просмотру");
            }
          
        }

        private void просмотрВариантовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.testsService.ActiveVariants == null || Program.testsService.ActiveVariants.Count== 0)
            {
                MessageBox.Show("Сначала откройте или сгенерируйте варианты");
                return;
            }
            var f = new FormWatchVariants();
            f.Show();
        }

        private void открытьВариантыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();
            var dialog = new OpenFileDialog();
            dialog.Filter = "XML (*.xml)|*.xml";
            dialog.InitialDirectory = path + "\\варианты";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
               _variantsFileName = dialog.FileName;
               LoadVariants();

            }
        }

        private void привязкаПутиКЗадачеToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var f = new FormLinkPathsToOntology();
            f.Show();
        }

        private void сохранитьВариантыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_variantsFileName == "")
            {
                сохранитьВариантыКакToolStripMenuItem.PerformClick();
            }
            else
            {
                SaveVariants();
            }
        }

        private void сохранитьВариантыКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();
            var dialog = new SaveFileDialog();
            dialog.Filter = "XML (*.xml)|*.xml";
            dialog.InitialDirectory = path + "\\варианты";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _variantsFileName = dialog.FileName;
                SaveVariants();

            }
        }

        private void распечататьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.testsService.ActiveVariants == null || Program.testsService.ActiveVariants.Count == 0)
            {
                MessageBox.Show("Сначала откройте или сгенерируйте варианты");
                return; 
            }
            Program.testsService.printVariants();
        }
    }
}
