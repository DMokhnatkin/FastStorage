namespace FastStorage.Execution.ExecutorsCore.Complexity
{
    internal interface IComplexity
    {
        /// <summary>
        /// When we can execute operation different ways we need an ability to select fastest.
        /// This property sets some float to each time complexity, so we can compare complexities just like floats. 
        /// </summary>
        float Weight { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string DebugView { get; }
    }
}