using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class TrueNode : PropositionalNode
    {
        private bool trueConstant = true;
        public TrueNode(char c):base(c)
        {
            
        }
        public override char GetChar()
        {
            return '1';
        }

        public override bool GetValue()
        {
            return true;
        }

        public override string ToNand()
        {
            //complete nandified
            return "%(0,0)";
        }
    }
}
