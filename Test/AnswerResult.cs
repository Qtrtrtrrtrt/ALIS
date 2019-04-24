using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class AnswerResult
    {
        string answer;
        bool correct;
        public AnswerResult (string answer, bool correct)
        {
            this.answer = answer;
            this.correct = correct;
        }
        public string Answer { get { return answer; } }
        public bool Correct { get { return correct; } }
    }
}
