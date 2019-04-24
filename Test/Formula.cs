using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
class FormulaTree
{
    public string Value;
    public List<FormulaTree> ChildsList;

    public FormulaTree(string value)
    {
        Value = value;
        ChildsList = new List<FormulaTree>();
    }

    public void AddChild(FormulaTree child)
    {
        ChildsList.Add(child);
    }

    public int Depth
    {
        get { return CountDepth(); }
    }

    private int CountDepth()
    {
        var maxChildD = 0;
        foreach (var subTree in ChildsList)
        {
            var childD = subTree.CountDepth();
            if (childD > maxChildD)
            {
                maxChildD = childD;
            }
        }
        return maxChildD + 1;
    }
}


    class Formula
    {
        public string Text;
        private FormulaTree _tree;
        private int _currentIndex;

        private readonly List<char> _addOperations = new List<char> { '⋁', '⊕' };
        private readonly List<char> _multOperations = new List<char> { '&' };

        private bool _wrongFormula;

        private List<char> _variables = new List<char>(); 

        public List<char> Variables
        {
            get
            {
                _variables.Sort();
                return _variables;
            }
            set { _variables = value; }
        }

        public int TreeDepth
        {
            get
            {
                if (_tree != null)
                {
                    return _tree.Depth;
                }
                return 0;
            }
        }

        public bool isWrong
        {
            get { return _wrongFormula; }
        }

        public int VariablesCount
        {
            get { return _variables.Count; }
        }

        public Formula(string text)
        {
            if (text.Length == 0)
                throw new Exception("Есть не введенные формулы");
            if (text[0] == '(' && text[text.Length - 1] == ')')
            {
                text = text.Substring(1, text.Length - 2);
            }
            Text = text.Replace(" ", "");

            _currentIndex = 0;
            _wrongFormula = false;
            _tree = ParseExpression();

        }

        private FormulaTree ParseExpression()
        {
            if (_currentIndex < Text.Length)
            {
                var tree = new FormulaTree("expression");
                tree.AddChild(ParseSummand());
                while (_currentIndex < Text.Length && _addOperations.Contains(Text[_currentIndex]))
                {
                    tree.AddChild(new FormulaTree(Text[_currentIndex].ToString()));
                    _currentIndex++;
                    tree.AddChild(ParseSummand());
                }
                if (_currentIndex < Text.Length && !_multOperations.Contains(Text[_currentIndex]) && Text[_currentIndex] != ')')
                {
                    _wrongFormula = true;
                    return null;
                }
                return tree;
            }
            _wrongFormula = true;
            return null;
        }

        private FormulaTree ParseSummand()
        {
            if (_currentIndex < Text.Length)
            {
                var tree = new FormulaTree("summand");
                tree.AddChild(ParseMultiplier());
                while (_currentIndex < Text.Length && _multOperations.Contains(Text[_currentIndex]))
                {
                    tree.AddChild(new FormulaTree(Text[_currentIndex].ToString()));
                    _currentIndex++;
                    tree.AddChild(ParseMultiplier());
                }
                if (_currentIndex < Text.Length && !_addOperations.Contains(Text[_currentIndex]) && Text[_currentIndex] != ')')
                {
                    _wrongFormula = true;
                    return null;
                }
                return tree;
            }
            _wrongFormula = true;
            return null;
        }

        private FormulaTree ParseMultiplier()
        {
            if (_currentIndex < Text.Length)
            {
                var tree = new FormulaTree("multiplier");
                if (Text[_currentIndex] == '¬')
                {
                    tree.AddChild(new FormulaTree("¬"));
                    _currentIndex++;
                }
                if (_currentIndex < Text.Length)
                {
                    if (Text[_currentIndex] == '(')
                    {
                        if (_currentIndex + 3 < Text.Length && Text[_currentIndex + 1] == '¬' &&
                            char.IsLetter(Text[_currentIndex + 2]) && Text[_currentIndex + 3] == ')')
                        {
                            _currentIndex++;
                            tree.AddChild(new FormulaTree("¬"));
                            _currentIndex++;
                            tree.AddChild(ParseVariable());
                            _currentIndex ++;
                        }
                        else
                        {
                            tree.AddChild(new FormulaTree("("));
                            _currentIndex++;
                            var expr = ParseExpression();
                            tree.AddChild(expr);
                            if (_currentIndex < Text.Length && Text[_currentIndex] == ')')
                            {
                                tree.AddChild(new FormulaTree(")"));
                                _currentIndex++;
                            }
                            else
                            {
                                _wrongFormula = true;
                                return null;
                            }
                        }   
                    }
                    else
                    {
                        if (Char.IsLetterOrDigit(Text[_currentIndex]))
                        {
                            if (Char.IsLetter(Text[_currentIndex]))
                            {
                                tree.AddChild(ParseVariable());
                            }
                            else
                            {
                                tree.AddChild(ParseConstant());
                            }
                        }
                        else
                        {
                            _wrongFormula = true;
                            return null;
                        }
                    }   
                }
                return tree;
            }
            _wrongFormula = true;
            return null;
        }

        private FormulaTree ParseVariable()
        {
            var tree = new FormulaTree("variable");
            var variable = Text[_currentIndex].ToString().ToLower();
            tree.AddChild(new FormulaTree(variable));
            if (!_variables.Contains(variable[0]))
            {
                _variables.Add(variable[0]);
            }
            _currentIndex++;
            return tree;
        }

        private FormulaTree ParseConstant()
        {
            var tree = new FormulaTree("constant");
            tree.AddChild(new FormulaTree(Text[_currentIndex].ToString()));
            _currentIndex++;
            return tree;
        }

        private bool HasConstants()
        {
            foreach (var ch in Text)
            {
                if (char.IsDigit(ch)) return true;
            }
            return false;
        }

        public bool CheckFormula(string formulaType)
        {
            if (_tree == null)
            {
                return false;
            }
            switch (formulaType)
            {
                case "СДНФ" :
                    if (TreeDepth != 5 || HasConstants())
                    {
                        return false;
                    }
                    if (_tree.Value == "expression" && _tree.ChildsList.Count > 0)
                    {
                        foreach (var summand in _tree.ChildsList)
                        {
                            if (summand.Value == "summand")
                            {
                                if (summand.ChildsList.Count != 2 * VariablesCount - 1)
                                    return false;
                            }
                            else
                            {
                                if (summand.Value != "⋁")
                                    return false;
                            }

                        }
                        return true;
                    }
                    return false;
                case "СКНФ":
                    if (TreeDepth != 8 || HasConstants())
                    {
                        return false;
                    }
                    if (_tree.Value == "expression" && _tree.ChildsList.Count == 1)
                    {
                        var summand = _tree.ChildsList[0];
                        if (summand.Value == "summand")
                        {
                            foreach (var multiplier in summand.ChildsList)
                            {
                                if (multiplier.ChildsList.Count == 3 && multiplier.ChildsList[1].Value == "expression")
                                {
                                    var expr = multiplier.ChildsList[1];
                                    if (expr.ChildsList.Count == 2*VariablesCount - 1)
                                    {
                                        foreach (var summand2 in expr.ChildsList)
                                        {
                                            if (summand2.Value == "summand")
                                            {
                                                if (summand2.ChildsList.Count != 1)
                                                    return false;
                                            }
                                            else
                                            {
                                                if (summand2.Value != "⋁")
                                                    return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
               
                                }
                            }
                            return true;
                        }          
                    }
                    return false;
                case "полином Жегалкина":
                    if (TreeDepth != 5)
                    {
                        return false;
                    }
                    if (_tree.Value == "expression" && _tree.ChildsList.Count > 0)
                    {
                        foreach (var summand in _tree.ChildsList)
                        {
                            if (summand.Value != "summand" && summand.Value != "⊕")
                            {
                               return false;
                            }
                        }
                        return true;
                    }
                    return false;
            }
            return isWrong;
        }

        public bool IsEqual(Formula formula)
        {
            if (isWrong || formula.isWrong) 
                return false;
            return IsEqualRec(_tree, formula._tree);
        }

        private bool IsEqualRec(FormulaTree tree1, FormulaTree tree2)
        {
            if (tree1.Depth != tree2.Depth) 
                return false;
            if (tree1.ChildsList.Count != tree2.ChildsList.Count)
                return false;
            if (tree1.Value != tree2.Value)
                return false;

            if (tree1.ChildsList.Count > 0)
            {
                var equal = true;
                foreach (var child in tree1.ChildsList)
                {
                    if (!EqualSubtreeFound(child, tree2.ChildsList))
                    {
                        equal = false;
                        break;
                    }
                }
                return equal;
            }
            return true;
        }

        private bool EqualSubtreeFound(FormulaTree tree, List<FormulaTree> childs)
        {
            var found = false;
            foreach (var child in childs)
            {
                if (IsEqualRec(tree, child))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

    }


}
