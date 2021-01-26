using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LPP_App
{
    class DisjunctiveNormalFormHandler
    {
        private List<TruthTableRow> truthTableRowsWithResult1;
        private List<char> listOfVars;
        private TruthTable truthTable;
        public DisjunctiveNormalFormHandler(TruthTable truthTable)
        {
            this.truthTable = truthTable;
            this.truthTableRowsWithResult1 = truthTable.GetRowsWithResultOf1();
            listOfVars = truthTable.GetListOfVariables();
            //listOfVars.Sort();
        }

        //get DNF for the original truth table
        public string GetDNFForOriginalTruthTable()
        {
            List<string> listOfFormulasEachRow = new List<string>();
            string finalDNF = string.Empty;

            //add the DNF of each row to the list of formulas
            foreach(TruthTableRow r in truthTableRowsWithResult1)
            {   
                listOfFormulasEachRow.Add(r.GetDnf(truthTable));
            }

            //combine the formulas
            for(int i=0;i<truthTableRowsWithResult1.Count;i++)
            {
                if(i< truthTableRowsWithResult1.Count - 1)
                {
                    finalDNF += "|(";
                }

                finalDNF += listOfFormulasEachRow[i];

                if (i < truthTableRowsWithResult1.Count - 1)
                {
                    finalDNF += ",";
                }
            }
            for(int i = 0; i < truthTableRowsWithResult1.Count-1; i++)
            {
                finalDNF += ")";
            }
            return finalDNF;
        }

        //
        public string GetDNFForSimplifiedTable(DataTable simplifiedTable)
        {
            DataTable normalizedSimplifiedTable = simplifiedTable.Copy();
            List<string> listOfDnfs = new List<string>();
            string normalizedFormula = string.Empty;

            //calculation
            //remove false rows from the normalized simplified table
            for (int i = normalizedSimplifiedTable.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToChar(normalizedSimplifiedTable.Rows[i][listOfVars.Count]) == '0')
                {
                    normalizedSimplifiedTable.Rows.RemoveAt(i);
                }
                
            }

            foreach (DataRow r in normalizedSimplifiedTable.Rows)
            {
                int nrof1or0 = 0;
                string formula = string.Empty;
                nrof1or0++;
                for (int i = 0; i < normalizedSimplifiedTable.Columns.Count - 1; i++)
                {
                    if (Convert.ToChar(r[i]) == '0' || Convert.ToChar(r[i]) == '1')
                    {
                        nrof1or0++;
                    }
                }
               

                for (int i = 0; i < normalizedSimplifiedTable.Columns.Count - 1; i++)
                {               
                    if (i < nrof1or0-2)
                    {
                        formula += "&(";
                    }
                    if (Convert.ToChar(r[i]).Equals('0'))
                    {
                        formula += "~(" + listOfVars[i] + ")";
                    }
                    else if (Convert.ToChar(r[i]).Equals('1'))
                    {
                        formula += listOfVars[i];
                    }
                    if (i<nrof1or0-2)
                    {
                        formula += ",";
                    }
                }

                for (int i = 0; i < nrof1or0-2; i++)
                {
                    formula += ")";
                }
                listOfDnfs.Add(formula);
            }


            for (int i = 0; i < listOfDnfs.Count; i++)
            {
                if (i < listOfDnfs.Count - 1)
                {
                    normalizedFormula += "|(";
                }

                normalizedFormula += listOfDnfs[i];

                if (i < listOfDnfs.Count - 1)
                {
                    normalizedFormula += ",";
                }            
            }
            for (int i = 0;i<listOfDnfs.Count-1;i++)
            {
                normalizedFormula += ")";
            }

            return normalizedFormula;
        }
    }
}

