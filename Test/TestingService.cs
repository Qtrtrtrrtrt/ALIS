using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public class TestingService
    {
        private int countGood = 0;
        private Variant currentVariant;
        private StudentResults results;
        public Variant CurrentVariant { get { return currentVariant; } }
      

        public TestingService(Variant variant)
        {
            currentVariant = variant;
            DateTime dateTime = DateTime.UtcNow.Date;
            results = new StudentResults(currentVariant.TestName, currentVariant.Number, dateTime.ToString("d"));
        }
        public void AddGoodResult (string text)
        {
            countGood++;
            results.AddResult(text, true);
        }

        public void AddBadResult(string text)
        {
            results.AddResult(text, false);
        }

        public void PrintResults()
        {
             MessageBox.Show("Правильных ответов: " + countGood + " из " + currentVariant.Exercises.Count);
            results.Print();
        }
        public void ClearService()
        {
            results = null;
            countGood = 0;
            currentVariant = null;
           
        }
    }
}
