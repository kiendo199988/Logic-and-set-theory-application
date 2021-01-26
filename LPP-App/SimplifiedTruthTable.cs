using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;

namespace LPP_App
{
    class SimplifiedTruthTable
    {
        
        public SimplifiedTruthTable()
        {
          
        }

        //get the final simplified truth table 
        public DataTable GetFinalSimplifiedTruthTable(DataTable originalTruthTable)
        {
            DataTable transitionalTable = originalTruthTable.Copy(); //the table to hold one iteration of simplification
            DataTable finalSimplifiedTable = originalTruthTable.Clone(); //the final simplified table, have the structure cloned from the original one
            DataTable zeroResultsTable = originalTruthTable.Clone();

            int[] results = new int[originalTruthTable.Rows.Count];
            for (int i = 0; i < originalTruthTable.Rows.Count; i++)
            {
                results[i] = Convert.ToInt32(originalTruthTable.Rows[i][originalTruthTable.Columns.Count - 1]);
            }

            if (!results.Contains(1)) //return original table 
            {
                return originalTruthTable;
            }           
            //if the results of the truth table indicates that the proposition is a tautology
            else if (!results.Contains(0)) 
            {
                DataRow row = finalSimplifiedTable.NewRow();

                for (int i = 0; i < finalSimplifiedTable.Columns.Count - 1; i++) 
                {
                    row[i] = '*';
                }
                finalSimplifiedTable.Rows.Add(row);

                row[Convert.ToInt32(originalTruthTable.Columns.Count - 1)] = '1';
                return finalSimplifiedTable;
            }

            //calculation
            //add false rows to the simplified table and remove them from the intermediate one
            for (int i = transitionalTable.Rows.Count - 1; i >= 0; i--) //go backwards because removing items while the loop is increasing disrupts the relative position i
            {
                if (Convert.ToChar(transitionalTable.Rows[i][transitionalTable.Columns.Count - 1]) == '0')
                {
                    //add the rows with result = 0 to zeroResultsTable
                    zeroResultsTable.ImportRow(transitionalTable.Rows[i]);
                    transitionalTable.Rows.RemoveAt(i);
                }
            }

            finalSimplifiedTable = SimplifyTable(AssignRowsToGroups(transitionalTable), finalSimplifiedTable, transitionalTable);

            //merge the zero result table
            finalSimplifiedTable.Merge(zeroResultsTable);

            return finalSimplifiedTable;
        }

        // Uses the groups to simplify and populate the given table
        private DataTable SimplifyTable(Dictionary<int, List<DataRow>> groups, DataTable newTable, DataTable previousTable)
        {
            //check if this is the last simplification before tautology by checking the nr of stars in a row. 
            //If the nr of stars = nr of variables - 1 
            //cant simplify more and return the previousTable
            for (int i = 0; i < previousTable.Rows.Count; i++)
            {
                DataRow currentRow = previousTable.Rows[i];
                //check for differences
                int amountOfStars = 0;
                for (int j = 0; j < newTable.Columns.Count - 1; j++)
                {
                    //add the column indexes of the variables that differ
                    if (currentRow[j].Equals('*'))
                    {
                        amountOfStars++;
                    }
                }
                if (amountOfStars == previousTable.Columns.Count - 2) //nr of star = nr of variables - 1
                {
                    return previousTable;
                }
            }

            List<DataRow> nextGroupUsedRows = new List<DataRow>();

            //for every group except the last one
            for (int i = 0; i < groups.Count - 1; i++)
            {
                List<DataRow> currentGroup = groups.ElementAt(i).Value;
                List<DataRow> currentGroupUsedRows = nextGroupUsedRows;
                nextGroupUsedRows = new List<DataRow>();

                //for every row in the current group
                for (int j = 0; j < currentGroup.Count; j++)
                {
                    DataRow currentRow = currentGroup[j];

                    //for every row in the next group
                    for (int k = 0; k < groups.ElementAt(i + 1).Value.Count; k++)
                    {
                        DataRow nextGroupCurrentRow = groups.ElementAt(i + 1).Value[k];
                        List<int> differentValueIndexes = new List<int>();

                        //check for differences
                        for (int l = 0; l < newTable.Columns.Count - 1; l++)
                        {
                            //add the column indexes of the variables that differ
                            if (!currentRow.ItemArray[l].Equals(nextGroupCurrentRow.ItemArray[l]))
                            {
                                differentValueIndexes.Add(l);
                            }
                        }

                        //if there is only one different value, we can simplify
                        if (differentValueIndexes.Count == 1)
                        {
                            DataRow newRow = newTable.NewRow();
                            newRow.ItemArray = currentRow.ItemArray; //copy the values from the current row that we're checking
                            newRow.SetField(newTable.Columns[differentValueIndexes[0]], '*');
                            newTable.Rows.Add(newRow);
                            newTable = newTable.DefaultView.ToTable(true);

                            if (!currentGroupUsedRows.Contains(currentRow))
                            {
                                currentGroupUsedRows.Add(currentRow);
                            }
                            if (!nextGroupUsedRows.Contains(nextGroupCurrentRow))
                            {
                                nextGroupUsedRows.Add(nextGroupCurrentRow);
                            }
                        }
                    }
                }

                //once its finished processing, remove the used rows from the current group
                foreach (DataRow row in currentGroupUsedRows)
                {
                    currentGroup.Remove(row);
                }

                //add the rows that were not used to the table and remove it from the groups
                for (int j = 0; j < currentGroup.Count; j++)
                {
                    newTable.ImportRow(currentGroup.ElementAt(j));
                    currentGroup.RemoveAt(j);
                }
            }

            //add the rows that were not used from the last group
            foreach (DataRow row in groups.Last().Value)
            {
                if (!nextGroupUsedRows.Contains(row))
                {
                    newTable.ImportRow(row);
                }
            }

            //if we do have a previous table, check if they are the same
            List<DataRow> differences = previousTable.AsEnumerable().Except(newTable.AsEnumerable(), DataRowComparer.Default).ToList();

            //if there are differences, run the simplification again
            if (differences.Any() && newTable.Rows.Count > 0)
            {
                return SimplifyTable(AssignRowsToGroups(newTable), newTable.Clone(), newTable);
            }
            //if we have no differences, it means that the last run produced the same result as the current run and more simplification is not possible. Return the table
            else
            {
                return newTable;
            }
        }

        /// <summary>
        /// Groups the DataRows based on the amount of ones in their binary row index. <br></br>
        /// The key for each group is made using the amount of ones: 0Ones, 1Ones, 2Ones, etc.
        /// </summary>
        /// <param name="dataToGroup">The DataTable to organize in groups.</param>
        /// <returns>Returns a dictionary with the groups assigned into the amount of stars in the row.</returns>
        private Dictionary<int, List<DataRow>> AssignRowsToGroups(DataTable dataToGroup)
        {
            Dictionary<int, List<DataRow>> groups = new Dictionary<int, List<DataRow>>();

            for (int i = 0; i < dataToGroup.Columns.Count; i++) 
            {
                groups.Add(i, new List<DataRow>());
            }

            foreach (DataRow row in dataToGroup.Rows)
            {
                int amountOfOnes = 0;
                for (int i = 0; i < dataToGroup.Columns.Count - 1; i++) 
                {
                    if (Convert.ToChar(row[i]) == '1') 
                    {
                        amountOfOnes++;
                    }
                }
                groups[amountOfOnes].Add(row);
            }

            Dictionary<int, List<DataRow>> temp = new Dictionary<int, List<DataRow>>();

            for (int i = 0; i < groups.Count; i++)
            {
                if (groups.ElementAt(i).Value.Count > 0)
                {
                    temp.Add(groups.ElementAt(i).Key, groups.ElementAt(i).Value);
                }
            }

            return temp;
        }
    }
}
    

