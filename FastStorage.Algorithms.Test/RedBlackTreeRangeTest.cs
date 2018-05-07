using System;
using System.Linq;
using Xunit;

namespace FastStorage.Algorithms.Test
{
    public class RedBlackTreeRangeTest
    {
        private readonly int[] _keys1 = {3, 2, 4, 1, 5, 3, 10};
        private readonly string[] _vals1 = {"k3", "k2", "k4", "k1", "k5", "k3", "k10"};
        private readonly float[] _keys2 = {4.0f, 4.0f, 4.0f, -1.0f, 2.0f, 5.0f, 3.0f, -7.0f, 10.0f};
        private readonly string[] _vals2 = {"k4.0", "k4.0", "k4.0", "k-1.0", "k2.0", "k5.0", "k3.0", "k-7.0", "k10.0"};

        [Fact]
        public void LeftInnerRightOuterTest1()
        {
            var rangeTree = new RedBlackTree<int, string>();
            InsertMass(rangeTree, _keys1, _vals1);

            var r = rangeTree.GetInRange(2, 30).ToArray();
            Assert.Equal(new[] { "k2", "k3", "k3", "k4", "k5", "k10" }, r);
        }

        [Fact]
        public void LeftInnerRightOuterTest2()
        {
            var rangeTree = new RedBlackTree<float, string>();
            InsertMass(rangeTree, _keys2, _vals2);

            var r = rangeTree.GetInRange(4, 89);
            Assert.Equal(new[] { "k4.0", "k4.0", "k4.0", "k5.0", "k10.0" }, r);
        }

        [Fact]
        public void LeftOuterRightInnerTest1()
        {
            var rangeTree = new RedBlackTree<int, string>();
            InsertMass(rangeTree, _keys1, _vals1);

            var r1 = rangeTree.GetInRange(-10, 3).ToArray();
            Assert.Equal(new[] { "k1", "k2", "k3", "k3" }, r1);
        }

        [Fact]
        public void LeftOuterRightInnerTest2()
        {
            var rangeTree = new RedBlackTree<float, string>();
            InsertMass(rangeTree, _keys2, _vals2);

            var r = rangeTree.GetInRange(-60, 5);
            Assert.Equal(new[] { "k-7.0", "k-1.0", "k2.0", "k3.0", "k4.0", "k4.0", "k4.0", "k5.0" }, r);
        }

        [Fact]
        public void LeftInnerRightInnerTest1()
        {
            var rangeTree = new RedBlackTree<int, string>();
            InsertMass(rangeTree, _keys1, _vals1);

            var r = rangeTree.GetInRange(2, 4).ToArray();
            Assert.Equal(new[] { "k2", "k3", "k3", "k4" }, r);
        }

        [Fact]
        public void LeftInnerRightInnerTest2()
        {
            var rangeTree = new RedBlackTree<float, string>();
            InsertMass(rangeTree, _keys2, _vals2);

            var r = rangeTree.GetInRange(3.0f, 4.0f).ToArray();
            Assert.Equal(new[] { "k3.0", "k4.0", "k4.0", "k4.0" }, r);
        }

        [Fact]
        public void LeftOuterRightOuterTest1()
        {
            var rangeTree = new RedBlackTree<int, string>();
            InsertMass(rangeTree, _keys1, _vals1);

            var r = rangeTree.GetInRange(-30, 40).ToArray();
            Assert.Equal(new[] { "k1", "k2", "k3", "k3", "k4", "k5", "k10" }, r);
        }

        [Fact]
        public void LeftOuterRightOuterTest2()
        {
            var rangeTree = new RedBlackTree<float, string>();
            InsertMass(rangeTree, _keys2, _vals2);

            var r = rangeTree.GetInRange(-60, 80).ToArray();
            Assert.Equal(new[] { "k-7.0", "k-1.0", "k2.0", "k3.0", "k4.0", "k4.0", "k4.0", "k5.0", "k10.0" }, r);
        }

        [Fact]
        public void GetGreaterOrEqualTest1()
        {
            var rangeTree = new RedBlackTree<int, string>();
            InsertMass(rangeTree, _keys1, _vals1);

            var r = rangeTree.GetGreater(3).ToArray();
            Assert.Equal(new[] { "k3", "k3", "k4", "k5", "k10" }, r);
        }

        [Fact]
        public void GetGreaterOrEqualTest2()
        {
            var rangeTree = new RedBlackTree<float, string>();
            InsertMass(rangeTree, _keys2, _vals2);

            var r = rangeTree.GetGreater(2.3f).ToArray();
            Assert.Equal(new[] { "k3.0", "k4.0", "k4.0", "k4.0", "k5.0", "k10.0" }, r);
        }

        [Fact]
        public void GetLessOrEqualTest1()
        {
            var rangeTree = new RedBlackTree<int, string>();
            InsertMass(rangeTree, _keys1, _vals1);

            var r = rangeTree.GetLess(4).ToArray();
            Assert.Equal(new[] { "k1", "k2", "k3", "k3", "k4" }, r);
        }

        [Fact]
        public void GetLessOrEqualTest2()
        {
            var rangeTree = new RedBlackTree<float, string>();
            InsertMass(rangeTree, _keys2, _vals2);

            var r = rangeTree.GetLess(3.1f).ToArray();
            Assert.Equal(new[] { "k-7.0", "k-1.0", "k2.0", "k3.0" }, r);
        }

        /// <summary>
        /// Insert array of values. Value = -Key
        /// </summary>
        private static void InsertMass<TKey, TValue>(RedBlackTree<TKey, TValue> tree, TKey[] keys, TValue[] vals)
        {
            if (keys.Length != vals.Length)
                throw new ArgumentException();

            for (int i = 0; i < keys.Length; i++)
            {
                tree.Insert(keys[i], vals[i]);
            }
        }
    }
}
