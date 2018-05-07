namespace FastStorage.Execution.ExecutorsCore.Complexity
{
    /// <summary>
    /// This class is used as one point where all complexity constants are defined.
    /// </summary>
    internal static class ComplexityConstants
    {
        /// <summary>
        /// O(1)
        /// </summary>
        public static Complexity Constant => new Complexity(0.1f, "O(1)");

        /// <summary>
        /// O(log log n)
        /// </summary>
        public static Complexity DoubleLogarithmic => new Complexity(0.2f, "O(log log n)");

        /// <summary>
        /// O(log n)
        /// </summary>
        public static Complexity Logarithmic => new Complexity(0.3f, "O(log n)");

        /// <summary>
        /// O((log n)^c)
        /// </summary>
        public static Complexity Polylogarithmic => new Complexity(0.4f, "O((log n)^c)");

        /// <summary>
        /// O(n)
        /// </summary>
        public static Complexity Linear => new Complexity(0.5f, "O(n)");

        /// <summary>
        /// O(n log* n)
        /// </summary>
        public static Complexity NLogStarN => new Complexity(0.6f, "O(n log* n)");

        /// <summary>
        /// O(n log n)
        /// </summary>
        public static Complexity Linearithmic => new Complexity(0.7f, "O(n log n)");

        /// <summary>
        /// O(n^2)
        /// </summary>
        public static Complexity Quadratic => new Complexity(0.8f, "O(n^2)");

        /// <summary>
        /// O(n^c)
        /// </summary>
        public static Complexity Polynomial => new Complexity(0.9f, "O(n^c)");

        /// <summary>
        /// O(c^n)
        /// </summary>
        public static Complexity Exponential => new Complexity(0.95f, "O(c^n)");

        /// <summary>
        /// O(n!)
        /// </summary>
        public static Complexity Factorial => new Complexity(1.0f, "O(n!)");
    }

    internal class Complexity : IComplexity
    {
        /// <inheritdoc />
        public float Weight { get; }

        /// <inheritdoc />
        public string DebugView { get; }

        public Complexity(float weight, string debugView)
        {
            Weight = weight;
            DebugView = debugView;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return DebugView;
        }
    }
}