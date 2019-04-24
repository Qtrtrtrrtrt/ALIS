using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [Serializable]
    public abstract class AbstractExercise
    {
       
        public string Type { get; set ;  }
        public AbstractExercise() { }
        public AbstractExercise(string type)
            {
            this.Type = type;
            }
    }
}
