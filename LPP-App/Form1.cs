using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace LPP_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void CalculateBtn_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        public void Calculate()
        {
            try
            {
                infixNotationTb.Text = string.Empty;
                extractedVarTb.Text = string.Empty;
                TbHashCode.Text = string.Empty;
                tbDNFForTruthTable.Text = string.Empty;
                tbDNFForSimplifiedTruthTable.Text = string.Empty;
                tbInfixOfDNFForTruthTable.Text = string.Empty;
                tbInfixDNFForSimplifiedTable.Text = string.Empty;
                tbHashCodeTableDNFSimplified.Text = string.Empty;
                tbHashCodeSimNorNand.Text = string.Empty;
                tbNandifiedFormula.Text = string.Empty;
                tbHashCodeFromNandifiedFormula.Text = string.Empty;
                tbInfixFormulaFromNandifiedFormula.Text = string.Empty;
                tbHashCodeNandifiedSimplified.Text = string.Empty;
                tbHashCodeForDNFOriginal.Text = string.Empty;
                tbCheckForPredicate.BackColor = Color.White;
                
                //binary tree
                Proposition binaryTree = new Proposition(formulaTb.Text);

                if (binaryTree.HasValidPredicate())
                {
                    //binary tree image    
                    binaryTree.GenerateTreeImage();
                    treePb.ImageLocation = "binarytree.png";
                    //showing infix notation
                    infixNotationTb.Text = binaryTree.ConvertToInfixNotation(binaryTree.GetRootNode());
                }
                else
                {
                    MessageBox.Show("The formula has invalid predicates!");
                }
                

                if(!binaryTree.IsPredicateFormula())
                {
                    //check for predicate formula
                    tbCheckForPredicate.BackColor = Color.Red;
                    
                    //extract the variables
                    List<char> extractedFormula = binaryTree.ExtractVariable(formulaTb.Text);
                    extractedFormula.Sort();
                    foreach (char c in extractedFormula)
                    {
                        if (c == extractedFormula[extractedFormula.Count - 1])
                        {
                            extractedVarTb.Text += c + ".";
                        }
                        else
                        {
                            extractedVarTb.Text += c + ", ";
                        }
                    }


                    

                    //truth table
                    TruthTable truthTable = new TruthTable(binaryTree);
                    DataTable dataTable = truthTable.GenerateTruthTable(truthTable);
                    PopulateListbox(listBoxTruthTable, dataTable);


                    //hash code
                    HashCodeCalculator hashCodeCalculator = new HashCodeCalculator();
                    TbHashCode.Text = hashCodeCalculator.GetHashCode(truthTable.GetBinaryResultOfTruthTable());

                    //simplified truth table
                    SimplifiedTruthTable simplifiedTruthTable = new SimplifiedTruthTable();
                    DataTable simplifiedDataTable = simplifiedTruthTable.GetFinalSimplifiedTruthTable(dataTable);
                    PopulateListbox(lbSimplifiedTruthTable, simplifiedDataTable);


                    //DNF
                    DisjunctiveNormalFormHandler disjunctiveNormalForm = new DisjunctiveNormalFormHandler(truthTable);
                    
                    string DNFForOriginalTruthTable = string.Empty;
                    string DNFForSimplifiedTruthTable = string.Empty;
                   

                    //DNF for original prop (original + normalize)
                    DNFForOriginalTruthTable = disjunctiveNormalForm.GetDNFForOriginalTruthTable();
                    tbDNFForTruthTable.Text = DNFForOriginalTruthTable;
                    Proposition DNFProposition = new Proposition(DNFForOriginalTruthTable);
                    tbInfixOfDNFForTruthTable.Text = DNFProposition.ConvertToInfixNotation(DNFProposition.GetRootNode());

                    //Truth table for DNF (original + normalize)
                    TruthTable truthTableForDNF = new TruthTable(DNFProposition);
                    DataTable dataTableForDNF = truthTableForDNF.GenerateTruthTable(truthTableForDNF);
                    PopulateListbox(lbDNFTruthTable, dataTableForDNF);

                    //DNF for simplified truth table
                    if (!truthTable.IsTautology() && !truthTable.IsContradiction())
                    {
                        DNFForSimplifiedTruthTable = disjunctiveNormalForm.GetDNFForSimplifiedTable(simplifiedDataTable);
                        tbDNFForSimplifiedTruthTable.Text = DNFForSimplifiedTruthTable;
                        Proposition DNFForSimplifiedTableProposition = new Proposition(DNFForSimplifiedTruthTable);
                        tbInfixDNFForSimplifiedTable.Text = DNFForSimplifiedTableProposition.ConvertToInfixNotation(DNFForSimplifiedTableProposition.GetRootNode());

                        //Truth table for DNF (simplified)
                        TruthTable truthTableForDNFForSimplifiedTable = new TruthTable(DNFForSimplifiedTableProposition);
                        DataTable dataTableForDNFForSimplifiedTable = truthTableForDNFForSimplifiedTable.GenerateTruthTable(truthTableForDNFForSimplifiedTable);
                        PopulateListbox(lbTruthTableForDNFFromSimplifiedTruthTable, dataTableForDNFForSimplifiedTable);
                        tbHashCodeTableDNFSimplified.Text = hashCodeCalculator.GetHashCode(truthTableForDNFForSimplifiedTable.GetBinaryResultOfTruthTable());

                        //hashcode for DNF orginal
                        tbHashCodeForDNFOriginal.Text = hashCodeCalculator.GetHashCode(truthTableForDNF.GetBinaryResultOfTruthTable());


                        //Original + simplified + Normalize + Nandified Proposition
                        Proposition simplifiedNormNandProp = new Proposition(DNFForSimplifiedTableProposition.GetNandifiedFormula());

                        TruthTable truthTableForSimpNormNand = new TruthTable(simplifiedNormNandProp);
                        DataTable dataTableForSimpNormNandTable = truthTableForSimpNormNand.GenerateTruthTable(truthTableForSimpNormNand);
                        PopulateListbox(lbSNNTable, dataTableForSimpNormNandTable);
                        tbHashCodeSimNorNand.Text = hashCodeCalculator.GetHashCode(truthTableForSimpNormNand.GetBinaryResultOfTruthTable());

                    }
                    else if (truthTable.IsTautology())
                    {
                        //Propostion DNFForSimplifiedTableProposition = new Propostion("1");
                        DNFForSimplifiedTruthTable = "1";
                        //lbDNFTruthTable.Items.Clear();
                        lbTruthTableForDNFFromSimplifiedTruthTable.Items.Clear();
                        tbDNFForSimplifiedTruthTable.Text = DNFForSimplifiedTruthTable;
                        tbInfixDNFForSimplifiedTable.Text = DNFForSimplifiedTruthTable;

                        //hashcode for DNF
                        tbHashCodeTableDNFSimplified.Text = hashCodeCalculator.GetHashCode(truthTable.GetBinaryResultOfTruthTable());
                        tbHashCodeForDNFOriginal.Text = string.Empty;
                        tbHashCodeForDNFOriginal.Text = hashCodeCalculator.GetHashCode(truthTableForDNF.GetBinaryResultOfTruthTable());
                        tbHashCodeSimNorNand.Text = hashCodeCalculator.GetHashCode(truthTable.GetBinaryResultOfTruthTable());
                    }
                    else if (truthTable.IsContradiction())
                    {
                        lbDNFTruthTable.Items.Clear();
                        lbSNNTable.Items.Clear();
                        lbTruthTableForDNFFromSimplifiedTruthTable.Items.Clear();
                        tbHashCodeTableDNFSimplified.Text = "0";
                        tbHashCodeForDNFOriginal.Text = "0";
                        tbHashCodeSimNorNand.Text = "0";
                    }



                    //nandifying
                   
                    tbNandifiedFormula.Text = binaryTree.GetNandifiedFormula();
                    string DNFForNandSimplifyTable = string.Empty;

                    Proposition nandifiedFormulaProposiion = new Proposition(binaryTree.GetNandifiedFormula());

                    TruthTable truthTableForNandifiedFormula = new TruthTable(nandifiedFormulaProposiion);
                    DataTable dataTableForNandifiedFormula = truthTableForNandifiedFormula.GenerateTruthTable(truthTableForNandifiedFormula);
                    PopulateListbox(lbNandifiedTruthTable, dataTableForNandifiedFormula);
                    tbHashCodeFromNandifiedFormula.Text = hashCodeCalculator.GetHashCode(truthTableForNandifiedFormula.GetBinaryResultOfTruthTable());
                    tbInfixFormulaFromNandifiedFormula.Text = nandifiedFormulaProposiion.ConvertToInfixNotation(nandifiedFormulaProposiion.GetRootNode());


                    //Nandified + Simplified
                    //table
                    SimplifiedTruthTable nandifiedSimplifiedTruthTable = new SimplifiedTruthTable();
                    DataTable nandifiedSimplifiedDataTable = nandifiedSimplifiedTruthTable.GetFinalSimplifiedTruthTable(dataTableForNandifiedFormula);
                    PopulateListbox(lbNSTable, nandifiedSimplifiedDataTable);

                    //normalize it to get the hash value
                    DNFForNandSimplifyTable = disjunctiveNormalForm.GetDNFForSimplifiedTable(nandifiedSimplifiedDataTable);
                    Proposition DNFForNandSimplifyTableProposition = new Proposition(DNFForNandSimplifyTable);

                    //table for nand+simp+normalized prop
                    TruthTable truthTableForNandSimpNorm = new TruthTable(DNFForNandSimplifyTableProposition);
                    DataTable dataTableForNandSimpNorm = truthTableForNandSimpNorm.GenerateTruthTable(truthTableForNandSimpNorm);
                    PopulateListbox(lbNandSimpNorm, dataTableForNandSimpNorm);

                    //hashcode
                    if (!truthTableForNandifiedFormula.IsTautology() && !truthTableForNandifiedFormula.IsContradiction())
                    {
                        tbHashCodeNandifiedSimplified.Text = hashCodeCalculator.GetHashCode(truthTableForNandSimpNorm.GetBinaryResultOfTruthTable());
                    }
                    else if (truthTableForNandifiedFormula.IsContradiction())
                    {
                        lbNandSimpNorm.Items.Clear();
                        tbHashCodeNandifiedSimplified.Text = "0";
                    }
                    else if (truthTableForNandifiedFormula.IsTautology())
                    {
                        lbNandSimpNorm.Items.Clear();
                        tbHashCodeNandifiedSimplified.Text = hashCodeCalculator.GetHashCode(truthTableForNandifiedFormula.GetBinaryResultOfTruthTable());
                    }


                    //automated hashcode testing
                    if (TbHashCode.Text.Equals(tbHashCodeForDNFOriginal.Text)
                        && TbHashCode.Text.Equals(tbHashCodeTableDNFSimplified.Text)
                        && TbHashCode.Text.Equals(tbHashCodeNandifiedSimplified.Text)
                        && TbHashCode.Text.Equals(tbHashCodeSimNorNand.Text)
                       )
                    {
                        TbHashCode.BackColor = Color.Green;
                    }
                    else
                    {
                        TbHashCode.BackColor = Color.Red;
                    }
                }
                else
                {
                    tbCheckForPredicate.BackColor = Color.Green;
                }



            }
            catch (NullReferenceException ex)
            {
                //MessageBox.Show("Please input a prosition (without numbers)");
            }
            catch(IndexOutOfRangeException ex)
            {
                MessageBox.Show("Index out of range exception");
            }




        }

        private void PopulateListbox(ListBox lb, DataTable table)
        {
            lb.DataSource = null;
            lb.Items.Clear();

            string columnNames = string.Empty;
            foreach (DataColumn column in table.Columns)
            {
                columnNames += column.ColumnName + " ";
            }
            lb.Items.Add(columnNames);
            lb.Items.Add(string.Empty);

            foreach (DataRow row in table.Rows)
            {
                string rowString = string.Empty;
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    rowString += row[i] + "   ";
                }
                lb.Items.Add(rowString);
            }
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbInfixFormulaFromNandifiedFormula_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tbDNFForTruthTable_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tbInfixOfDNFForTruthTable_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void tbHashCodeForDNFOriginal_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbHashCodeSimNorNan_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tbDNFForSimplifiedTruthTable_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void tbInfixDNFForSimplifiedTable_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }

        private void tbInfixOfDNFForTruthTable_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label9_Click_1(object sender, EventArgs e)
        {

        }

        private void tbInfixDNFForSimplifiedTable_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void btnTableux_Click(object sender, EventArgs e)
        {
            
        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void btnSemanticTableux_Click(object sender, EventArgs e)
        {
            try
            {
                Proposition binaryTree = new Proposition(formulaTb.Text);
                //negate and double-negation elim
                Proposition p1 = binaryTree.Negate();
                Proposition propostion = p1.EliminateDoubleNegation(p1);

                //tableux
                if (propostion.HasUnboundedObjectVariables() == false && propostion.HasValidPredicate())
                {
                    SemanticTableaux semanticTableux = new SemanticTableaux(propostion);
                    SemanticTableauxElement semanticTableuxElementRoot = semanticTableux.GetRootTableuxElement();

                    //change color to green when it is a tautology
                    //else red
                    if (semanticTableux.isContradiction() == true)
                    {
                        tbIsTautology.BackColor = Color.Green;
                    }
                    else
                    {
                        tbIsTautology.BackColor = Color.Red;
                    }

                    //steps shows by tree
                    semanticTableux.GenerateTreeImage();
                    pbSemanticTableux.ImageLocation = "semanticTableux.png";
                }
                else if (propostion.HasUnboundedObjectVariables() == true || !propostion.HasValidPredicate())
                {
                    MessageBox.Show("This proposition has unbounded object variables or invalid predicates!");
                }
            }
            catch(IndexOutOfRangeException ex)
            {
                MessageBox.Show("Infinite recursion formula!");
            }
            
            
            
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            lbChildrenTest.Items.Clear();
            List<char> activeVars = new List<char>();
            activeVars.Add('a');
            activeVars.Add('b');
            activeVars.Add('c');
            Proposition p1 = new Proposition("P(a,y)");
            Proposition p2 = new Proposition("@xP(x,y)");
            Proposition p3 = new Proposition("P(b,y)");
            Proposition p4 = new Proposition("P(c,y)");

            HashSet<Proposition> hashSet = new HashSet<Proposition>();
            hashSet.Add(p1);
            hashSet.Add(p2);
            hashSet.Add(p3);
            hashSet.Add(p4);
            SetOfProps set = new SetOfProps(hashSet);
            SetOfProps gammSet = set.GammaRuleForASetOfProp(activeVars);
            if(gammSet != null)
            {
                foreach (Proposition p in gammSet.GetSet())
                {
                    lbChildrenTest.Items.Add(p.ConvertToInfixNotation(p.GetRootNode()));
                }
            }
            else
            {
                foreach (Proposition p in set.GetSet())
                {
                    lbChildrenTest.Items.Add(p.ConvertToInfixNotation(p.GetRootNode()));
                }
            }
            
            //Proposition binaryTree = new Proposition(formulaTb.Text);

            //foreach (Proposition p in binaryTree.GetChildPropFromGammaProp(activeVars))
            //{
            //    lbGammaChildren.Items.Add(p.ConvertToInfixNotation(p.GetRootNode()));
            //}
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbTestDelta_TextChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnChildrenFromADeltaSet_Click(object sender, EventArgs e)
        {
            lbChildrenTest.Items.Clear();
            List<char> activeVars = new List<char>();
            activeVars.Add('a');
            activeVars.Add('b');
            activeVars.Add('c');
            Proposition p1 = new Proposition("P(a,y)");
            Proposition p2 = new Proposition("~@xP(x,x)");
            Proposition p3 = new Proposition("P(b,y)");
            Proposition p4 = new Proposition("P(d,y)");

            HashSet<Proposition> hashSet = new HashSet<Proposition>();
            hashSet.Add(p1);
            hashSet.Add(p2);
            hashSet.Add(p3);
            hashSet.Add(p4);
            SetOfProps set = new SetOfProps(hashSet);
            SetOfProps deltaSet = set.DeltaRule(activeVars[0]);

            foreach (Proposition p in deltaSet.GetSet())
            {
                lbChildrenTest.Items.Add(p.ConvertToInfixNotation(p.GetRootNode()));
            }
        }

        private void btnChildrenFromABetaSet_Click(object sender, EventArgs e)
        {
            lbChildrenTest.Items.Clear();
            
            Proposition p1 = new Proposition("P(a,y)");
            Proposition p2 = new Proposition(">(P,Q)");
            Proposition p3 = new Proposition("P(b,y)");
            Proposition p4 = new Proposition("P(d,y)");

            HashSet<Proposition> hashSet = new HashSet<Proposition>();
            hashSet.Add(p1);
            hashSet.Add(p2);
            hashSet.Add(p3);
            hashSet.Add(p4);
            SetOfProps set = new SetOfProps(hashSet);
            SetOfProps deltaSet = set.BetaRuleForASetOfProp().ElementAt(0);

            foreach (Proposition p in deltaSet.GetSet())
            {
                lbChildrenTest.Items.Add(p.ConvertToInfixNotation(p.GetRootNode()));
            }
        }

        private void btnChildrenFromAAlphaSet_Click(object sender, EventArgs e)
        {
            lbChildrenTest.Items.Clear();
            
            Proposition p1 = new Proposition("P(a,y)");
            Proposition p2 = new Proposition("&(P,Q)");
            Proposition p3 = new Proposition("P(b,y)");
            Proposition p4 = new Proposition("P(d,y)");

            HashSet<Proposition> hashSet = new HashSet<Proposition>();
            hashSet.Add(p1);
            hashSet.Add(p2);
            hashSet.Add(p3);
            hashSet.Add(p4);
            SetOfProps set = new SetOfProps(hashSet);
            SetOfProps deltaSet = set.AlphaRuleForASetOfProp();

            foreach (Proposition p in deltaSet.GetSet())
            {
                lbChildrenTest.Items.Add(p.ConvertToInfixNotation(p.GetRootNode()));
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void btnTest_Click_1(object sender, EventArgs e)
        {
            Proposition p = new Proposition(tbPropRule.Text);
            if (p.IsDelta())
            {
                tbDelta.BackColor = Color.Green;
            }
            else if (p.IsGamma())
            {
                tbGamma.BackColor = Color.Green;
            }
            else if (p.IsBeta())
            {
                tbBeta.BackColor = Color.Green;
            }
            else if (p.IsAlpha())
            {
                tbAlpha.BackColor = Color.Green;
            }
        }

        private void tabPage13_Click(object sender, EventArgs e)
        {

        }
    }
}
