using System;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Algorithms.TreeHelpers;
using JetBrains.Annotations;

namespace FastStorage.Algorithms
{
    /// <summary>
    /// Extension of red black tree for fast range operations
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/Range_tree</remarks>
    public partial class RedBlackTree<TKey, TValue>
    {
        /// <summary>
        /// Return values which keys are in range (including bounds).
        /// </summary>
        /// <remarks>
        /// Result will be sorted by key.
        /// </remarks>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<TValue> GetInRange(TKey value1, TKey value2)
        {
            var path1 = FindPathInSubTree(_root, value1).ToArray();
            var path2 = FindPathInSubTree(_root, value2).ToArray();

            return DoPathsTraversal(path1, path2, value1, value2);
        }

        /// <summary>
        /// Return values which keys are greater or equal of specifed (including bound).
        /// </summary>
        /// <remarks>
        /// Result will be sorted by key.
        /// </remarks>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<TValue> GetGreater(TKey value, bool includeBound = true)
        {
            var path1 = FindPathInSubTree(_root, value).ToArray();
            var path2 = FindRightLeafPath(_root).ToArray();

            return DoPathsTraversal(path1, path2, value, default(TKey), 
                ignoreValue2: true, 
                excludeLeftBound: !includeBound);
        }

        /// <summary>
        /// Return values which keys are less or equal of specifed (including bound).
        /// </summary>
        /// <remarks>
        /// Result will be sorted by key.
        /// </remarks>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<TValue> GetLess(TKey value, bool includeBound = true)
        {
            var path1 = FindLeftLeafPath(_root).ToArray();
            var path2 = FindPathInSubTree(_root, value).ToArray();

            return DoPathsTraversal(path1, path2, default(TKey), value, 
                ignoreValue1: true, 
                excludeRightBound: !includeBound);
        }

        /// <summary>
        /// This method uses already found paths and return values in right order (see range tree description).
        /// </summary>
        /// <param name="path1">Path to node with lower value</param>
        /// <param name="path2">Path to node with high value</param>
        /// <param name="value1">Lower value</param>
        /// <param name="value2">High value</param>
        /// <param name="ignoreValue1">It's hack for ranges where lower bound is not specifed</param>
        /// <param name="ignoreValue2">It's hack for ranges where high bound is not specifed</param>
        /// <remarks>
        ///     This method was created as independent for ability to inject paths. 
        ///     F.e. when lower or high bound is not specifed and we need to return
        ///     all values which are Greater or Lower then specifed.
        /// </remarks>
        private IEnumerable<TValue> DoPathsTraversal(
            Node[] path1, 
            Node[] path2, 
            TKey value1, 
            TKey value2, 
            bool ignoreValue1 = false, 
            bool ignoreValue2 = false,
            bool excludeLeftBound = false,
            bool excludeRightBound = false)
        {
            // Find node where path1 and path2 are splitted
            int i = 0;
            while (i < path1.Length && i < path2.Length && path1[i] == path2[i])
            {
                i++;
            }

            // Path to x1 (started in splitted node);
            foreach (var res in LeftSubtreeTraversal(
                path1.Skip(i).Reverse(), 
                key => ignoreValue1 || _comparer.Compare(key, value1) >= 0))
            {
                if (excludeLeftBound && _comparer.Compare(res.Key, value1) == 0)
                    continue;
                foreach (var resValue in res.Values)
                {
                    yield return resValue;
                }
            }

            // Do not forget splitted node
            var splittedNode = path1[i - 1];
            foreach (var val in splittedNode.Values)
                yield return val;

            // Path to x2 (started in splitted node)

            foreach (var res in RightSubtreeTraversal(
                path2.Skip(i), 
                key => ignoreValue2 || _comparer.Compare(key, value2) <= 0))
            {
                if (excludeRightBound && _comparer.Compare(res.Key, value2) == 0)
                    continue;
                foreach (var resValue in res.Values)
                {
                    yield return resValue;
                }
            }
        }

        /// <summary>
        /// It is part of algorithm where we walk over left subtree (of found splitted node) 
        /// (see alg description for more information)
        /// </summary>
        private IEnumerable<Node> LeftSubtreeTraversal(IEnumerable<Node> path, Func<TKey, bool> addToResult)
        {
            foreach (var v in path)
            {
                if (addToResult(v.Key))
                {
                    // 1) return v 
                    // 2) return right subtree of v
                    yield return v;
                    foreach (var nd in BinTreeTraversal.InOrderTraversal(v.RightChildNode, null, null))
                    {
                        yield return nd;
                    }
                }
            }
        }

        /// <summary>
        /// It is part of algorithm where we walk over right subtree (of found splitted node)
        /// (see alg description for more information)
        /// </summary>
        private IEnumerable<Node> RightSubtreeTraversal(IEnumerable<Node> path, Func<TKey, bool> addToResult)
        {
            foreach (var v in path)
            {
                if (addToResult(v.Key))
                {
                    // 1) return left subtree of v
                    // 2) return v
                    foreach (var nd in BinTreeTraversal.InOrderTraversal(v.LeftChildNode, null, null))
                    {
                        yield return nd;
                    }
                    yield return v;
                }
            }
        }
    }
}
