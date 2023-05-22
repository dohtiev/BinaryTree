namespace BinaryTrey.Tests
{
    [TestFixture]
    public class BinaryTreeTests
    {
        [Test]
        public void BinaryTreeArray_SetRoot_SetsRoot()
        {
            BinaryTreeArray<int> tree = new BinaryTreeArray<int>(3);
            tree.SetRoot(10);

            Assert.AreEqual(10, tree.GetRoot());
        }

        [Test]
        public void BinaryTreeArray_SetLeft_SetsLeftChild()
        {
            BinaryTreeArray<int> tree = new BinaryTreeArray<int>(3);
            tree.SetRoot(10);
            tree.SetLeft(0, 20);

            Assert.AreEqual(20, tree.GetLeft(0));
        }

        [Test]
        public void BinaryTreeArray_SetRight_SetsRightChild()
        {
            BinaryTreeArray<int> tree = new BinaryTreeArray<int>(3);
            tree.SetRoot(10);
            tree.SetRight(0, 30);

            Assert.AreEqual(30, tree.GetRight(0));
        }

        [Test]
        public void BinaryTreeArray_SearchNode_ReturnsCorrectIndex()
        {
            BinaryTreeArray<int> tree = new BinaryTreeArray<int>("10 20 30 40 50");
            int index = tree.SearchNode(40);

            Assert.AreEqual(3, index);
        }

        [Test]
        public void BinaryTreeArray_FindSmallestEvenNode_ReturnsSmallestEvenNode()
        {
            BinaryTreeArray<int> tree = new BinaryTreeArray<int>("10 5 12 3 9 14 17");
            int smallestEvenNode = tree.FindSmallestEvenNode();

            Assert.AreEqual(10, smallestEvenNode);
        }

        [Test]
        public void BinaryTreeArray_Traverse_PreorderTraversal_CallsActionInCorrectOrder()
        {
            BinaryTreeArray<int> tree = new BinaryTreeArray<int>("1 2 3 4 5 6 7");
            List<int> values = new List<int>();

            tree.Traverse(x => values.Add(x), TraversalType.Preorder);

            Assert.AreEqual(new List<int> { 1, 2, 4, 5, 3, 6, 7 }, values);
        }

        [Test]
        public void BinaryTreeArray_Traverse_InorderTraversal_CallsActionInCorrectOrder()
        {
            BinaryTreeArray<int> tree = new BinaryTreeArray<int>("1 2 3 4 5 6 7");
            List<int> values = new List<int>();

            tree.Traverse(x => values.Add(x), TraversalType.Inorder);

            Assert.AreEqual(new List<int> { 4, 2, 5, 1, 6, 3, 7 }, values);
        }

        [Test]
        public void BinaryTreeArray_Traverse_PostorderTraversal_CallsActionInCorrectOrder()
        {
            BinaryTreeArray<int> tree = new BinaryTreeArray<int>("1 2 3 4 5 6 7");
            List<int> values = new List<int>();

            tree.Traverse(x => values.Add(x), TraversalType.Postorder);

            Assert.AreEqual(new List<int> { 4, 5, 2, 6, 7, 3, 1 }, values);
        }
    }
}
