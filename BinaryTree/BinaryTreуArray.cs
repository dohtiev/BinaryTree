using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrey
{
    public class BinaryTreeArray<T>
    {
        private T[] _nodes;

        public BinaryTreeArray(int depth)
        {
            _nodes = new T[(int)Math.Pow(2, depth + 1) - 1];
        }
        public static int Log2(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentException("Argument must be positive", nameof(n));
            }

            int result = 0;

            while (n > 1)
            {
                n >>= 1;
                result++;
            }

            return result;
        }

        public BinaryTreeArray(string nodes)
        {
            string[] nodeValues = nodes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int depth = (int)Math.Ceiling(Convert.ToDecimal(Log2(nodeValues.Length + 1)));
            _nodes = new T[(int)Math.Pow(2, depth + 1) - 1];

            if (nodeValues.Length > 0)
            {
                _nodes[0] = (T)Convert.ChangeType(nodeValues[0], typeof(T));
            }

            for (int i = 1; i < nodeValues.Length; i++)
            {
                int parentIndex = (i - 1) / 2;

                if (i % 2 == 1)
                {
                    _nodes[2 * parentIndex + 1] = (T)Convert.ChangeType(nodeValues[i], typeof(T));
                }
                else
                {
                    _nodes[2 * parentIndex + 2] = (T)Convert.ChangeType(nodeValues[i], typeof(T));
                }
            }
        }

        public void SetRoot(T value)
        {
            _nodes[0] = value;
        }

        public void SetLeft(int index, T value)
        {
            _nodes[2 * index + 1] = value;
        }

        public void SetRight(int index, T value)
        {
            _nodes[2 * index + 2] = value;
        }

        public T GetRoot()
        {
            return _nodes[0];
        }

        public T GetLeft(int index)
        {
            return _nodes[2 * index + 1];
        }

        public T GetRight(int index)
        {
            return _nodes[2 * index + 2];
        }

        public int SearchNode(T value)
        {
            return SearchNodeRecursive(0, value);
        }
        public T FindSmallestEvenNode()
        {
            return FindSmallestEvenNodeRecursive(0, default(T));
        }

        private T ParseValue(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        private T FindSmallestEvenNodeRecursive(int index, T smallestEvenNode)
        {
            if (Comparer<T>.Default.Compare(_nodes[index], default(T)) != 0 && IsEven(_nodes[index]) && (Comparer<T>.Default.Compare(smallestEvenNode, default(T)) == 0 || Comparer<T>.Default.Compare(_nodes[index], smallestEvenNode) < 0))
            {
                smallestEvenNode = _nodes[index];
            }

            if (2 * index + 1 < _nodes.Length && Comparer<T>.Default.Compare(_nodes[2 * index + 1], default(T)) != 0)
            {
                smallestEvenNode = FindSmallestEvenNodeRecursive(2 * index + 1, smallestEvenNode);
            }

            if (2 * index + 2 < _nodes.Length && Comparer<T>.Default.Compare(_nodes[2 * index + 2], default(T)) != 0)
            {
                smallestEvenNode = FindSmallestEvenNodeRecursive(2 * index + 2, smallestEvenNode);
            }

            return smallestEvenNode;
        }

        private bool IsEven(T value)
        {
            int intValue = Convert.ToInt32(value);
            return intValue % 2 == 0;
        }

        private int SearchNodeRecursive(int index, T value)
        {
            if (Comparer<T>.Default.Compare(_nodes[index], value) == 0)
            {
                return index;
            }

            int result = -1;

            if (2 * index + 1 < _nodes.Length && Comparer<T>.Default.Compare(_nodes[2 * index + 1], default(T)) != 0)
            {
                result = SearchNodeRecursive(2 * index + 1, value);
            }

            if (result == -1 && 2 * index + 2 < _nodes.Length && Comparer<T>.Default.Compare(_nodes[2 * index + 2], default(T)) != 0)
            {
                result = SearchNodeRecursive(2 * index + 2, value);
            }

            return result;
        }

        public void Traverse(Action<T> action, TraversalType type)
        {
            void PreorderTraversal(int index)
            {
                if (index >= _nodes.Length || Comparer<T>.Default.Compare(_nodes[index], default(T)) == 0)
                {
                    return;
                }

                action(_nodes[index]);
                PreorderTraversal(2 * index + 1);
                PreorderTraversal(2 * index + 2);
            }

            void InorderTraversal(int index)
            {
                if (index >= _nodes.Length || Comparer<T>.Default.Compare(_nodes[index], default(T)) == 0)
                {
                    return;
                }

                InorderTraversal(2 * index + 1);
                action(_nodes[index]);
                InorderTraversal(2 * index + 2);
            }

            void PostorderTraversal(int index)
            {
                if (index >= _nodes.Length || Comparer<T>.Default.Compare(_nodes[index], default(T)) == 0)
                {
                    return;
                }

                PostorderTraversal(2 * index + 1);
                PostorderTraversal(2 * index + 2);
                action(_nodes[index]);
            }

            switch (type)
            {
                case TraversalType.Preorder:
                    PreorderTraversal(0);
                    break;
                case TraversalType.Inorder:
                    InorderTraversal(0);
                    break;
                case TraversalType.Postorder:
                    PostorderTraversal(0);
                    break;
            }
        }

    }

}
