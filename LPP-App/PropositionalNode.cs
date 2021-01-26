using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    abstract class PropositionalNode
    {
        public Char Character { get; set; }
        public PropositionalNode Left { get; set; }
        public PropositionalNode Right { get; set; }
        public ObjectVariableNode ObjectVariableNode { get; set; }
        private List<ObjectVariableNode> objectVariableNodes;

        //values of a node
        //either 1 or 0
        //public string Value { get; set; }
        public ObjectVariableNode GetObjectVariableNode()
        {
            return this.ObjectVariableNode;
        }
        public List<ObjectVariableNode> GetObjectVars()
        {
            return this.objectVariableNodes;
        }
        public int Id { get; set; }
        //proposition variable
        public PropositionalNode(char c)
        {
            this.Character = c;
            this.Left = this.Right = null;
        }
        //conjunction, disjucntion, implication, biimiplication, nand
        public PropositionalNode(char c,PropositionalNode left,PropositionalNode right)
        {
            this.Character = c;
            this.Left = left;
            this.Right = right;
        }
        //negation
        public PropositionalNode(char c, PropositionalNode left)
        {
            this.Character = c;
            this.Left = left;
            this.Right = null;
        }
        //predicate
        public PropositionalNode(char c, List<ObjectVariableNode> objectVariables)
        {
            this.objectVariableNodes = objectVariables;
            this.Character = c;
            this.Left = this.Right = null;
        }
        //universal+existential quantifier
        public PropositionalNode(char c, ObjectVariableNode node, PropositionalNode left)
        {
            this.ObjectVariableNode = node;
            this.Character = c;
            this.Left = left;
        }
        //get infix notation of a char
        public abstract Char GetChar();

        //get the prefix character of the node
        public Char GetPrefixChar()
        {
            return this.Character;
        }

        

        //get the index for the node
        public int GetIndex()
        {
            return this.Id;
        }

        //set the index for the node
        public void SetIndex(int id)
        {
            this.Id = id;
        }

        //get the left node
        public PropositionalNode GetLeftNode()
        {
            if (Left != null)
            {
                return this.Left;
            }
            else
            {
                return null;
            }
        }

        //get the right node
        public PropositionalNode GetRightNode()
        {
            if(Right != null)
            {
                return this.Right;
            }
            else
            {
                return null;
            }
        }

        //set index for each node
        public void SetIndexForChildrenNode()
        {
            if (Left != null)
            {
                Left.SetIndex(2 * this.GetIndex() + 1);
                Left.SetIndexForChildrenNode();
            }
            if(Right != null)
            {
                Right.SetIndex(2 * this.GetIndex() + 2);
                Right.SetIndexForChildrenNode();
            }
        }

        //set value of a operator node
        public abstract bool GetValue();

        //deep copy
        public PropositionalNode DeepCopy()
        {
            PropositionalNode left = null;
            PropositionalNode right = null;
            if (this.Left != null)
            {
                left = this.Left.DeepCopy();
            }
            if (this.Right != null)
            {
                right = this.Right.DeepCopy();
            }
            if (this is ConjunctionNode) { return new ConjunctionNode(this.Character, left, right); }
            else if(this is DisjunctionNode) { return new DisjunctionNode(this.Character, left, right); }
            else if (this is ImplicationNode) { return new ImplicationNode(this.Character, left, right); }
            else if (this is BiImplicationNode) { return new BiImplicationNode(this.Character, left, right); }
            else if (this is NandNode) { return new NandNode(this.Character, left, right); }
            else if (this is NegationNode) { return new NegationNode(this.Character, left); }
            else if (this is VariableNode) { return new VariableNode(this.Character); }
            else if (this is PredicateNode)
            {
                List<ObjectVariableNode> newList = new List<ObjectVariableNode>();
                foreach(ObjectVariableNode objectVariableNode in this.objectVariableNodes)
                {
                    ObjectVariableNode obj = new ObjectVariableNode(objectVariableNode.GetChar());
                    newList.Add(obj);
                }
                return new PredicateNode(this.Character, newList); 
            }
            else if(this is ExistQuantifierNode)
            {
                return new ExistQuantifierNode(this.Character, new ObjectVariableNode(this.ObjectVariableNode.GetChar()), left);
            }
            else if (this is UniversalQuantifierNode)
            {
                return new UniversalQuantifierNode(this.Character, new ObjectVariableNode(this.ObjectVariableNode.GetChar()), left);
            }
            else
            {
                return null;
            }

        }

        //set value to variables
        public virtual void SetVariableValue(bool variableValue)
        {

        }

        public abstract string ToNand();

        public virtual string GetString()
        {
            string s = "";
            return s;
        }

        //check if a node is or has children that are predicate || quantifier
        public bool HasPredicateLogicNode()
        {
            bool result = false;
            if(this is PredicateNode || this is ExistQuantifierNode || this is UniversalQuantifierNode)
            {
                result = true;
            }
            if(result == false)
            {
                if(this.GetLeftNode() != null && this.GetRightNode() != null)
                {
                    result = this.GetLeftNode().HasPredicateLogicNode() || this.GetRightNode().HasPredicateLogicNode();
                }
                else if(this.GetLeftNode() != null && this.GetRightNode() == null)
                {
                    result = this.GetLeftNode().HasPredicateLogicNode();
                }
                else if(this.GetLeftNode() == null && this.GetRightNode() == null)
                {
                    result = false;
                }
            }
            return result;
        }

        //check if a node is a leaf
        public bool IsLeaf()
        {
            if(this is VariableNode || this is TrueNode || this is FalseNode || this is PredicateNode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //get all the predicate Nodes
        public List<PredicateNode> GetPredicates(PropositionalNode root)
        {
            List<PredicateNode> predicates = new List<PredicateNode>();
            if (root == null)
            {
                return predicates;
            }
            else if (root is PredicateNode)
            {
                predicates.Add(root as PredicateNode);
            }
            else if (!root.IsLeaf())
            {
                if (root.GetLeftNode() != null & root.GetRightNode() != null)
                {
                    predicates.AddRange(GetPredicates(root.GetLeftNode()));
                    predicates.AddRange(GetPredicates(root.GetRightNode()));
                }
                else if (root.GetLeftNode() != null & root.GetRightNode() == null)
                {
                    predicates.AddRange(GetPredicates(root.GetLeftNode()));
                }
            }
            return predicates;
        }

       
       

    }
}
