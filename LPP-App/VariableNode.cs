using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class VariableNode:PropositionalNode
    {
        private bool variableValue;

        public VariableNode(char c) : base(c)
        {

        }
        public override char GetChar()
        {
            return this.Character;
        }

        public override void SetVariableValue(bool variableValue)
        {
            this.variableValue = variableValue;
        }
        
        public override bool GetValue()
        {
            return variableValue;     
        }

        public override string ToNand()
        {
            string nand = this.GetChar().ToString();
            return nand;
        }
    }
}
