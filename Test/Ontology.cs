using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Test
{
    [Serializable]
    public class LinkTypes
    {
        public string Node;
        public string Type;

        public LinkTypes(string node, string type)
        {
            Node = node;
            Type = type;
        }
    
    }

    [Serializable]
    public class LinkPath: AbstractExercise
    {

        private PathWithStructs path;
        public PathWithStructs Path { get { return path; } }
        public LinkPath(string typeEx, List<string> path, List<string> structs,  bool isStraight): base(typeEx)
        {
            this.path = new PathWithStructs(path, structs, isStraight);
        }

    }

    [Serializable]
    public class LinkBetweenOntologyClasses
    {
        public string NodeFrom;
        public string NodeTo;
        public string MethodName;
        public string Arc;

        public LinkBetweenOntologyClasses(string from, string to, string method)
        {
            Arc = from + " → " + to;
            NodeFrom = from;
            NodeTo = to;
            MethodName = method;
        }

        public LinkBetweenOntologyClasses(string arc, string method)
        {
            Arc = arc;
            var k = arc.Split('→');
            NodeFrom = k[0].Remove(k[0].Length - 1, 1);
            NodeTo = k[1].Remove(0, 1);
            MethodName = method;
        }
    }

    public class OntologyClass
    {
        public string Name { get; set; }
        public string Translit { get; set; }

        public List<string> ConnectedClasses { get; set; }

        public OntologyClass(string name, string translit, IEnumerable<string> connected)
        {
            Name = name;
            Translit = translit;
            ConnectedClasses = new List<string>();
            foreach (var s in connected)
            {
                ConnectedClasses.Add(s);
            }

        }

    }

    public class Ontology
    {
        private StreamReader _file;
        public string Name;
        public List<string> RelationsList { get; set; }
        public List<OntologyClass> ClassesList { get; set; }

        private readonly List<string> _classes;

        public Ontology(string filename)
        {
            _file = new StreamReader(filename);
            Name = Path.GetFileNameWithoutExtension(filename);
            RelationsList = FindAllRelations();
            ClassesList = FindAllClasses();
            RepairTranslitSubclasses();
            AddHasSubclass();
            
            _classes = new List<string>();
            foreach (var c in ClassesList)
            {
                _classes.Add(c.Name);
            }
        }
        public bool CheckExercise(string text, bool open)
        {
            var input = FindConnectedClasses(text, "input")[0];
            var output = FindConnectedClasses(text, "output")[0];
            var path = FindMethods(input, output);
            if ((path == null || path.Count == 0) && open)
                path = FindMethods(output, input);
            return path != null && path.Count != 0;
        }
        private List<string> FindAllRelations()
        {
            var res = new List<string>();
            _file.BaseStream.Position = 0;
            string line;
            while ((line = _file.ReadLine()) != null)
            {
                if (line.Contains("<owl:ObjectProperty"))
                {
                    line = _file.ReadLine();
                    if (line.Contains("<rdfs:label"))
                    {
                        var r = FindLabelName(line);
                        res.Add(r);
                    }
                    
                }
            }
            return res;
        }

        private List<OntologyClass> FindAllClasses()
        {
            var classes = new List<OntologyClass>();
            _file.BaseStream.Position = 0;
            var line = "";
            var currentclass = "";
            var currentclassTranslit = "";
            var currentconnectedclasses = new List<string>();

            var currentrelation = "";

            while ((line = _file.ReadLine()) != null)
            {
                if (line.Contains("<owl:Class"))
                {
                    var c = FindName(line);
                    if (currentclass != "")
                    {
                        classes.Add(new OntologyClass(currentclass, currentclassTranslit, currentconnectedclasses));
                    }
                    currentclassTranslit = c;
                    currentconnectedclasses.Clear();
                }
                if (line.Contains("<rdfs:subClassOf"))
                {
                    var c = FindName(line);
                    if (c != "")
                        currentconnectedclasses.Add("subClassOf:" + c);

                }
                if (line.Contains("<owl:onProperty"))
                {
                    currentrelation = FindName(line);
                }

                if (line.Contains("<owl:someValuesFrom"))
                {
                    var c = FindName(line);
                    if (c != "")
                        currentconnectedclasses.Add(currentrelation + ":" + c);
                }
                if (line.Contains("<rdfs:label"))
                {
                    var c = FindLabelName(line);
                    currentclass = c;
                }

            }

            classes.Add(new OntologyClass(currentclass, currentclassTranslit, currentconnectedclasses));
            return classes;
        }

        

        public List<string> FindPathBetween(string class1, string class2)
        {
            var n = ClassesList.Count;
            var q = new List<string>();
            q.Add(class1);
            var used = new bool[n];
            var d = new int[n];
            var p = new int[n];
            for (var i = 0; i < n; i++)
                used[i] = false;
            used[_classes.IndexOf(class1)] = true;
            p[_classes.IndexOf(class1)] = -1;
            while (q.Count != 0)
            {
                string cur = q.First();
                if (cur != class1) {
                    cur = cur.Split(':')[1];
                }
                int v =  _classes.IndexOf(cur);
                q.RemoveAt(0);

                foreach (var child in ClassesList[v].ConnectedClasses)
                {
                   
                        var i = _classes.IndexOf(child.Split(':')[1]);
                        if (!used[i])
                        {
                            q.Add(child);
                            used[i] = true;
                            d[i] = d[v] + 1;
                            p[i] = v;
                        }
                    }
                
            }


            var res = new List<string>();


            if (used[_classes.IndexOf(class2)])
            {

                for (var v = _classes.IndexOf(class2); v != -1; v = p[v])
                {
                    
                    res.Add(_classes[v]);
                    
                }
            }
          
            res.Reverse();
            return res;
        }
        
        public List<string> FindPathMethods(string class1, string class2)
        {
            var n = ClassesList.Count;
            var q = new List<int>();
            q.Add(_classes.IndexOf(class1));
            var used = new bool[n];
            var d = new int[n];
            var p = new int[n];
            for (var i = 0; i < n; i++)
                used[i] = false;
            used[_classes.IndexOf(class1)] = true;
            p[_classes.IndexOf(class1)] = -1;
            while (q.Count != 0)
            {
                var v = q.First();
                q.RemoveAt(0);

                foreach (var child in ClassesList[v].ConnectedClasses)
                {
                    if (child.Contains("method") || child.Contains("is_a"))
                    {
                        var i = _classes.IndexOf(child.Split(':')[1]);
                        if (!used[i])
                        {
                            q.Add(i);
                            used[i] = true;
                            d[i] = d[v] + 1;
                            p[i] = v;
                        }
                    }
                    
                }
            }


            var res = new List<string>();


            if (used[_classes.IndexOf(class2)])
            {

                for (var v = _classes.IndexOf(class2); v != -1; v = p[v])
                {
                    res.Add(_classes[v]);
                }
            }
            res.Reverse();
            return res;
        }
        public List<string> PathToStructs(List<string> list)
        {
            var res = new List<string>();
            for (var i = 1; i < list.Count; i++)
            {
                var c = FindOntologyClass(list[i - 1]);
                var flag = false;
                foreach (var x in c.ConnectedClasses)
                {
                    if (x.Contains(list[i]))
                    {

                        var m = x.Split(':');
                        if (m[0] == "method")
                        {
                            
                            res.Add(m[1]);
                            flag = true;
                            
                        }
                        if (m[0] == "is_a")
                        {
                            flag = true;
                        }

                    }
                }
                if (!flag)
                    return null;
            }
            return res;
        }
        public List<string> PathToMethods(List<string> list)
        {
            var res = new List<string>();
            for (var i = 1; i < list.Count; i++)
            {
                var c = FindOntologyClass(list[i - 1]);
                var flag = false;
                foreach (var x in c.ConnectedClasses)
                {
                    if (x.Contains(list[i]))
                    {

                        var m = x.Split(':');
                        if (m[0] == "method")
                        {
                            var arc = c.Name + " → " + x.Split(':')[1];
                            var l = Program.LinksList.Find(w => w.Arc == arc);
                            if (l != null)
                            {
                                res.Add(l.MethodName);
                                flag = true;
                            }
                            else
                            {
                                return null;
                            }

                        }
                        if (m[0] == "is_a")
                        {
                            flag = true;
                        }

                    }
                }
                if (!flag)
                    return null;
            }
            return res;
        }
        public List<string> FindMethods(string class1, string class2)
        {
            var list = FindPathMethods(class1, class2);
            return PathToMethods(list);

        }


        public List<string> FindConnectedClasses(string text, string relation)
        {
            var res = new List<string>();
            foreach (var c in ClassesList)
            {
                if (c.Name == text)
                {
                    var str = c.ConnectedClasses.Where(x => x.Contains(relation));
                    foreach (var s in str)
                    {
                        res.Add(s.Split(':')[1]);
                    }
                }
            }
            return res;
        }

        public List<string> FindAllExercises()
        {
            var res = new List<string>();
            foreach (var c in ClassesList)
            {
                foreach (var z in c.ConnectedClasses)
                {
                    if (z.Contains("задача"))
                    {
                        res.Add(c.Name);
                    }
                }
            }
            return res;
        }
        //находит все возможные варианты решения (от first до last)
        public List<List<string>> FindAllPaths (string first, string last)
        {
            List<List<string>> AllPaths = new List<List<string>>();
            int startV = _classes.IndexOf(first);
            Stack<int> st = new Stack<int>();
            Stack<int> predok = new Stack<int>();
            st.Push(startV);
          
            predok.Push(-1);
            List<string> path = new List<string>();
            bool NeedClearPath = false;
            while (st.Count > 0)
            {
                int cur_ind = st.Pop();
                int pred = predok.Pop();
                     
                if (NeedClearPath && pred != -1)
                {
                    while (path[path.Count - 1] != _classes[pred])
                    {
                       
                        path.RemoveAt(path.Count - 1);
                        
                    }
                    NeedClearPath = false;
                }
              
                path.Add(_classes[cur_ind]);
                
                if (_classes[cur_ind] == last)
                {
                    if (path.Count != 0)
                    {
                        List<string> newPath = new List<string>();
                        foreach (string p in path)
                        {
                            newPath.Add(p);
                        }
                        AllPaths.Add(newPath);
                    }
                   
                    NeedClearPath = true;
                    continue;
                }
                bool flagBad = true;
                foreach (string conn in ClassesList[cur_ind].ConnectedClasses)
                {
                    if (conn.Contains("method") || conn.Contains("is_a"))
                    {
                        flagBad = false;
                        var i = _classes.IndexOf(conn.Split(':')[1]);
                        if (path.Contains(_classes[i]))
                        {
                            //цикл

                            NeedClearPath = true;
                            continue;
                        }
                        st.Push(i);
                        predok.Push(cur_ind);
                    }
                        
                }
                if (flagBad)
                {
                    NeedClearPath = true;
                    continue;
                }
                
            }
            return AllPaths;
        }
        public OntologyClass FindOntologyClass(string name)
        {
            foreach (var c in ClassesList)
            {
                if (name == c.Name)
                    return c;
            }
            return null;
        }


        public void AddHasSubclass()
        {
            foreach (var c in ClassesList)
            {
                foreach (var x in c.ConnectedClasses)
                {
                    if (x.Contains("subClassOf") || x.Contains("is_a"))
                    {
                        var clas = FindOntologyClass(x.Split(':')[1]);
                        clas.ConnectedClasses.Add("hasSubclass:" + c.Name);

                    }
                }
            }
        }

        public void RepairTranslitSubclasses()
        {
            foreach (var c in ClassesList)
            {
                for (var i = 0; i < c.ConnectedClasses.Count; i++)
                {
                    var str = c.ConnectedClasses[i].Split(':');
                    var russianName = ClassesList.Find(cl => cl.Translit == str[1]);
                    if (russianName != null)
                    {
                        c.ConnectedClasses[i] = str[0] + ":" + russianName.Name;
                    }
                }
            }
        }

        private string FindName(string line)
        {
            var i1 = line.IndexOf('#');
            if (i1 == -1)
                return "";
            var i2 = line.IndexOf("__h__", i1);
            if (i2 == -1)
                return "";
            return line.Substring(i1 + 1, i2 - i1 - 1);
        }

        private string FindLabelName(string line)
        {
            var i1 = line.IndexOf('>');
            if (i1 == -1)
                return "";
            var i2 = line.IndexOf("<", i1);
            if (i2 == -1)
                return "";
            return line.Substring(i1 + 1, i2 - i1 - 1);
        }
    }
}
