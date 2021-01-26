using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class FalseNode : PropositionalNode
    {
        private bool falseConstant = true;
        public FalseNode(char c) : base(c)
        {

        }
        public override char GetChar()
        {
            return '0';
        }

        public override bool GetValue()
        {
            return false;
        }

        public override string ToNand()
        {
            return "%(1,1)";
        }
    }
}
