using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BinaryTrey
{
    public partial class MainWindow : Form
    {

        public static BinaryTree Tree;
        public static BinaryTreeArray<int> TreeArray;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearImage(PictureBox pBox)
        {
            pBox.Image = null;
        }

        private void SetZoomMode(PictureBox pBox)
        {
            if (pBox.Image != null)
            {
                if (pBox.Image.Height > pBox.Height | pBox.Image.Width > pBox.Width)
                    pBox.SizeMode = PictureBoxSizeMode.Zoom;
                else
                    pBox.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }

        private Bitmap CreateBitmap(BinaryTree tree)
        {
            int horizontalOffset = 0, x, y;
            var coords = new Dictionary<BinaryTreeNode, Tuple<int, int>>();

            GetCoords(tree.Root, 0);

            int w = horizontalOffset * 50 - 5;
            int h = tree.MaxLevel * 60 - 16;
            Bitmap bmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Black, 2);
            SolidBrush br = new SolidBrush(pictureBox_tree1.BackColor);
            Font font = new Font("Arial", 18, FontStyle.Regular);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            DrawBranches();
            DrawNodes();
            return bmp;

            void GetCoords(BinaryTreeNode current, int depth)
            {
                if (current != null)
                {
                    if (current.Left != null) GetCoords(current.Left, depth + 1);
                    x = horizontalOffset * 50 + 22;
                    y = depth * 60 + 22;
                    horizontalOffset++;
                    coords.Add(current, new Tuple<int, int>(x, y));
                    if (current.Right != null) GetCoords(current.Right, depth + 1);
                }
            }

            void DrawNodes()
            {
                foreach (var i in coords)
                {
                    int thisX = i.Value.Item1;
                    int thisY = i.Value.Item2;
                    g.FillEllipse(br, thisX - 20, thisY - 20, 40, 40);
                    g.DrawEllipse(pen, thisX - 20, thisY - 20, 40, 40);
                    string valueString = i.Key.Value.ToString();
                    g.DrawString(valueString, font, Brushes.Black, thisX - g.MeasureString(valueString, font).ToSize().Width / 2, thisY - 14);
                }
            }

            void DrawBranches()
            {
                foreach (var i in coords)
                {
                    int thisX = i.Value.Item1;
                    int thisY = i.Value.Item2;
                    if (i.Key.Parent != null) g.DrawLine(pen, coords[i.Key.Parent].Item1, coords[i.Key.Parent].Item2, thisX, thisY);
                }
            }
        }

        private void button_makeTree_Click(object sender, EventArgs e)
        {
            string input = this.textBox_treeSequenceInput.Text;
            int[] seq;
            try
            {
                seq = input.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
                if (seq.Length == 0) throw new InvalidOperationException("Последовательность пуста. Введите хотя бы одно число.");
            }
            catch (Exception ex) when (
                ex is FormatException ||
                ex is OverflowException ||
                ex is InvalidOperationException)
            {
                string title = "Неверный ввод";
                string msg = "";
                if (ex is FormatException) msg = "Введённая строка должна состоять только из целых чисел и пробелов между ними.";
                else if (ex is OverflowException) msg = "Одно или несколько из введённых чисел находятся вне допустимого диапазона.\n\nПожалуйста, используйте числа от -2147483648 до 2147483647.";
                else if (ex is InvalidOperationException) msg = ex.Message;
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            label_act3.ForeColor = Color.Black;

            Tree = new BinaryTree(seq);


            pictureBox_tree1.Image = CreateBitmap(Tree);
            SetZoomMode(pictureBox_tree1);
        }

        private void button_act2_Click_1(object sender, EventArgs e)
        {
            if (Tree == null)
            {
                MessageBox.Show($"Перед вычислением показателя нужно сначала построить дерево.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = CTimer.Timer(() => Tree.FindMinEvenNumberInBranches());
            label9.Text = result.Item1;
            int minEvenInBranches = result.Item2;

            if (minEvenInBranches != int.MaxValue)
            {
                label_act3.Text = "Результат: " + minEvenInBranches.ToString();
                label_act3.ForeColor = Color.Green;
            }
            else
            {
                label_act3.Text = "Среди ветвей дерева чётные элементы не найдены.";
                label_act3.ForeColor = Color.Red;
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            SetZoomMode(pictureBox_tree1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tree.SaveToFile("date.txt");

            pictureBox_tree1.Image = CreateBitmap(Tree);
            SetZoomMode(pictureBox_tree1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Tree = BinaryTree.LoadFromFile("date.txt");
            textBox_treeSequenceInput.Text = Tree.SerializeToString();
            TreeArray = new BinaryTreeArray<int>(textBox_treeSequenceInput.Text);
            pictureBox_tree1.Image = CreateBitmap(Tree);
            SetZoomMode(pictureBox_tree1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int item = 0;
            try
            {
                item = Int32.Parse(textBox1.Text);
            }
            catch (Exception ex) when (
                ex is FormatException ||
                ex is OverflowException ||
                ex is InvalidOperationException)
            {
                string title = "Неверный ввод";
                string msg = "";
                if (ex is FormatException) msg = "Введённая строка должна состоять только из целых чисел";
                else if (ex is OverflowException) msg = "Одно или несколько из введённых чисел находятся вне допустимого диапазона.\n\nПожалуйста, используйте числа от -2147483648 до 2147483647.";
                else if (ex is InvalidOperationException) msg = ex.Message;
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = CTimer.Timer(() => Tree.SearchNode(item));
            label9.Text = result.Item1;
            var found = result.Item2;

            label_act3.Text = "Результат: " + (found && item != 0).ToString();
            if (found && item != 0)
            {
                label_act3.ForeColor = Color.Green;
            }
            else
            {
                label_act3.ForeColor = Color.Red;
            }

        }

        private void buttonTraverse_Click(object sender, EventArgs e)
        {

            //var result;// = CTimer.Timer(() => );
            if (radioButtonPreorder.Checked)
                label9.Text = CTimer.Timer(() => Tree.Traverse(node =>
                {
                    var treeNode = new TreeNode(node.Value.ToString());
                }, TraversalType.Preorder));
            else if (radioButtonInorder.Checked)
                label9.Text = CTimer.Timer(() => Tree.Traverse(node =>
                {
                    var treeNode = new TreeNode(node.Value.ToString());
                }, TraversalType.Inorder));

            else if (radioButtonPostorder.Checked)
                label9.Text = CTimer.Timer(() => Tree.Traverse(node =>
                {
                    var treeNode = new TreeNode(node.Value.ToString());
                }, TraversalType.Postorder));
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                label9.Text = CTimer.Timer(() => TreeArray.Traverse(node =>
                {
                    var treeNode = new TreeNode(node.ToString());
                }, TraversalType.Preorder));
            else if (radioButton2.Checked)
                label9.Text = CTimer.Timer(() => TreeArray.Traverse(node =>
                {
                    var treeNode = new TreeNode(node.ToString());
                }, TraversalType.Inorder));

            else if (radioButton1.Checked)
                label9.Text = CTimer.Timer(() => Tree.Traverse(node =>
                {
                    var treeNode = new TreeNode(node.ToString());
                }, TraversalType.Postorder));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TreeArray = new BinaryTreeArray<int>(textBox_treeSequenceInput.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int item = 0;
            try
            {
                item = Int32.Parse(textBox1.Text);
            }
            catch (Exception ex) when (
                ex is FormatException ||
                ex is OverflowException ||
                ex is InvalidOperationException)
            {
                string title = "Неверный ввод";
                string msg = "";
                if (ex is FormatException) msg = "Введённая строка должна состоять только из целых чисел";
                else if (ex is OverflowException) msg = "Одно или несколько из введённых чисел находятся вне допустимого диапазона.\n\nПожалуйста, используйте числа от -2147483648 до 2147483647.";
                else if (ex is InvalidOperationException) msg = ex.Message;
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = CTimer.Timer(() => TreeArray.SearchNode(item));
            label9.Text = result.Item1;
            var found = result.Item2;

            label_act3.Text = "Результат: " + found.ToString();
            if (found != -1)
            {
                label_act3.ForeColor = Color.Green;
            }
            else
            {
                label_act3.ForeColor = Color.Red;
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (TreeArray == null)
            {
                MessageBox.Show($"Перед вычислением показателя нужно сначала построить дерево.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = CTimer.Timer(() => TreeArray.FindSmallestEvenNode());
            label9.Text = result.Item1;
            int minEvenInBranches = result.Item2;

            if (minEvenInBranches != int.MaxValue)
            {
                label_act3.Text = "Результат: " + minEvenInBranches.ToString();
                label_act3.ForeColor = Color.Green;
            }
            else
            {
                label_act3.Text = "Среди ветвей дерева чётные элементы не найдены.";
                label_act3.ForeColor = Color.Red;
            }
        }
    }
}