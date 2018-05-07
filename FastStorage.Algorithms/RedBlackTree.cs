using System;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Algorithms.TreeHelpers;
using JetBrains.Annotations;

// ReSharper disable PossibleNullReferenceException

namespace FastStorage.Algorithms
{
    [PublicAPI]
    public partial class RedBlackTree<TKey, TValue>
    {
        private int _itemsCt = 0;
        public int ItemsCt => _itemsCt;

        /// <summary>
        /// Just for simplify (can be used bool instead)
        /// https://ru.wikipedia.org/wiki/Красно-чёрное_дерево
        /// </summary>
        protected enum NodeColor
        {
            Red,
            Black
        }

        protected class Node : IBinNode<Node>
        {
            public TKey Key { get; set; }

            /// <summary>
            /// Keys in the tree can be repeated. We will use single node with list of values in this case.
            /// </summary>
            [NotNull]
            public LinkedList<TValue> Values { get; set; }

            public Node Parent { get; set; }

            public Node LeftChildNode { get; set; }

            public Node RightChildNode { get; set; }

            public NodeColor Color { get; set; }
        }

        protected Node _root;

        protected Node _leafNode = new Node
        {
            Values = new LinkedList<TValue>(),
            Parent = null,
            Color = NodeColor.Black
        };

        [NotNull]
        protected readonly IComparer<TKey> _comparer;

        public RedBlackTree(IComparer<TKey> comparer = null)
        {
            if (comparer == null)
            {
                _comparer = 
                    Comparer<TKey>.Default ?? 
                    throw new ArgumentException($"Comparer for {nameof(TKey)} was not found");
            }
            else
            {
                _comparer = comparer;
            }
        }

        #region RelationHelpers

        /// <summary>
        /// Parent of parent
        /// </summary>
        private Node GrandParent(Node node)
        {
            return node?.Parent?.Parent;
        }

        /// <summary>
        /// Brother of father
        /// </summary>
        private Node Uncle(Node node)
        {
            var grandParent = GrandParent(node);
            if (grandParent == null)
                return null;
            return
                node.Parent == grandParent.LeftChildNode ?
                    grandParent.RightChildNode :
                    grandParent.LeftChildNode;
        }

        #endregion

        #region Private

        private void RotateLeft(Node node)
        {
            var pivot = node.RightChildNode;
            pivot.Parent = node.Parent;
	
            if (node.Parent != null)
            {
                if (node.Parent.LeftChildNode==node)
                    node.Parent.LeftChildNode = pivot;
                else
                    node.Parent.RightChildNode = pivot;
            }

            node.RightChildNode = pivot.LeftChildNode;
            if (pivot.LeftChildNode != null)
                pivot.LeftChildNode.Parent = node;

            node.Parent = pivot;
            pivot.LeftChildNode = node;
        }

        private void RotateRight(Node node)
        {
            var pivot = node.LeftChildNode;
            pivot.Parent = node.Parent;

            if (node.Parent != null)
            {
                if (node.Parent.LeftChildNode == node)
                    node.Parent.LeftChildNode = pivot;
                else
                    node.Parent.RightChildNode = pivot;
            }

            node.LeftChildNode = pivot.RightChildNode;
            if (pivot.RightChildNode != null)
                pivot.RightChildNode.Parent = node;

            node.Parent = pivot;
            pivot.RightChildNode = node;
        }

        #region Insert cases
        private void InsertCase1(Node node)
        {
            if (node.Parent == null)
            {
                node.Color = NodeColor.Black;
            }
            else
            {
                InsertCase2(node);
            }
        }

        private void InsertCase2(Node node)
        {
            if (IsBlack(node.Parent))
            {
                // Tree is still valid
            }
            else
            {
                InsertCase3(node);
            }
        }

        private void InsertCase3(Node node)
        {
            var uncle = Uncle(node);
            if (uncle != null && IsRed(uncle))
            {
                node.Parent.Color = NodeColor.Black;
                uncle.Color = NodeColor.Black;
                var grandParent = GrandParent(node);
                grandParent.Color = NodeColor.Red;
                InsertCase1(grandParent);
            }
            else
            {
                InsertCase4(node);
            }
        }

        private void InsertCase4(Node node)
        {
            var grandParent = GrandParent(node);

            if (node == node.Parent.RightChildNode && node.Parent == grandParent.LeftChildNode)
            {
                RotateLeft(node.Parent);
                if (node.Parent == null)
                    _root = node.LeftChildNode;
                node = node.LeftChildNode;
            }
            else if (node == node.Parent.LeftChildNode && node.Parent == grandParent.RightChildNode)
            {
                RotateRight(node.Parent);
                if (node.Parent == null)
                    _root = node.RightChildNode;
                node = node.RightChildNode;
            }

            InsertCase5(node);
        }

        private void InsertCase5(Node node)
        {
            var grandParent = GrandParent(node);

            node.Parent.Color = NodeColor.Black;
            grandParent.Color = NodeColor.Red;

            if (node == node.Parent.LeftChildNode && node.Parent == grandParent.LeftChildNode)
                RotateRight(grandParent);
            else
                RotateLeft(grandParent);
        }
        #endregion

        private Node FindRoot(Node node)
        {
            var cur = node;
            while (cur.Parent != null)
            {
                cur = cur.Parent;
            }
            return cur;
        }

        protected bool IsPseudoLeaf(Node node)
        {
            return node == _leafNode;
        }

        protected bool IsLeaf(Node node)
        {
            return IsPseudoLeaf(node.LeftChildNode) && IsPseudoLeaf(node.RightChildNode);
        }

        protected bool IsBlack(Node node)
        {
            return node.Color == NodeColor.Black;
        }

        protected bool IsRed(Node node)
        {
            return node.Color == NodeColor.Red;
        }

        private Node CreateNode(TKey key, TValue value, Node parent)
        {
            return new Node
            {
                Key = key,
                Values = new LinkedList<TValue>(new []{ value }),
                Color = NodeColor.Red,
                Parent = parent,
                LeftChildNode = _leafNode,
                RightChildNode = _leafNode
            };
        }

        private void AddToSubtree(Node node, TKey key, TValue value)
        {
            var comparation = _comparer.Compare(key, node.Key);
            if (comparation > 0)
            {
                if (IsPseudoLeaf(node.RightChildNode))
                {
                    var newNode = CreateNode(key, value, node);
                    node.RightChildNode = newNode;
                    InsertCase1(newNode);
                }
                else
                    AddToSubtree(node.RightChildNode, key, value);
            }
            else if (comparation < 0)
            {
                if (IsPseudoLeaf(node.LeftChildNode))
                {
                    var newNode = CreateNode(key, value, node);
                    node.LeftChildNode = newNode;
                    InsertCase1(newNode);
                }
                else
                    AddToSubtree(node.LeftChildNode, key, value);
            }
            else if (comparation == 0)
            {
                node.Values.AddLast(value);
            }
        }

        protected Node FindInSubTree(Node node, TKey key)
        {
            if (node == null || IsPseudoLeaf(node))
                return null;

            var comparation = _comparer.Compare(key, node.Key);
            if (comparation == 0)
                return node;
            else if (comparation > 0)
            {
                return FindInSubTree(node.RightChildNode, key);
            }
            else
            {
                return FindInSubTree(node.LeftChildNode, key);
            }
        }

        /// <summary>
        /// Find path to node with specifed key. (Path starts in root and ends in destination node) 
        /// </summary>
        [NotNull]
        [ItemNotNull]
        protected IEnumerable<Node> FindPathInSubTree(Node node, TKey key)
        {
            if (node == null || IsPseudoLeaf(node))
                yield break;

            yield return node;

            var comparation = _comparer.Compare(key, node.Key);
            if (comparation == 0)
                yield break;
            if (comparation > 0)
            {
                foreach (var nd in FindPathInSubTree(node.RightChildNode, key))
                {
                    yield return nd;
                }
            }
            else
            {
                foreach (var nd in FindPathInSubTree(node.LeftChildNode, key))
                {
                    yield return nd;
                }
            }
        }

        /// <summary>
        /// Find path to deepest left leaf
        /// </summary>
        protected IEnumerable<Node> FindLeftLeafPath(Node node)
        {
            var cur = node;
            yield return cur;
            while (cur.LeftChildNode != null && !IsLeaf(cur))
            {
                cur = cur.LeftChildNode;
                yield return cur;
            }
        }

        /// <summary>
        /// Find path to deepest right leaf
        /// </summary>
        protected IEnumerable<Node> FindRightLeafPath(Node node)
        {
            var cur = node;
            yield return cur;
            while (cur.RightChildNode != null && !IsLeaf(cur))
            {
                cur = cur.RightChildNode;
                yield return cur;
            }
        }

        #endregion

        public void Insert(TKey key, TValue val)
        {
            if (_root == null)
            {
                _root = CreateNode(key, val, null);
                InsertCase1(_root);
            }
            else
            {
                AddToSubtree(_root, key, val);
                _root = FindRoot(_root);
            }
            _itemsCt++;
        }

        /// <summary>
        /// Return values by key.
        /// </summary>
        public IEnumerable<TValue> Get(TKey key)
        {
            var foundNode = FindInSubTree(_root, key);
            return foundNode?.Values ?? Enumerable.Empty<TValue>();
        }

        /// <summary>
        /// Number of specifed elements in collection
        /// </summary>
        public int CountOf(TKey key)
        {
            var foundNode = FindInSubTree(_root, key);
            return foundNode?.Values.Count ?? 0;
        }

        public bool Contains(TKey key)
        {
            return CountOf(key) != 0;
        }
    }
}