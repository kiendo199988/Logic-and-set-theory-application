using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class BetaElement:SemanticTableauxElement
    {
        public BetaElement(SemanticTableauxElement left, SemanticTableauxElement right, SetOfProps set, List<char> activeVars):base(left,right,set,activeVars)
        {

        }
    }
}
