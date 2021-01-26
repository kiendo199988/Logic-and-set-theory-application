using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class ImplicationNode : PropositionalNode
    {
        public ImplicationNode(char c, PropositionalNode left, PropositionalNode right):base(c,left,right)
        {

        }
        public override char GetChar()
        {
            return '→';
        }

        public override bool GetValue()
        {
            return (!this.GetLeftNode().GetValue() || this.GetRightNode().GetValue());
        }

        public override string ToNand()
        {
            string nand = "%(" + this.GetLeftNode().ToNand() + ",%(" + this.GetRightNode().ToNand() + "," 
                + this.GetRightNode().ToNand() + "))";
            return nand;
        }
    }
}
