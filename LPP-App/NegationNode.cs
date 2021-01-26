using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class NegationNode : PropositionalNode
    {
        public NegationNode(char c, PropositionalNode left):base(c,left)
        {
            
        }

        public override char GetChar()
        {
            return '¬';
        }

        public override bool GetValue()
        {
            return (!this.GetLeftNode().GetValue());
        }

        public override string ToNand()
        {
            string nand = "%(" + this.GetLeftNode().ToNand() + "," + this.GetLeftNode().ToNand() + ")";
            return nand;
        }
    }
}
