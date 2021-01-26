using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class UniversalQuantifierNode : PropositionalNode
    {
        
        public UniversalQuantifierNode(char c, ObjectVariableNode o, PropositionalNode p) :base(c, o, p)
        {
            //this.objectVariableNode = o;
            this.SetBounded();
        }
        public override char GetChar()
        {
            return '@';
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
            return "@"+this.ObjectVariableNode.GetChar();
        }

        public void SetBounded()
        {
            foreach (PredicateNode predicate in this.GetPredicates(this.GetLeftNode()))
            {
                predicate.SetIsbounded(this.ObjectVariableNode);
            }
        }
    }
}
