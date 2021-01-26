using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class SetOfProps 
    {
        //fields
        public HashSet<Proposition> set { get; set; }

        //
        public HashSet<Proposition> GetSet()
        {
            return this.set;
        }
        //constructor
        public SetOfProps(HashSet<Proposition> set)
        {
            this.set = set;
        }

        //add prop to a set
        public void AddProposition(Proposition p)
        {
            this.set.Add(p);
        }

        //copy a set
        public SetOfProps DeepCopy()
        {
            SetOfProps newSet = new SetOfProps(this.set);
            return newSet;
        }
        //remove an element from a set
        public void Remove(Proposition p)
        {
            this.set.Remove(p);
        }
        

        //check whether a set has 2 props that contradict each other
        public bool HasContradictProp()
        {
            bool result = false;
            int nrOfContradictProp =0;
            for (int i = 0; i < this.set.Count; i++)
            {
                for (int j = 0; j < this.set.Count; j++)
                {
                    if (i != j)
                    {
                        if(this.set.ElementAt(j).GetRootNode() is NegationNode)
                        {
                            if (this.Compare(this.set.ElementAt(i).GetRootNode(), this.set.ElementAt(j).GetRootNode().GetLeftNode()) == true)
                            {
                                nrOfContradictProp++;
                            }
                        }                  
                    }
                }
            }
            if (nrOfContradictProp > 0)
            {
                result = true;
            }
            return result;
        }

        //compare two propositions if they are identical
        public bool Compare(PropositionalNode n1, PropositionalNode n2)
        {
            if(n1 == null && n2 == null)
            {
                return true;
            }
            if (n1 != null && n2 != null)
            {
                if(n1 is PredicateNode && n2 is PredicateNode)
                {
                    PredicateNode predicate1 = n1 as PredicateNode;
                    PredicateNode predicate2 = n2 as PredicateNode;
                    return predicate1.GetString() == predicate2.GetString();
                }
                else
                {
                    return n1.GetPrefixChar() == n2.GetPrefixChar() &&
                    Compare(n1.GetLeftNode(), n2.GetLeftNode()) &&
                    Compare(n1.GetRightNode(), n2.GetRightNode());
                }
                
            }
            return false;
        }

        //check if a set exist an alpha formula
        public bool CheckForAlphaProp()
        {
            int nrOfAlphaProps = 0;
            bool hasAlphaProp = false;
            foreach (Proposition p in this.set)
            {
                if (p.IsAlpha() == true)
                {
                    nrOfAlphaProps++;
                }
            }
            if (nrOfAlphaProps > 0)
            {
                hasAlphaProp = true;
                return hasAlphaProp;
            }
            else
            {
                return hasAlphaProp;
            }
        }

        //check if a set exist a beta formula
        public bool CheckForBetaProp()
        {
            int nrOfBetaProps = 0;
            bool hasBetaProp = false;
            foreach (Proposition p in this.set)
            {
                if (p.IsBeta() == true)
                {
                    nrOfBetaProps++;
                }
            }
            if (nrOfBetaProps > 0)
            {
                hasBetaProp = true;
                return hasBetaProp;
            }
            else
            {
                return hasBetaProp;
            }
        }

        //check for delta prop
        public bool CheckForDeltaProp()
        {
            int nrOfDeltaProps = 0;
            bool hasDeltaProp = false;
            foreach (Proposition p in this.set)
            {
                if (p.IsDelta() == true)
                {
                    nrOfDeltaProps++;
                }
            }
            if (nrOfDeltaProps > 0)
            {
                hasDeltaProp = true;
                return hasDeltaProp;
            }
            else
            {
                return hasDeltaProp;
            }
        }

        //check for gamma prop
        public bool CheckForGammaProp()
        {
            int nrOfGammaProps = 0;
            bool hasGammaProp = false;
            foreach (Proposition p in this.set)
            {
                if (p.IsGamma() == true)
                {
                    nrOfGammaProps++;
                }
            }
            if (nrOfGammaProps > 0)
            {
                hasGammaProp = true;
                return hasGammaProp;
            }
            else
            {
                return hasGammaProp;
            }
        }

        //return a new set after applying alpha rule
        public SetOfProps AlphaRuleForASetOfProp()
        {
            if (this.CheckForAlphaProp() == true)
            {
                //create a new hash set
                HashSet<Proposition> newHashSet = new HashSet<Proposition>();
                //HashSet<Propostion> final = new HashSet<Propostion>();
                foreach (Proposition p in this.GetSet())
                {
                    //check for alpha props
                    //if is an alpha prop
                    if(p.IsAlpha() == false)
                    {
                        newHashSet.Add(p);
                    }
                    else if (p.IsAlpha() == true)
                    {
                        //seperate the alpha prop into smaller ones
                        foreach (Proposition prop in p.GetChildPropFromAlphaProp())
                        {                          
                            newHashSet.Add(prop);                                                         
                        }
                    }
                }
               
                //group props by infix notation
                //create new hash set with no duplicate props
                HashSet<Proposition> final = new HashSet<Proposition>(newHashSet.GroupBy(x => x.ConvertToInfixNotation(x.GetRootNode())).Select(x => x.First()).ToList());
                //return new set of prop
                SetOfProps newSet = new SetOfProps(final);             
                return newSet;
            }
            else
            {
                return null;
            }
        }

        //apply delta rule
        public SetOfProps DeltaRule(char c)
        {
            if (this.CheckForDeltaProp())
            {
                List<Proposition> listOfDeltas = new List<Proposition>();
                HashSet<Proposition> newHashset = new HashSet<Proposition>();
                foreach(Proposition p in this.set)
                {
                    if (p.IsDelta())
                    {
                        listOfDeltas.Add(p);
                    }
                    else
                    {
                        newHashset.Add(p);
                    }
                }
                //get the first element of list of deltas, get its child
                Proposition deltaChild = listOfDeltas.ElementAt(0).GetChildPropFromDeltaProp(c);
                //add it to hash set
                newHashset.Add(deltaChild);
                //remove first element after applying delta rule
                listOfDeltas.RemoveAt(0);
                //add the remaining delta props from the list
                foreach(Proposition p in listOfDeltas)
                {
                    newHashset.Add(p);
                }
                //returen new set of propositions
                SetOfProps newSet = new SetOfProps(newHashset);
                return newSet;
            }
            else
            {
                return null;
            }
        }


        //apply beta rule
        public List<SetOfProps> BetaRuleForASetOfProp()
        {
            if (this.CheckForBetaProp() == true)
            {
                //create 2 new hash sets
                HashSet<Proposition> newHashSet1 = new HashSet<Proposition>();
                HashSet<Proposition> newHashSet2 = new HashSet<Proposition>();
                //list of beta props in a set
                List<Proposition> listOfBetaProps = new List<Proposition>();

                foreach (Proposition p in this.GetSet())
                {
                    //check for beta prop
                    //if it is not
                    if (p.IsBeta() == false)
                    {
                        //add it to 2 new hash sets
                        newHashSet1.Add(p);
                        newHashSet2.Add(p);
                    }
                    else if (p.IsBeta() == true)
                    {
                        //add them to the list                       
                        listOfBetaProps.Add(p);                      
                    }
                }

                //if there is only 1 beta element in the list
                if(listOfBetaProps.Count == 1)
                {
                    //add its first child to hash set 1
                    newHashSet1.Add(listOfBetaProps.ElementAt(0).GetChildPropFromBetaProp().ElementAt(0));
                    //add its second child to hash set 2
                    newHashSet2.Add(listOfBetaProps.ElementAt(0).GetChildPropFromBetaProp().ElementAt(1));

                }
                else if(listOfBetaProps.Count>1)
                {
                    //add its first child to hash set 1
                    newHashSet1.Add(listOfBetaProps.ElementAt(0).GetChildPropFromBetaProp().ElementAt(0));
                    //add its second child to hash set 2
                    newHashSet2.Add(listOfBetaProps.ElementAt(0).GetChildPropFromBetaProp().ElementAt(1));
                    //remove the parent from the list
                    listOfBetaProps.RemoveAt(0);
                    //add the rest of the beta props from the list
                    for(int i = 0; i < listOfBetaProps.Count; i++)
                    {
                        newHashSet1.Add(listOfBetaProps.ElementAt(i));
                        newHashSet2.Add(listOfBetaProps.ElementAt(i));
                    }
                }

                //group props by infix notation
                //create new hash set with no duplicate props
                HashSet<Proposition> final1 = new HashSet<Proposition>(newHashSet1.GroupBy(x => x.ConvertToInfixNotation(x.GetRootNode())).Select(x => x.First()).ToList());
                HashSet<Proposition> final2 = new HashSet<Proposition>(newHashSet2.GroupBy(x => x.ConvertToInfixNotation(x.GetRootNode())).Select(x => x.First()).ToList());
                //creata 2 new sets
                SetOfProps newSet1 = new SetOfProps(final1);
                SetOfProps newSet2 = new SetOfProps(final2);
                //add them to a list of set
                List<SetOfProps> listOfSets = new List<SetOfProps>();
                listOfSets.Add(newSet1);
                listOfSets.Add(newSet2);

                return listOfSets;
            }
            else
            {
                return null;
            }
        }

        //gamma rule 
        public SetOfProps GammaRuleForASetOfProp(List<char> activeVars)
        {
            if (this.CheckForGammaProp())
            {
                //list of gamma propostions
                List<Proposition> gammaProps = new List<Proposition>();
                //list of children after applying gamma rule to a list of gamma props
                List<Proposition> children1 = new List<Proposition>();
                //hash set
                HashSet<Proposition> hashSet = new HashSet<Proposition>();
                //get all gamma propositions
                foreach(Proposition p in this.set)
                {
                    if (p.IsGamma())
                    {
                        hashSet.Add(p.EliminateDoubleNegation(p));
                        //add them to a list of gamma props
                        gammaProps.Add(p.DeepCopy());
                    }
                    else
                    {
                        //add to hash set
                        hashSet.Add(p.EliminateDoubleNegation(p));
                    }
                }

                foreach(Proposition p in gammaProps)
                {
                    foreach(Proposition child in p.GetChildPropFromGammaProp(activeVars))
                    {
                        int duplicatedOccurence = 0;
                        //compare the gamma children to all props from current set
                        for(int i = 0; i < this.set.Count; i++)
                        {
                            if (this.Compare(child.GetRootNode(), this.set.ElementAt(i).GetRootNode()))
                            {
                                duplicatedOccurence++;
                            }
                        }
                        //if there is no duplicate
                        if (duplicatedOccurence == 0)
                        {
                            //add it to list
                            children1.Add(child);
                        }
                    }
                }
                List<Proposition> childrenWithNoDup = new List<Proposition>(children1.GroupBy(x => x.ConvertToInfixNotation(x.GetRootNode())).Select(x => x.First()).ToList());
                //if there is no new children
                if (childrenWithNoDup.Count == 0)
                {
                    //return a null set of props
                    return null;
                }
                else
                { 
                    //add the gamma children to hash set
                    foreach (Proposition p in childrenWithNoDup)
                    {
                        hashSet.Add(p);
                    }
                    //return new set of props
                    SetOfProps set = new SetOfProps(hashSet);
                    return set;
                }                          
            }
            else
            {
                return null;
            }
        }
    }
}
