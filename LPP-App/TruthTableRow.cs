using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace LPP_App
{
    class TruthTableRow
    {
        private int index;
        private BitArray variableValues;
        private bool finalResult;

        public TruthTableRow(int index, BitArray values, bool finalResult)
        {
            this.index = index;
            this.variableValues = values;
            this.finalResult = finalResult;
        }

        public int GetIndex()
        {
            return this.index;
        }

        public BitArray GetVariableValues()
        {
            return this.variableValues;
        }

        public bool GetFinalResult()
        {
            return this.finalResult;
        }

        public bool Compare(TruthTableRow row)
        {
            int nrOfDifferentValuesInSameIndex = 0;
            for (int i = 0; i < this.variableValues.Count; i++)
            {
                if (variableValues.Get(i) != row.GetVariableValues().Get(i))
                {
                    nrOfDifferentValuesInSameIndex++;
                }
            }
            if (nrOfDifferentValuesInSameIndex == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetTheIndexOfTheOnlyDifferentValue(TruthTableRow row)
        {
            int nrOfDifferentValuesInSameIndex = 0;
            int k = 0;
            for (int i = 0; i < this.variableValues.Count; i++)
            {
                if (variableValues.Get(i) != row.GetVariableValues().Get(i))
                {
                    nrOfDifferentValuesInSameIndex++;
                    k = i;
                }
            }
            if (nrOfDifferentValuesInSameIndex == 1)
            {
                return k;
            }
            else
            {
                return -1;
            }
        }

        //return DNF for each row
        public string GetDnf(TruthTable t)
        {
            List<char> variables = t.GetListOfVariables();
            variables.Sort();
            string formulaForEachRow = string.Empty;
            for (int i = 0; i < this.variableValues.Count; i++)
            {
                if (i < this.variableValues.Count - 1)
                {
                    formulaForEachRow += "&(";
                }
                if (this.variableValues.Get(i) == true)
                {
                    formulaForEachRow += variables[i];
                }
                else if (this.variableValues.Get(i) == false)
                {
                    formulaForEachRow += "~(" + variables[i] + ")";
                }
                if (i < this.variableValues.Count - 1)
                {
                    formulaForEachRow += ",";
                }
            }
            for (int i = 0; i < this.variableValues.Count - 1; i++)
            {
                formulaForEachRow += ")";
            }
            return formulaForEachRow;
        }
    }
}
