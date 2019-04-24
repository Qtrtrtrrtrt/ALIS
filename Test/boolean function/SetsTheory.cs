using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.boolean_function
{
    public class CustomSet
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);
        private List<int> data = new List<int>();

        private List<int> generateSet(int n)
        {
            var res = new List<int>();
            while(res.Count != n)
            {
                var elem = rnd.Next(21);
                if (!res.Contains(elem))
                {
                    res.Add(elem);
                }
                
            }
            res.Sort();
            return res;
        }

        public CustomSet()
        {
            data = generateSet(rnd.Next(8)+2);
        }

        public CustomSet(int n)
        {
            data = generateSet(n);
        }

        public List<int> getSet()
        {
            List<int> d = new List<int>();
            d.AddRange(data);
            return d;
        }

        public void setSet(List<int> s)
        {
            data.AddRange(s);
        }

        public string getString()
        {
            return "{" + String.Join("; ", data) + "}";
        }

    }
}
