using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace LPP_App
{
    class SemanticTableaux
    {
        //field
        private SemanticTableauxElement rootTableuxElement;
        private SetOfProps set;
        private HashSet<Proposition> hashSet;
        private char[] activeVars = "abcdefghijklmno".ToCharArray();
       
        //get root element
        public SemanticTableauxElement GetRootTableuxElement()
        {
            return this.rootTableuxElement;
        }

        //constructor
        public SemanticTableaux(Proposition propostion)
        {
            hashSet = new HashSet<Proposition>();
            hashSet.Add(propostion);
            this.set = new SetOfProps(hashSet);          
            rootTableuxElement = this.GenerateTree(this.set, 0);
            if (this.rootTableuxElement != null)
            {
                this.rootTableuxElement.SetIndex(0);
                rootTableuxElement.SetIndexForChildrenNode();
            }
        }

        //check if the root is closed?
        public bool isContradiction()
        {
            return rootTableuxElement.IsClosed();
        }
               
       //binary tree of semantic tableux element
       public SemanticTableauxElement GenerateTree(SetOfProps set, int index)
       {
            int i = 0;
            if(set.HasContradictProp()== false)
            {
                if (index >= activeVars.Length)
                {
                    List<char> activeVariables = new List<char>();
                    for (int j = 0; j < index; j++)
                    {
                        activeVariables.Add(activeVars[j]);
                    }
                    FinalizedElement finalizedElement = new FinalizedElement(set, activeVariables);
                    return finalizedElement;
                }
                else
                {
                    if (set.CheckForAlphaProp() == true)
                    {
                        SetOfProps newSet = set.AlphaRuleForASetOfProp();
                        List<char> activeVariables = new List<char>();
                        for (int j = 0; j < index; j++)
                        {
                            activeVariables.Add(activeVars[j]);
                        }
                        SemanticTableauxElement left = this.GenerateTree(newSet, index);
                        AlphaElement alphaElement = new AlphaElement(left, set, activeVariables);
                        return alphaElement;
                    }
                    else if (set.CheckForDeltaProp())
                    {
                        SetOfProps newSet = set.DeltaRule(activeVars[index]);
                        List<char> activeVariables = new List<char>();
                        for (int j = 0; j < index; j++)
                        {
                            activeVariables.Add(activeVars[j]);
                        }
                        i = index + 1;
                        SemanticTableauxElement left = this.GenerateTree(newSet, i);
                        DeltaElement deltaElement = new DeltaElement(left, set, activeVariables);
                        return deltaElement;
                    }
                    else if (set.CheckForBetaProp() == true)
                    {
                        List<char> activeVariables = new List<char>();
                        for (int j = 0; j < index; j++)
                        {
                            activeVariables.Add(activeVars[j]);
                        }
                        List<SetOfProps> listOfSets = set.BetaRuleForASetOfProp();
                        SemanticTableauxElement left = this.GenerateTree(listOfSets.ElementAt(0), index);
                        SemanticTableauxElement right = this.GenerateTree(listOfSets.ElementAt(1), index);
                        BetaElement betaElement = new BetaElement(left, right, set, activeVariables);
                        return betaElement;
                    }
                    else if (set.CheckForGammaProp() == true)
                    {
                        List<char> activeVariables = new List<char>();
                        for (int j = 0; j < index; j++)
                        {
                            activeVariables.Add(activeVars[j]);
                        }
                        if (set.GammaRuleForASetOfProp(activeVariables) != null)
                        {
                            SetOfProps newSet = set.GammaRuleForASetOfProp(activeVariables);
                            SemanticTableauxElement left = this.GenerateTree(newSet, index);
                            GammaElement gammaElement = new GammaElement(left, set, activeVariables);
                            return gammaElement;
                        }
                        else
                        {
                            FinalizedElement finalizedElement = new FinalizedElement(set, activeVariables);
                            return finalizedElement;
                        }
                    }
                    else
                    {
                        List<char> activeVariables = new List<char>();
                        for (int j = 0; j < index; j++)
                        {
                            activeVariables.Add(activeVars[j]);
                        }
                        FinalizedElement finalizedElement = new FinalizedElement(set, activeVariables);
                        return finalizedElement;
                    }
                }
                
            }
            else
            {
                List<char> activeVariables = new List<char>();
                for (int j = 0; j < index; j++)
                {
                    activeVariables.Add(activeVars[j]);
                }
                FinalizedElement finalizedElement = new FinalizedElement(set, activeVariables);
                return finalizedElement;
            }
           
       }

        //setting up to build a tree image
        public void GenerateTreeImage()
        {
            GenerateTextFile();
            Process dot = new Process();

            dot.StartInfo.FileName = @"dot.exe";
            dot.StartInfo.Arguments = "-Tpng -osemanticTableux.png semanticTableux.dot";
            dot.Start();
            dot.WaitForExit();
        }

        //generate a text that helps to build a tree image
        public string WriteTextFileWithFormula(SemanticTableauxElement currentNode)
        {
            string text = "";
            if (currentNode.GetSetOfPropostions().HasContradictProp() == false)
            {
                if (currentNode.GetActiveVariables()!=null && currentNode.GetActiveVariables().Count>0)
                {
                    string s = "{ ";
                    foreach (char c in currentNode.GetActiveVariables())
                    {
                        if(c != currentNode.GetActiveVariables().Last())
                        {
                            s += c + ", ";
                        }
                        else
                        {
                            s += c;
                        }
                    }
                    s += " }";
                    text += "node" + currentNode.GetIndex() + " [ label = \"" + currentNode.ToString() + s + "\" ]" + "\n";
                }
                else
                {
                    text += "node" + currentNode.GetIndex() + " [ label = \"" + currentNode.ToString() + "\" ]" + "\n";
                }
            }          
            else
            {              
                text += "node" + currentNode.GetIndex() + " [ label = \"" + currentNode.ToString() + "X - Closed!" + "\" ]" + "\n";                
            }
            if (currentNode.GetLeftTableuxElement() != null)
            {
                text += "node" + currentNode.GetIndex() + " -- " + "node" + currentNode.GetLeftTableuxElement().GetIndex() + "\n";
                text += WriteTextFileWithFormula(currentNode.GetLeftTableuxElement());
            }

            if (currentNode.GetRightTableuxElement() != null)
            {
                text += "node" + currentNode.GetIndex() + " -- " + "node" + currentNode.GetRightTableuxElement().GetIndex() + "\n";
                text += WriteTextFileWithFormula(currentNode.GetRightTableuxElement());
            }
            return text;
        }

        //generate a text file to create a tree image
        private void GenerateTextFile()
        {
            string s = "graph logic{node [ fontname = \"arial\"]" + "\n" + this.WriteTextFileWithFormula(this.rootTableuxElement) + "\n" + "}";
            try
            {
                using (StreamWriter sw = new StreamWriter("./semanticTableux.dot"))
                {

                    sw.WriteLine(s);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
