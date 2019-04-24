using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.forms;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Test
{
    public static class Program
    {
        /// <summary>
        /// Главная форма приложения для препода
        /// </summary>
        public static MainForm mainForm;

        /// <summary>
        /// Главная форма приложения для студента
        /// </summary>
        public static StudentForm studentForm;

        public static FormUserAuth loginForm;

        /// <summary>
        /// Онтология
        /// </summary>
        public static Ontology MainOntology;

        /// <summary>
        /// Назначенные методы
        /// </summary>
        public static List<LinkBetweenOntologyClasses> LinksList;

        public static List<LinkTypes> TypesList;

        public static List<LinkPath> RecommendedPaths;

        public static TestingService testingService;

        public static WorkingWithTestsService testsService;


        public static string FindTypeName(string node)
        {
            foreach (var x in TypesList)
            {
                if (x.Node == node)
                    return x.Type;
            }
            return null;
        }

        private static void FindOntology(string filter)
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), filter);
            if (files.Length == 0)
            {
                throw new Exception("Онтология не найдена");
            }
            if (files.Length == 1)
            {
                MainOntology = new Ontology(files[0]);
               
            }
            else
            {
                FormChooseOntology f = new FormChooseOntology(files);
                //  f.Show();
                Application.Run(f);

            }
        }

        public static void ReadLinks()
        {
            var formatter = new BinaryFormatter();
            LinksList = new List<LinkBetweenOntologyClasses>();
            TypesList = new List<LinkTypes>();
            RecommendedPaths = new List<LinkPath>();

            var fStream = File.OpenRead(MainOntology.Name + "_links.bin");
            try
            {
                var res = (List<object>)formatter.Deserialize(fStream);
                LinksList = (List<LinkBetweenOntologyClasses>)res[0];
                TypesList = (List<LinkTypes>)res[1];
                if (res.Count > 2)
                {
                    RecommendedPaths = (List<LinkPath>)res[2];
                }
            }
            catch (Exception e)
            {
                if (! (e.Message.Contains("пустого") || e.Message.Contains("empty")))
                {
                    MessageBox.Show("Ошибка при открытии файла с настройками онтологии. Error: ", e.Message);
                }
            }
            finally
            {
                fStream.Close();
            }
        }
       
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Exerсises = new List<Exercise>();
            mainForm = new MainForm();
            studentForm = new StudentForm();
            try
            {
                FindOntology("*.owl");
            } catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            ReadLinks();

            loginForm = new FormUserAuth();
            Application.Run(loginForm);  
        }


    }
}
