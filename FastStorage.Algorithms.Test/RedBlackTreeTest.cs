using JetBrains.Annotations;
using Xunit;

namespace FastStorage.Algorithms.Test
{
    public class RedBlackTreeTest
    {
        [NotNull]
        private readonly int[] _sampleData1 = { 3, 5, 5, 7, 8, 9, 9, 9 };
        [NotNull]
        private readonly float[] _sampleData2 = { -5, 10.5f, 8, 1, 4, 8.1f, 4, 8.1f };

        [Fact]
        public void TestCase1()
        {
            var tree = new RedBlackTree<int, int>();

            for (int i = 0; i < _sampleData1.Length; i++)
            {
                tree.Insert(_sampleData1[i], i);
            }

            Assert.Equal(1, tree.CountOf(3));
            Assert.Equal(2, tree.CountOf(5));
            Assert.Equal(1, tree.CountOf(7));
            Assert.Equal(1, tree.CountOf(8));
            Assert.Equal(3, tree.CountOf(9));
        }

        [Fact]
        public void TestCase2()
        {
            var tree = new RedBlackTree<float, int>();
            for (int i = 0; i < _sampleData2.Length; i++)
            {
                tree.Insert(_sampleData2[i], i);
            }

            Assert.Equal(1, tree.CountOf(-5));
            Assert.Equal(1, tree.CountOf(10.5f));
            Assert.Equal(1, tree.CountOf(8));
            Assert.Equal(1, tree.CountOf(1));
            Assert.Equal(2, tree.CountOf(4));
            Assert.Equal(2, tree.CountOf(8.1f));
        }
    }
}
