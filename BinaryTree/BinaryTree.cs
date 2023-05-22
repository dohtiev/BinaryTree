using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrey
{
    public class BinaryTreeNode : IComparable<BinaryTreeNode>
    {
        public int Value;
        public int Level;
        public BinaryTreeNode Left;
        public BinaryTreeNode Right;
        public BinaryTreeNode Parent;

        public BinaryTreeNode(int value, int level, BinaryTreeNode parent = null)
        {
            Value = value;
            Level = level;
            Parent = parent;
        }

        public int CompareTo(BinaryTreeNode other) => Value.CompareTo(other.Value);
    }

    public class BinaryTree
    {
        private BinaryTreeNode root;
        private int maxLevel;

        public BinaryTree(params int[] sequence)
        {
            foreach (int i in sequence)
                Add(i);
        }

        public BinaryTree(BinaryTree copyFrom)
        {
            void PreorderTraversal(BinaryTreeNode node)
            {
                if (node != null)
                {
                    Add(node.Value);
                    PreorderTraversal(node.Left);
                    PreorderTraversal(node.Right);
                }
            }

            PreorderTraversal(copyFrom.root);
        }

        public BinaryTreeNode Root => root;

        public int MaxLevel => maxLevel;

        public void Add(int value)
        {
            void AddNode(ref BinaryTreeNode node, int currentLevel, BinaryTreeNode parent)
            {
                maxLevel = Math.Max(maxLevel, currentLevel);
                if (node == null)
                    node = new BinaryTreeNode(value, currentLevel, parent);
                else if (value < node.Value)
                    AddNode(ref node.Left, currentLevel + 1, node);
                else if (value > node.Value)
                    AddNode(ref node.Right, currentLevel + 1, node);
            }

            AddNode(ref root, 1, null);
        }

        public bool SearchNode(int value)
        {
            bool Search(BinaryTreeNode node)
            {
                if (node == null)
                    return false;
                if (value == node.Value)
                    return true;
                if (value < node.Value)
                    return Search(node.Left);
                else
                    return Search(node.Right);
            }

            return Search(root);
        }

        public void SaveToFile(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                void WriteNode(BinaryTreeNode node)
                {
                    if (node != null)
                    {
                        writer.WriteLine($"{node.Value} {node.Level}");
                        WriteNode(node.Left);
                        WriteNode(node.Right);
                    }
                }

                writer.WriteLine(maxLevel);
                WriteNode(root);
            }
        }

        public static BinaryTree LoadFromFile(string fileName)
        {
            BinaryTree tree = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            using (StreamReader reader = new StreamReader(fs))
            {
                int maxLevel = int.Parse(reader.ReadLine());
                tree = new BinaryTree();
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    string[] parts = line.Split(' ');
                    int value = int.Parse(parts[0]);
                    int level = int.Parse(parts[1]);
                    if (level > tree.MaxLevel)
                        tree.maxLevel = level;
                    tree.Add(value);
                }
            }
            return tree;
        }

        public string SerializeToString()
        {
            StringBuilder sb = new StringBuilder();
            void PreorderTraversal(BinaryTreeNode node)
            {
                if (node != null)
                {
                    sb.Append($"{node.Value} ");
                    PreorderTraversal(node.Left);
                    PreorderTraversal(node.Right);
                }
            }
            PreorderTraversal(root);
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1); // remove last space 
            return sb.ToString();
        }

        public void RemoveNode(int value)
        {
            void Remove(ref BinaryTreeNode node)
            {
                if (node != null)
                {
                    if (value == node.Value)
                    {
                        if (node.Left == null && node.Right == null)
                            node = null;
                        else if (node.Left == null)
                            node = node.Right;
                        else if (node.Right == null)
                            node = node.Left;
                        else
                        {
                            BinaryTreeNode minNode = node.Right;
                            while (minNode.Left != null)
                                minNode = minNode.Left;
                            node.Value = minNode.Value;
                            Remove(ref node.Right);
                        }
                    }
                    else if (value < node.Value)
                        Remove(ref node.Left);
                    else
                        Remove(ref node.Right);
                }
            }

            Remove(ref root);
        }

        public int FindMinEvenNumberInBranches()
        {
            int FindMinEvenNumberInBranches(BinaryTreeNode node)
            {
                if (node != null)
                {
                    if (node.Left != null || node.Right != null)
                    {
                        if (node.Value % 2 == 0) return Math.Min(node.Value, Math.Min(FindMinEvenNumberInBranches(node.Left), FindMinEvenNumberInBranches(node.Right)));
                        else return Math.Min(FindMinEvenNumberInBranches(node.Left), FindMinEvenNumberInBranches(node.Right));
                    }
                    else return int.MaxValue;
                }
                else return int.MaxValue;
            }

            return FindMinEvenNumberInBranches(root);
        }

        public void Traverse(Action<BinaryTreeNode> action, TraversalType type)
        {
            void PreorderTraversal(BinaryTreeNode node)
            {
                if (node != null)
                {
                    action(node);
                    PreorderTraversal(node.Left);
                    PreorderTraversal(node.Right);
                }
            }
            void InorderTraversal(BinaryTreeNode node)
            {
                if (node != null)
                {
                    InorderTraversal(node.Left);
                    action(node);
                    InorderTraversal(node.Right);
                }
            }

            void PostorderTraversal(BinaryTreeNode node)
            {
                if (node != null)
                {
                    PostorderTraversal(node.Left);
                    PostorderTraversal(node.Right);
                    action(node);
                }
            }

            switch (type)
            {
                case TraversalType.Preorder:
                    PreorderTraversal(root);
                    break;
                case TraversalType.Inorder:
                    InorderTraversal(root);
                    break;
                case TraversalType.Postorder:
                    PostorderTraversal(root);
                    break;
            }
        }
    }
}
