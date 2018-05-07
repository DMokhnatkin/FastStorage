namespace FastStorage.Algorithms.TreeHelpers
{
    internal interface IBinNode<TNode>
    {
        TNode LeftChildNode { get; set; }

        TNode RightChildNode { get; set; }
    }
}
