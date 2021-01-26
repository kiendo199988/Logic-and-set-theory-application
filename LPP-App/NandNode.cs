using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class NandNode:PropositionalNode
    {
        public NandNode(char c, PropositionalNode left, PropositionalNode right) : base(c, left, right)
        {

        }
        public override char GetChar()
        {
            return '%';
        }

        public override bool GetValue()
        {
            return !(this.GetLeftNode().GetValue() & this.GetRightNode().GetValue());
        }

        public override string ToNand()
        {
            return "%("+this.GetLeftNode().ToNand()+","+this.GetRightNode().ToNand()+")";
        }
    }
}
