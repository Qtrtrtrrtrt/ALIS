using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class StudentResults
    {
        string testName;
        int variantNumber;
        string date;
        List<AnswerResult> results;
        public StudentResults (string testName, int variantNumber, string date)
        {
            this.testName = testName;
            this.date = date;
            this.variantNumber = variantNumber;
            results = new List<AnswerResult>();
        }
         public void AddResult(string answer, bool correct)
        {
            results.Add(new AnswerResult(answer, correct));
        }
        public void Print()
        {
            var app = new Microsoft.Office.Interop.Word.Application
            {
                Visible = true
            };
            var doc = app.Documents.Add();
            var k = 1;
            doc.Content.Text += "Тест: " + testName;
            doc.Content.Text += "Вариант " + variantNumber;
            doc.Content.Text += "Дата: " + date + "\n";
            int all = 0;
            int cor = 0;
            foreach (AnswerResult res in results)
            {
                all++;
                if (res.Correct)
                    cor++;
                doc.Content.Text += (k++) + ") " + res.Answer + (res.Correct ? "  +" : "  -");
            }
            doc.Content.Text += "Правильных ответов " + cor.ToString() + " из " + all.ToString();
        }
    }
}
