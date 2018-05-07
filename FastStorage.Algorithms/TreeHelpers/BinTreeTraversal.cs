using System.Collections.Generic;
using JetBrains.Annotations;

namespace FastStorage.Algorithms.TreeHelpers
{
    public static class BinTreeTraversal
    {
        /// <remarks>https://en.wikipedia.org/wiki/Tree_traversal</remarks>
        [NotNull]
        [ItemNotNull]
        internal static IEnumerable<TNode> InOrderTraversal<TNode>(TNode cur, TNode start, TNode end) where TNode : IBinNode<TNode>
        {
            if (cur == null) yield break;

            if (ReferenceEquals(cur, start) || ReferenceEquals(cur, end))
            {
                yield return cur;
                yield break;
            }

            var leftNodes = InOrderTraversal(cur.LeftChildNode, start, end);
            foreach (var node in leftNodes)
            {
                yield return node;
            }
            yield return cur;
            var rightNodes = InOrderTraversal(cur.RightChildNode, start, end);
            foreach (var node in rightNodes)
            {
                yield return node;
            }
        }
    }
}
