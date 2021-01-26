using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class PredicateNode : PropositionalNode
    {
        private List<ObjectVariableNode> objectVariableNodes;
        public PredicateNode(char c, List<ObjectVariableNode> objectVariables):base(c,objectVariables)
        {
            this.objectVariableNodes = objectVariables;
        }

        public List<ObjectVariableNode> GetObjectVariableNodes()
        {
            return this.objectVariableNodes;
        }
        public override char GetChar()
        {
            return this.Character;
        }

        public override bool GetValue()
        {
            throw new NotImplementedException();
        }

        public override string ToNand()
        {
            throw new NotImplementedException();
        }

        
        public override string GetString()
        {
            string s = "";
            s += this.Character + "(";
            foreach(ObjectVariableNode n in this.objectVariableNodes)
            {
                if (n != objectVariableNodes.Last())
                {
                    s += n.GetChar();
                    s += ",";
                }
                else
                {
                    s += n.GetChar() + ")";
                }
            }
            return s;
        }

        //compare number of obj vars between 2 predicate symbols
        public bool CompareNrOfObjVars(PredicateNode node)
        {
            bool result = false;
            if (this.Character == node.GetChar())
            {
                if (this.GetObjectVariableNodes().Count != node.GetObjectVariableNodes().Count)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        //set the isBounded to the object var
        public void SetIsbounded(ObjectVariableNode boundedVar)
        {
            foreach(ObjectVariableNode obj in this.objectVariableNodes)
            {
                if (obj.GetChar() == boundedVar.GetChar())
                {
                    obj.SetBound(true);
                }
            }
        }

        //check if a predicate has unbounded variables
        public bool HasUnboundedVariables()
        {
            if (this is PredicateNode)
            {
                int unbounded = 0;
                foreach (ObjectVariableNode objectVariable in this.GetObjectVariableNodes())
                {
                    if (objectVariable.GetIsBounded() == false)
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

    }
}
