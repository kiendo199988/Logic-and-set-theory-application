using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;

namespace LPP_App
{
    class TruthTable
    {       
        //data table
        private DataTable truthTable;
        //infix Notation
        private string infixNotation; 
        //list of vars
        private List<char> variables;
        //nr of rows
        private int NrOfRows;
        //nr of columns
        private int NrOfColumns;
        //list of truthtable-rows
        private List<TruthTableRow> rows;
        //bitarray handler
        private BitArrayHandler bitArrayHandler;
        //list of result;
        private List<bool> results;
        //get nr of rows
        public int GetNrOfRows()
        {
            return this.NrOfRows;
        }
        //get nr of columns
        public int GetNrOfColumns()
        {
            return this.NrOfColumns;
        }

        //get list of truth-table rows
        public List<TruthTableRow> GetListofRows()
        {
            return this.rows;
        }
        //get list of variables
        public List<char> GetListOfVariables()
        {
            return this.variables;
        }
        //get infix notation
        public string GetInfixNotation()
        {
            return this.infixNotation;
        }
        public List<bool> GetListOfResults()
        {
            return this.results;
        }
        //constructor
        public TruthTable(Proposition t)
        {
            this.variables = t.GetListOfVariables();
            this.variables.Sort();
            this.NrOfRows = (int)Math.Pow(2, variables.Count());
            this.NrOfColumns = variables.Count() + 1;
            rows = new List<TruthTableRow>();
            this.CreateRows(variables, t);
            this.infixNotation = "("+t.ConvertToInfixNotation(t.GetRootNode())+")";
            truthTable = new DataTable("TruthTable");
        }

       

        //get the result for each row
        //add newly created rows to the list of rows
        public void CreateRows(List<char> variables, Proposition tree)
        {
            results = new List<bool>();
            for (int i = 0; i<NrOfRows; i++)
            {
                bitArrayHandler = new BitArrayHandler();
                byte[] bits = new byte[] { (byte)i };
                BitArray variableValues = new BitArray(bits);
                variableValues.Length = NrOfColumns - 1;
                bitArrayHandler.Reverse(variableValues);
                IDictionary<char, bool> valuesDictionary = new Dictionary<char, bool>();

                for(int j = 0; j < variables.Count; j++)
                {   
                    valuesDictionary.Add(variables[j], variableValues.Get(j));
                }
                bool result = tree.GetFinalResult(valuesDictionary);
                results.Add(result);
                rows.Add(new TruthTableRow(i, variableValues, result));
            }
        }

        //generate data table
        public DataTable GenerateTruthTable(TruthTable table)
        {
            for(int i = 0; i < table.NrOfColumns; i++)
            {
                DataColumn c;
                if (i == table.NrOfColumns - 1)
                {
                    c = new DataColumn(table.GetInfixNotation());
                }
                else
                {
                    c = new DataColumn(table.GetListOfVariables()[i].ToString());
                }              
                truthTable.Columns.Add(c);
            }
            DataRow row;
            foreach (TruthTableRow r in table.GetListofRows())
            {
                row = truthTable.NewRow();
                truthTable.Rows.Add(row);
                for (int i=0; i<variables.Count(); i++)
                {
                    truthTable.Rows[r.GetIndex()][i] = Convert.ToInt32(r.GetVariableValues()[i]);
                }
                truthTable.Rows[r.GetIndex()][NrOfColumns - 1] = Convert.ToInt32(r.GetFinalResult());
            }
            return truthTable;
        }

        //get all the 0s and 1s from the last column and store them in list of integer
        public List<int> GetBinaryResultOfTruthTable()
        {
            List<int> results = new List<int>(truthTable.Rows.Count);
            foreach (DataRow row in truthTable.Rows)
            {
                results.Add(Convert.ToInt32(row[truthTable.Columns.Count - 1]));
            }
            results.Reverse();
            return results;
        }


        //get rows with truth value = o
        //public List<TruthTableRow> GetRowsWithResultOf0()
        //{
        //    List<TruthTableRow> listOfRowsWithResult0 = new List<TruthTableRow>();

        //    foreach (TruthTableRow r in rows)
        //    {
        //        if (r.GetFinalResult() == false)
        //        {
        //            listOfRowsWithResult0.Add(r);
        //        }
        //    }
        //    return listOfRowsWithResult0;
        //}

        //get rows with truth value = o
        public List<TruthTableRow> GetRowsWithResultOf1()
        {
            List<TruthTableRow> listOfRowsWithResult1 = new List<TruthTableRow>();

            foreach (TruthTableRow r in rows)
            {
                if (r.GetFinalResult() == true)
                {
                    listOfRowsWithResult1.Add(r);
                }
            }
            return listOfRowsWithResult1;
        }

        

        public bool IsTautology()
        {
            if (!results.Contains(false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsContradiction()
        {
            if (!results.Contains(true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
