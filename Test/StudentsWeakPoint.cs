using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    //Слабое место студента
    public class StudentsWeakPoint
    {
        //некоторый метод - этап решения
        string method;
        //коэффициент
        int coeff;

        public StudentsWeakPoint (string method)
        {
            this.method = method;
            this.coeff = 2;
        }

        public void WrongAnswer()
        {
           coeff++;
        }
        public void RightAnswer()
        {
            coeff--;
        }

        public bool IsFixed()
        {
            return coeff == 0;
        }

        public string Method { get { return method; } }
        public int Coeff { get { return coeff; } }

    }
}
