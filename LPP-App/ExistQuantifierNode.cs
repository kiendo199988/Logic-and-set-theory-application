using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class ExistQuantifierNode : PropositionalNode
    {
        
        public ExistQuantifierNode(char c, ObjectVariableNode o, PropositionalNode p):base(c,o,p)
        {
            //this.objectVariableNode = o;
            this.SetBounded();
        }
        public override char GetChar()
        {
            return '!';
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
            return "!"+ this.ObjectVariableNode.GetChar();
        }

        //set the bounded variable for all obj variables
        public void SetBounded()
        {
           foreach(PredicateNode predicate in this.GetPredicates(this.GetLeftNode()))
           {
                predicate.SetIsbounded(this.ObjectVariableNode);
           }
        }
    }
}
