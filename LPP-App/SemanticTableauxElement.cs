using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    abstract class SemanticTableauxElement
    {
        //fields
        private SetOfProps setOfProp; //each element should be distinct
        private SemanticTableauxElement leftTableuxElement;
        private SemanticTableauxElement rightTableuxElement;
        private List<char> activeVariables;
        public int Id { get; set; }
        //get the index for the node
        public int GetIndex()
        {
            return this.Id;
        }

        //set the index for the node
        public void SetIndex(int id)
        {
            this.Id = id;
        }
        //get fields;
        public SetOfProps GetSetOfPropostions()
        {
            return this.setOfProp;
        }
        public List<char> GetActiveVariables()
        {
            return this.activeVariables;
        }
        public void SetActiveVariables(List<char> activeVars)
        {
            this.activeVariables = activeVars;
        }
        public SemanticTableauxElement GetLeftTableuxElement()
        {
            return this.leftTableuxElement;
        }
        public SemanticTableauxElement GetRightTableuxElement()
        {
            return this.rightTableuxElement;
        }

        //constructor
        //alpha element
        public SemanticTableauxElement(SemanticTableauxElement leftElement,SetOfProps set)
        {
            this.leftTableuxElement = leftElement;
            this.rightTableuxElement = null;
            this.setOfProp = set;
        }
        //beta element
        public SemanticTableauxElement(SemanticTableauxElement leftElement, SemanticTableauxElement rightElement,SetOfProps set, List<char> activeVars)
        {
            this.leftTableuxElement = leftElement;
            this.rightTableuxElement = rightElement;
            this.setOfProp = set;
            this.activeVariables = activeVars;
        }
        //finalized element
        public SemanticTableauxElement(SetOfProps set, List<char> activeVars)
        {
            this.leftTableuxElement = this.rightTableuxElement = null;
            this.setOfProp = set;
            this.activeVariables = activeVars;
        }
        //Delta Element+Alpha
        public SemanticTableauxElement(SemanticTableauxElement left, SetOfProps set,List<char> activeVars)
        {
            this.leftTableuxElement = left;
            this.rightTableuxElement = null;
            this.setOfProp = set;
            this.activeVariables = activeVars;
        }
        
        //methods
        //set index
        public void SetIndexForChildrenNode()
        {
            if (leftTableuxElement != null)
            {
                leftTableuxElement.SetIndex(2 * this.GetIndex() + 1);
                leftTableuxElement.SetIndexForChildrenNode();
            }
            if (rightTableuxElement != null)
            {
                rightTableuxElement.SetIndex(2 * this.GetIndex() + 2);
                rightTableuxElement.SetIndexForChildrenNode();
            }
        }

        //check if this is closed
        public bool IsClosed()
        {
            bool result = false;
            // if a set contains 2 contradicting props
            if(this.setOfProp.HasContradictProp())
            {
                result = true;
            }
            if (result == false)
            {   
                if (this.GetLeftTableuxElement() != null && this.GetRightTableuxElement() == null)
                {
                    result = this.GetLeftTableuxElement().IsClosed();
                }
                else if(this.GetLeftTableuxElement()!=null && this.GetRightTableuxElement() != null)
                {
                    result = this.GetLeftTableuxElement().IsClosed() && this.GetRightTableuxElement().IsClosed();
                }
            }
            return result;
        }

        //public List<string> GetString()
        //{
        //    List<string> props = new List<string>();
        //    foreach(Propostion p in this.setOfProp.GetSet())
        //    {
        //        props.Add(p.ConvertToInfixNotation(p.GetRootNode()));
        //    }
        //    return props;
        //}
        
        //to string
        public override string ToString()
        {
            string s = "";
            foreach (Proposition p in this.setOfProp.GetSet())
            {
                s += p.ConvertToInfixNotation(p.GetRootNode())+"\n";
            }
            return s;
        }
    }
}
