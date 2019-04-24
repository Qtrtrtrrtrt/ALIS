using System;
using System.Collections.Generic;

namespace Test
{
    public class PathWithSA : PathWithStructs
    {
        private List<string> answers;
        public List<string> Answers { get { return answers;  } }
        public PathWithSA(List<string> _path, List<string> _structs,  List<string> _answers): base(_path,_structs, true)
        {
            this.answers = new List<string>();
            this.answers.AddRange(_answers);
        }
    }
}
