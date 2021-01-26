using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class BiImplicationNode:PropositionalNode
    {
        public BiImplicationNode(char c, PropositionalNode left, PropositionalNode right) : base(c, left, right)
        {

        }
        public override char GetChar()
        {
            //return '⇔';
            return '↔';
        }

        public override bool GetValue()
        {
            return (!this.GetLeftNode().GetValue() || this.GetRightNode().GetValue()) 
                && (!this.GetRightNode().GetValue() || this.GetLeftNode().GetValue());
        }

        public override string ToNand()
        {
            string nand = "%(%(%(" + this.GetLeftNode().ToNand() + "," + this.GetLeftNode().ToNand() + "),%("
                + this.GetRightNode().ToNand() + "," + this.GetRightNode().ToNand()
                + ")),%(" + this.GetLeftNode().ToNand() + "," + this.GetRightNode().ToNand() + "))";
            return nand;
        }
    }
}
