namespace BinaryTrey.Tests
{
    [TestFixture]
    public class BinaryTreeArrayTests
    {
        [Test]
        public void ConstructorWithNodes_SetsCorrectRoot()
        {
            var tree = new BinaryTreeArray<int>("1 2 3");
            Assert.AreEqual(1, tree.GetRoot());
        }

        [Test]
        public void SetLeft_SetsCorrectNode()
        {
            var tree = new BinaryTreeArray<int>(3);
            tree.SetLeft(2, 4);
            Assert.AreEqual(4, tree.GetLeft(2));
        }

        [Test]
        public void SetRight_SetsCorrectNode()
        {
            var tree = new BinaryTreeArray<int>(3);
            tree.SetRight(2, 5);
            Assert.AreEqual(5, tree.GetRight(2));
        }

        [Test]
        public void SearchNode_ReturnsCorrectIndex()
        {
            var tree = new BinaryTreeArray<int>("1 2 3");
            Assert.AreEqual(2, tree.SearchNode(3));
        }

        [Test]
        public void FindSmallestEvenNode_ReturnsCorrectValue()
        {
            var tree = new BinaryTreeArray<int>("1 2 3 4 5 6");
            Assert.AreEqual(2, tree.FindSmallestEvenNode());
        }

        [Test]
        public void Traverse_PreorderTraversal_CorrectOrder()
        {
            var tree = new BinaryTreeArray<int>("1 2 3 4 5 6 7");
            var expected = "1, 2, 4, 5, 3, 6, 7";
            var actual = string.Empty;
            tree.Traverse(x => actual += $"{x}, ", TraversalType.Preorder);
            Assert.AreEqual(expected, actual.TrimEnd(',', ' '));
        }

        [Test]
        public void Traverse_InorderTraversal_CorrectOrder()
        {
            var tree = new BinaryTreeArray<int>("1 2 3 4 5 6 7");
            var expected = "4, 2, 5, 1, 6, 3, 7";
            var actual = string.Empty;
            tree.Traverse(x => actual += $"{x}, ", TraversalType.Inorder);
            Assert.AreEqual(expected, actual.TrimEnd(',', ' '));
        }

        [Test]
        public void Traverse_PostorderTraversal_CorrectOrder()
        {
            var tree = new BinaryTreeArray<int>("1 2 3 4 5 6 7");
            var expected = "4, 5, 2, 6, 7, 3, 1";
            var actual = string.Empty;
            tree.Traverse(x => actual += $"{x}, ", TraversalType.Postorder);
            Assert.AreEqual(expected, actual.TrimEnd(',', ' '));
        }
    }
}
