using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

namespace LPP_App
{
    class Proposition
    {
        public PropositionalNode RootNode { get; set; }
        public string Formula { get; set; }
        public List<char> ListOfVariables;

        public List<char> GetListOfVariables()
        {
            return this.ListOfVariables;
        }
        //constructor
        public Proposition(string formula)
        {
            this.Formula = formula;
            this.RootNode = this.BuildBinaryTree(Formula.ToList<char>());
            if (this.RootNode != null)
            {
                this.RootNode.SetIndex(0);
                RootNode.SetIndexForChildrenNode();
            }

            this.ListOfVariables = new List<char>();
            ListOfVariables = this.ExtractVariable(formula);
        }

        public Proposition(PropositionalNode node)
        {
            this.RootNode = node;
        }

        //get the root node of a tree
        public PropositionalNode GetRootNode()
        {
            return this.RootNode;
        }
        //check if a character is an operator
        public bool isOperator(char x)
        {
            switch (x)
            {
                case '>':
                case '=':
                case '&':
                case '|':
                    return true;
            }
            return false;
        }

        public void SetFormula(string f)
        {
            this.Formula = f;
        }
        public string Getformula()
        {
            return this.Formula;
        }

        //construct a binary tree from a list of chars (a string)
        public PropositionalNode BuildBinaryTree(List<char> formula)
        {
            if (formula.Count > 0)
            {
                if (formula[0] == '~')
                {
                    char nodeChar = formula[0];
                    formula.RemoveAt(0);
                    PropositionalNode leftNode = BuildBinaryTree(formula);
                    NegationNode n = new NegationNode(nodeChar, leftNode);
                    return n;
                }
                else if (formula[0] == '&')
                {
                    char nodeChar = formula[0];
                    formula.RemoveAt(0);
                    PropositionalNode leftNode = BuildBinaryTree(formula);
                    PropositionalNode rightNode = BuildBinaryTree(formula);
                    ConjunctionNode n = new ConjunctionNode(nodeChar, leftNode, rightNode);
                    return n;
                }
                else if (formula[0] == '|')
                {
                    char nodeChar = formula[0];
                    formula.RemoveAt(0);
                    PropositionalNode leftNode = BuildBinaryTree(formula);
                    PropositionalNode rightNode = BuildBinaryTree(formula);
                    DisjunctionNode n = new DisjunctionNode(nodeChar, leftNode, rightNode);
                    return n;
                }
                else if (formula[0] == '>')
                {
                    char nodeChar = formula[0];
                    formula.RemoveAt(0);
                    PropositionalNode leftNode = BuildBinaryTree(formula);
                    PropositionalNode rightNode = BuildBinaryTree(formula);
                    ImplicationNode n = new ImplicationNode(nodeChar, leftNode, rightNode);
                    return n;
                }
                else if (formula[0] == '=')
                {
                    char nodeChar = formula[0];
                    formula.RemoveAt(0);
                    PropositionalNode leftNode = BuildBinaryTree(formula);
                    PropositionalNode rightNode = BuildBinaryTree(formula);
                    BiImplicationNode n = new BiImplicationNode(nodeChar, leftNode, rightNode);
                    return n;
                }
                else if (char.IsUpper(formula[0]))
                {
                    if (formula.Count >= 4)
                    {
                        if (formula[1] == '(' && char.IsLower(formula[2]))
                        {
                            char predicate = formula[0];
                            formula.RemoveAt(0);
                            formula.RemoveAt(0);
                            List<ObjectVariableNode> objectVariables = new List<ObjectVariableNode>();
                            string s = new string(formula.ToArray());
                            char[] temp = s.ToCharArray();

                            for (int i = 0; i < temp.Length; i++)
                            {
                                if (temp[i] == ')')
                                {
                                    break;
                                }
                                if (Char.IsLower(temp[i]))
                                {
                                    ObjectVariableNode objVar = new ObjectVariableNode(temp[i]);
                                    objectVariables.Add(objVar);
                                }
                            }

                            PredicateNode predicateNode = new PredicateNode(predicate, objectVariables);
                            return predicateNode;
                        }
                        else
                        {
                            char nodeChar = formula[0];
                            VariableNode n = new VariableNode(nodeChar);
                            formula.RemoveAt(0);
                            return n;
                        }
                    }
                    else
                    {
                        char nodeChar = formula[0];
                        VariableNode n = new VariableNode(nodeChar);
                        formula.RemoveAt(0);
                        return n;
                    }
                   
                    
                }
                else if (formula[0] == '%')
                {
                    char nodeChar = formula[0];
                    formula.RemoveAt(0);
                    PropositionalNode leftNode = BuildBinaryTree(formula);
                    PropositionalNode rightNode = BuildBinaryTree(formula);
                    NandNode n = new NandNode(nodeChar, leftNode, rightNode);
                    return n;
                }
                else if (formula[0] == '1')
                {
                    char nodeChar = formula[0];
                    TrueNode n = new TrueNode(nodeChar);
                    formula.RemoveAt(0);
                    return n;
                }
                else if (formula[0] == '0')
                {
                    char nodeChar = formula[0];
                    FalseNode n = new FalseNode(nodeChar);
                    formula.RemoveAt(0);
                    return n;
                }
                else if (formula[0]== '!' && char.IsLower(formula[1]))
                {
                    char quantifier = formula[0];
                    ObjectVariableNode o = new ObjectVariableNode(formula[1]);
                    formula.RemoveAt(0);
                    formula.RemoveAt(0);
                    PropositionalNode left = BuildBinaryTree(formula);
                    ExistQuantifierNode node = new ExistQuantifierNode(quantifier, o, left);
                    return node;
                }
                else if (formula[0] == '@' && char.IsLower(formula[1]))
                {
                    char quantifier = formula[0];
                    ObjectVariableNode o = new ObjectVariableNode(formula[1]);
                    formula.RemoveAt(0);
                    formula.RemoveAt(0);
                    PropositionalNode left = BuildBinaryTree(formula);
                    UniversalQuantifierNode node = new UniversalQuantifierNode(quantifier, o, left);
                    return node;
                }
                else
                {
                    formula.RemoveAt(0);
                    return BuildBinaryTree(formula);
                }
            }
            else
            {
                return null;
            }
        }

        //convert a formula to infix notation
        public string ConvertToInfixNotation(PropositionalNode rootNode)
        {
            string infixNotation = "";

            if (rootNode != null)
            {
                if(rootNode is BiImplicationNode||rootNode is ImplicationNode|| rootNode is ConjunctionNode ||
                    rootNode is DisjunctionNode)
                {
                    infixNotation += "(" + ConvertToInfixNotation(rootNode.GetLeftNode());
                    infixNotation += rootNode.GetChar();
                    infixNotation += ConvertToInfixNotation(rootNode.GetRightNode()) + ")";
                }
                else if (rootNode is VariableNode || rootNode is TrueNode || rootNode is FalseNode)
                {
                    infixNotation += rootNode.GetChar();

                }
                else if (rootNode is NegationNode)
                {
                    infixNotation += rootNode.GetChar() + "(";
                    if (rootNode.GetLeftNode() != null)
                    {
                        infixNotation += ConvertToInfixNotation(rootNode.GetLeftNode()) + ")";
                    }

                }
                else if (rootNode is NandNode)
                {
                    infixNotation += "¬";
                    infixNotation += "(" + ConvertToInfixNotation(rootNode.GetLeftNode());
                    infixNotation += "Λ";
                    infixNotation += ConvertToInfixNotation(rootNode.GetRightNode()) + ")";
                }
                else if (rootNode is ExistQuantifierNode || rootNode is UniversalQuantifierNode)
                {
                    infixNotation += rootNode.GetString() + ".(" + this.ConvertToInfixNotation(rootNode.GetLeftNode())
                        + ")";
                }
                else if (rootNode is PredicateNode)
                {
                    infixNotation += rootNode.GetString();
                }
                else
                {
                    infixNotation += "";
                }
                return infixNotation;
            }
            else
            {
                return null;
            }

        }

        //extract the variables of a formula
        public List<char> ExtractVariable(string formula)
        {
            formula.Replace(" ", string.Empty);
            List<char> variables = new List<char>();
            //formula.ToList();
            foreach (char c in formula.ToList())
            {
                if ((char.IsUpper(c)) && !variables.Contains(c))
                {
                    variables.Add(c);
                }

            }
            return variables;
        }


        //setting up to build a tree image
        public void GenerateTreeImage()
        {
            GenerateTextFile();
            Process dot = new Process();

            dot.StartInfo.FileName = @"dot.exe";
            dot.StartInfo.Arguments = "-Tpng -obinarytree.png graph.dot";
            dot.Start();
            dot.WaitForExit();
        }

        //generate a text that helps to build a tree image
        public string WriteTextFileWithFormula(PropositionalNode currentNode)
        {
            string text = "";

            if(currentNode is PredicateNode || currentNode is ExistQuantifierNode || currentNode is UniversalQuantifierNode)
            {
                text += "node" + currentNode.GetIndex() + " [ label = \"" + currentNode.GetString() + "\" ]" + "\n";
            }
            else
            {
                text += "node" + currentNode.GetIndex() + " [ label = \"" + currentNode.GetPrefixChar() + "\" ]" + "\n";
            }
            if (currentNode.GetLeftNode() != null)
            {
                text += "node" + currentNode.GetIndex() + " -- " + "node" + currentNode.GetLeftNode().GetIndex() + "\n";
                text += WriteTextFileWithFormula(currentNode.GetLeftNode());
            }

            if (currentNode.GetRightNode() != null)
            {
                text += "node" + currentNode.GetIndex() + " -- " + "node" + currentNode.GetRightNode().GetIndex() + "\n";
                text += WriteTextFileWithFormula(currentNode.GetRightNode());
            }
            return text;
        }

        //generate a text file to create a tree image
        private void GenerateTextFile()
        {
            string s = "graph logic{node [ fontname = \"arial\"]" + "\n" + this.WriteTextFileWithFormula(this.RootNode) + "\n" + "}";
            try
            {
                using (StreamWriter sw = new StreamWriter("./graph.dot"))
                {

                    sw.WriteLine(s);
                }
            }
            catch
            {
                throw;
            }
        }

        //set values to tree leaves
        public void SetValuesToTheTreeLeaves(PropositionalNode propNode, IDictionary<char, bool> valuesDictionary)
        {
            if (propNode != null)
            {
                if (Char.IsUpper(propNode.GetPrefixChar()))
                {
                    propNode.SetVariableValue(valuesDictionary[propNode.GetPrefixChar()]);
                }
                else if (propNode.GetPrefixChar() == '1')
                {
                    propNode.SetVariableValue(true);
                }
                else if (propNode.GetPrefixChar() == '0')
                {
                    propNode.SetVariableValue(false);
                }
                else
                {
                    SetValuesToTheTreeLeaves(propNode.GetLeftNode(), valuesDictionary);
                    if (propNode.GetRightNode() != null)
                    {
                        SetValuesToTheTreeLeaves(propNode.GetRightNode(), valuesDictionary);
                    }
                }
            }
            
        }

        //get final result of a proposition based on the values of the variables
        public bool GetFinalResult(IDictionary<char, bool> valuesDictionary)
        {
            if (this.RootNode != null)
            {
                this.SetValuesToTheTreeLeaves(this.RootNode, valuesDictionary);
                return this.RootNode.GetValue();
            }
            else
            {
                return false;
            }
            
        }

        //get the nandified formula
        public string GetNandifiedFormula()
        {
            if(this.RootNode != null)
            {
                string s = this.RootNode.ToNand();
                return s;
            }
            else
            {
                return null;
            }
           
        }   


        //deep copy a proposition
        public Proposition DeepCopy()
        {
            Proposition p = new Proposition(this.RootNode.DeepCopy());
            return p;
        }

        //negate a prop
        public Proposition Negate()
        {
            this.RootNode = new NegationNode('~', this.RootNode);
            Proposition p = new Proposition(RootNode);
            return p;
        }

        //check if this prop is an alpha prop
        public bool IsAlpha()
        {
            bool result = false;
            if (this.RootNode != null)
            {
                if (this.RootNode is ConjunctionNode || this.RootNode is BiImplicationNode)
                {
                    result =  true;
                }
                else if (this.RootNode is NegationNode)
                {
                    if (this.RootNode.GetLeftNode() is DisjunctionNode || this.RootNode.GetLeftNode() is ImplicationNode
                        || this.RootNode.GetLeftNode() is NandNode)
                    {
                        result = true;
                    }                  
                }
            }
            return result;
        }
        
        //check if a proposition is a delta prop
        public bool IsDelta()
        {
            bool result = false;
            if (this.RootNode != null)
            {
                if (this.RootNode is ExistQuantifierNode)
                {
                    result = true;
                }
                else if (this.RootNode is NegationNode)
                {
                    if (this.RootNode.GetLeftNode() is UniversalQuantifierNode)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        //check if a prop is a beta prop
        public bool IsBeta()
        {
            bool result = false;
            if (this.RootNode != null)
            {
                if (this.RootNode is DisjunctionNode
                    || this.RootNode is ImplicationNode
                    || this.RootNode is NandNode)
                {
                    result = true;
                }
                else if (this.RootNode is NegationNode)
                {
                    if (this.RootNode.GetLeftNode() is ConjunctionNode || this.RootNode.GetLeftNode() is BiImplicationNode)
                    {
                        result = true;
                    }               
                }
            }
            return result;
        }
        

        //check if a prop is a gamma prop
        public bool IsGamma()
        {
            bool result = false;
            if (this.RootNode != null)
            {
                if (this.RootNode is UniversalQuantifierNode)
                {
                    result = true;
                }
                else if (this.RootNode is NegationNode)
                {
                    if (this.RootNode.GetLeftNode() is ExistQuantifierNode)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        //check if a prop is a double negation
        public bool IsDoubleNegation()
        {
            bool result = false;
            if (this.RootNode != null)
            {
                if (this.RootNode is NegationNode)
                {
                    if(this.RootNode.GetLeftNode() is NegationNode)
                    result = true;
                }
            }
            return result;
        }

        //return an double-negation-eliminated prop
        public Proposition EliminateDoubleNegation(Proposition p)
        {
            if (p.IsDoubleNegation())
            {
                p = new Proposition(p.RootNode.GetLeftNode().GetLeftNode());
                return this.EliminateDoubleNegation(p);
            }
            else
            {
                return p;
            }
        }


        //get children props from an alpha prop
        public List<Proposition> GetChildPropFromAlphaProp()
        {
            List<Proposition> temp = new List<Proposition>();
            List<Proposition> childrenAfterApplyDoubleRule = new List<Proposition>();
            if (this.IsAlpha() == true)
            {
                if (this.RootNode is ConjunctionNode )
                {
                    Proposition p1 = new Proposition(this.RootNode.GetLeftNode());
                    temp.Add(p1);
                    Proposition p2 = new Proposition(this.RootNode.GetRightNode());
                    temp.Add(p2);
                }
                else if(this.RootNode is NegationNode)
                {
                    if(this.RootNode.GetLeftNode() is DisjunctionNode)
                    {
                        Proposition p1 = new Proposition(this.RootNode.GetLeftNode().GetLeftNode());
                        Proposition p2 = new Proposition(this.RootNode.GetLeftNode().GetRightNode());
                        Proposition left = p1.DeepCopy().Negate();
                        Proposition right = p2.DeepCopy().Negate();
                        temp.Add(left);
                        temp.Add(right);
                    }
                    else if (this.RootNode.GetLeftNode() is ImplicationNode)
                    {
                        Proposition left = new Proposition(this.RootNode.GetLeftNode().GetLeftNode());
                        Proposition p2 = new Proposition(this.RootNode.GetLeftNode().GetRightNode());
                        Proposition right = p2.DeepCopy().Negate();
                        temp.Add(left);
                        temp.Add(right);
                    }
                    else if (this.RootNode.GetLeftNode() is NandNode)
                    {
                        Proposition p1 = new Proposition(this.RootNode.GetLeftNode().GetLeftNode());
                        Proposition p2 = new Proposition(this.RootNode.GetLeftNode().GetRightNode());
                        temp.Add(p1);
                        temp.Add(p2);
                    }
                }
                else if(this.RootNode is BiImplicationNode)
                {
                    //assume we have =(A,B)
                    
                    //we have disjunction1 = |((~A),B)
                    Proposition p1 = this.DeepCopy();
                    NegationNode left1 = new NegationNode('~', p1.GetRootNode().GetLeftNode());
                    PropositionalNode right1 = p1.GetRootNode().GetRightNode();
                    p1.RootNode = new DisjunctionNode('|', left1, right1);
                    //we have disjunction1 = |(A,(~B))
                    Proposition p2 = this.DeepCopy();
                    PropositionalNode left2 = p2.GetRootNode().GetLeftNode();
                    NegationNode right2 = new NegationNode('~', p2.GetRootNode().GetRightNode());
                    p2.RootNode = new DisjunctionNode('|', left2, right2);
                    
                    //add them to the list
                    temp.Add(p1);
                    temp.Add(p2);
                }
            }
            foreach(Proposition p in temp)
            {
                childrenAfterApplyDoubleRule.Add(this.EliminateDoubleNegation(p));
            }
            return childrenAfterApplyDoubleRule;
        }
   

        //get children props from a beta element
        public List<Proposition> GetChildPropFromBetaProp()
        {
            List<Proposition> children = new List<Proposition>();
            List<Proposition> childrenAfterApplyDoubleRule = new List<Proposition>();
            if (this.IsBeta() == true)
            {
                if (this.RootNode is DisjunctionNode)
                {
                    Proposition p1 = new Proposition(this.RootNode.GetLeftNode());
                    Proposition p2 = new Proposition(this.RootNode.GetRightNode());
                    children.Add(p1);
                    children.Add(p2);
                }
                else if (this.RootNode is ImplicationNode)
                {
                    Proposition p1 = new Proposition(this.RootNode.GetLeftNode());
                    Proposition right = new Proposition(this.RootNode.GetRightNode());
                    Proposition left = p1.Negate();
                    children.Add(left);
                    children.Add(right);
                }
                else if (this.RootNode is NegationNode)
                {              
                    if(this.RootNode.GetLeftNode() is ConjunctionNode)
                    {
                        Proposition p1 = new Proposition(this.RootNode.GetLeftNode().GetLeftNode());
                        Proposition p2 = new Proposition(this.RootNode.GetLeftNode().GetRightNode());
                        Proposition left = p1.Negate();
                        Proposition right = p2.Negate();
                        children.Add(left);
                        children.Add(right);
                    }                 
                    else if (this.RootNode.GetLeftNode() is BiImplicationNode)
                    {
                        //assume we have =(A,B)

                        //we have disjunction1 = &((~A),B)
                        Proposition p1 = this.DeepCopy();
                        NegationNode left1 = new NegationNode('~', p1.GetRootNode().GetLeftNode().GetLeftNode());
                        PropositionalNode right1 = p1.GetRootNode().GetLeftNode().GetRightNode();
                        p1.RootNode = new ConjunctionNode('&', left1, right1);

                        //we have disjunction1 = &((~A),B)
                        Proposition p2 = this.DeepCopy();
                        PropositionalNode left2 = p2.GetRootNode().GetLeftNode().GetLeftNode();
                        NegationNode right2 = new NegationNode('~', p2.GetRootNode().GetLeftNode().GetRightNode());
                        p2.RootNode = new ConjunctionNode('&', left2, right2);

                        //add them to the list
                        children.Add(p1);
                        children.Add(p2);
                    }

                }
                else if(this.RootNode is NandNode)
                {
                    Proposition p1 = new Proposition(this.RootNode.GetLeftNode());
                    Proposition p2 = new Proposition(this.RootNode.GetRightNode());
                    Proposition left = p1.Negate();
                    Proposition right = p2.Negate();
                    children.Add(left);
                    children.Add(right);
                }
            }

            foreach(Proposition p in children)
            {
                childrenAfterApplyDoubleRule.Add(this.EliminateDoubleNegation(p));
            }
            return childrenAfterApplyDoubleRule;
        }

        //get children props from a delta prop
        public Proposition GetChildPropFromDeltaProp(char c)
        {
            if (this.IsDelta() == true)
            {
                Proposition p = this.DeepCopy();

                if (p.GetRootNode() is ExistQuantifierNode)
                {
                    this.ChangeBoundedVariable(p.GetRootNode(), c);
                    Proposition delta = new Proposition(p.GetRootNode().GetLeftNode());
                    Proposition final = EliminateDoubleNegation(delta);
                   
                    return final;
                }               
                else if(p.GetRootNode() is NegationNode)
                {
                    if (p.GetRootNode().GetLeftNode() is UniversalQuantifierNode)
                    {
                        this.ChangeBoundedVariable(p.GetRootNode().GetLeftNode(), c);
                        Proposition delta = new Proposition(p.GetRootNode().GetLeftNode().GetLeftNode());
                        Proposition deltaNegated = delta.Negate();
                        Proposition final = EliminateDoubleNegation(deltaNegated);
                        return final;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }         
        }


        // get children props from a gamma prop
        public List<Proposition> GetChildPropFromGammaProp(List<char> objVars)
        {
            List<Proposition> gammaChildren = new List<Proposition>();
            if (this.IsGamma())
            {
                Proposition p = this.DeepCopy();
                if(p.GetRootNode() is UniversalQuantifierNode)
                {
                    for(int i=0;i<objVars.Count;i++)
                    {
                        PropositionalNode root = p.GetRootNode().DeepCopy();
                        this.ChangeBoundedVariable(root, objVars[i]);
                        Proposition child = new Proposition(root.GetLeftNode());        
                        gammaChildren.Add(child.EliminateDoubleNegation(child));
                    }
                }
                else if(p.GetRootNode() is NegationNode)
                {
                    if(p.GetRootNode().GetLeftNode() is ExistQuantifierNode)
                    {
                        for (int i = 0; i < objVars.Count; i++)
                        {
                            PropositionalNode root = p.GetRootNode().GetLeftNode().DeepCopy();
                            this.ChangeBoundedVariable(root, objVars[i]);
                            Proposition child = new Proposition(root.GetLeftNode());
                            Proposition finalChild = child.Negate();
                            gammaChildren.Add(finalChild.EliminateDoubleNegation(finalChild));
                        }
                    }
                }
            }
            return gammaChildren;
        }

        //method to check if a proposition is predicate formula
        public bool IsPredicateFormula()
        {
            return this.RootNode.HasPredicateLogicNode() && HasValidPredicate();
        }       


        //check if a prop has valid predicates
        public bool HasValidPredicate()
        {
            int nrOfInvalidPredicate = 0;
            List<PredicateNode> predicates = this.RootNode.GetPredicates(RootNode);
            if(predicates == null)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < predicates.Count; i++)
                {
                    for (int j = 0; j < predicates.Count; j++)
                    {
                        if (i != j)
                        {
                            if (predicates[i].CompareNrOfObjVars(predicates[j]))
                            {
                                nrOfInvalidPredicate++;
                            }
                        }
                    }
                }
                if (nrOfInvalidPredicate > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
        }
      

        //replace bounded variable with another formula
        public void ChangeBoundedVariable(PropositionalNode p,char c)
        {
            if (p.GetPredicates(p) != null && (p is UniversalQuantifierNode || p is ExistQuantifierNode))
            {
                foreach (PredicateNode n in p.GetPredicates(p))
                {
                    foreach (ObjectVariableNode objVar in n.GetObjectVariableNodes())
                    {
                        if (objVar.GetIsBounded() == true && objVar.GetChar() == p.GetObjectVariableNode().GetChar())
                        {
                            objVar.SetChar(c);
                        }
                    }
                }
            }
            
        }

        //check whether a predicate propositon has unbounded variables
        public bool HasUnboundedObjectVariables()
        {
            if (this.RootNode.HasPredicateLogicNode())
            {
                int unbounded = 0;
                foreach(PredicateNode n in this.RootNode.GetPredicates(this.RootNode))
                {
                    if (n.HasUnboundedVariables() == true)
                    {
                        unbounded++;
                    }
                }
                if (unbounded == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        //change appearences of an unbound variable
        //public Propostion ChangeAppearenceUnbound(char replace, char existing)
        //{
        //    Propostion p = this.DeepCopy();
        //    if (p.IsPredicateFormula())
        //    {
        //        if(p.GetRootNode() is ExistQuantifierNode || p.GetRootNode() is ForAllQuantifierNode)
        //        {
        //            List<PredicateNode> predicates = p.GetRootNode().GetPredicates(p.GetRootNode().GetLeftNode());
        //            foreach(PredicateNode n in predicates)
        //            {
        //                foreach(ObjectVariableNode objVar in n.GetObjectVariableNodes())
        //                {
        //                    if(objVar.GetIsBounded() == false && objVar.GetChar() == existing)
        //                    {                                               
        //                        objVar.SetChar(replace);                               
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return p;
        //}
    }
}

