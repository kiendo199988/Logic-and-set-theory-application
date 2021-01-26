using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class DeltaElement:SemanticTableauxElement
    {
        private List<char> activeVars;
        public List<char> GetActiveVars()
        {
            return this.activeVars;
        }

        public DeltaElement(SemanticTableauxElement left, SetOfProps set,List<char> activeVars):base(left,set,activeVars)
        {
            this.activeVars = activeVars;
        }
    }
}
