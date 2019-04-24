using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ExerciseWithPaths : AbstractExercise
    {
        private string input;
        private string output;
        public string Input { get { return input; } }
        public string Output { get { return output; } }

        
        private List<PathWithStructs> straightPaths;
        private List<PathWithStructs> reversedPaths;
        public List<PathWithStructs> StraightPaths { get { return straightPaths; }}
        public List<PathWithStructs> ReversedPaths { get { return reversedPaths; } }

        public ExerciseWithPaths(string ex, string inp, string outp):base(ex)
         {
             input = inp;
             output = outp;
             straightPaths = new List<PathWithStructs>();
             reversedPaths = new List<PathWithStructs>();
        }
      
     
         public void findAllPaths()
         {
            LinkPath exWithRecommend = Program.RecommendedPaths.Find((y) => y.Type == this.Type);
            if (exWithRecommend != null)
            {
                if (exWithRecommend.Path.IsStraight)
                {
                    straightPaths.Add(exWithRecommend.Path);
                } else
                {
                    reversedPaths.Add(exWithRecommend.Path);
                }
               return;
            }
            List<List<string>> tmpS = Program.MainOntology.FindAllPaths(Input,Output);
             foreach (List<string> straight in tmpS)
             {
                straightPaths.Add(new PathWithStructs(
                        Program.MainOntology.PathToMethods(straight),
                        Program.MainOntology.PathToStructs(straight),
                        true
                    ));
             }
             List<List<string>> tmpR = Program.MainOntology.FindAllPaths(Output, Input);
             foreach (List<string> reverse in tmpR)
             {
                reversedPaths.Add(new PathWithStructs(
                        Program.MainOntology.PathToMethods(reverse),
                        Program.MainOntology.PathToStructs(reverse),
                        false
                    ));
             }
         }
         
    }
}
