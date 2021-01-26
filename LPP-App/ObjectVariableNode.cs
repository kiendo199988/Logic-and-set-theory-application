using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class ObjectVariableNode
    {
        private char character;
        public char GetChar()
        {
            return this.character;
        }
        public void SetChar(char c)
        {
            this.character = c;
        }
        public bool isBounded { get; set; }
        public bool GetIsBounded()
        {
            return this.isBounded;
        }
        public void SetBound(bool b)
        {
            this.isBounded = b;
        }
        public ObjectVariableNode(char c)
        {
            this.character = c;
        }

       
    }
}
