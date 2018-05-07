using System;
using System.Linq.Expressions;
using FastStorage.Expressions.EqualityComparer;
using FastStorage.Expressions.Helpers;
using NUnit.Framework;

namespace FastStorage.Test.Expressions.Helpers
{
    [TestFixture]
    public class LambdaParametersReplacerTest
    {
        static class A
        {
            public static float F1 { get; set; }

            public static int F2 { get; set; }
        }

        [Test]
        public void ReplaceOneParameterTest1()
        {
            Expression<Func<float, float>> expr = x => x * 2 + 3;
            Expression<Func<float, float>> exprReplacedValid = x => A.F1 * 2 + 3;
            var exprReplaced = LambdaParametersReplacer.ReplaceLambdaParameters(expr, new []{ Expression.Property(null, typeof(A), nameof(A.F1))});
            Assert.IsTrue(CompareExpressionsExtensions.AreEqual(exprReplaced, exprReplacedValid.Body));
        }

        [Test]
        public void ReplaceOneParameterTest2()
        {
            Expression<Func<float, float>> expr = x => x * 2 + x * 3;
            Expression<Func<float, float>> exprReplacedValid = x => A.F1 * 2 + A.F1 * 3;
            var exprReplaced = LambdaParametersReplacer.ReplaceLambdaParameters(expr, new[] { Expression.Property(null, typeof(A), nameof(A.F1)) });
            Assert.IsTrue(CompareExpressionsExtensions.AreEqual(exprReplaced, exprReplacedValid.Body));
        }

        [Test]
        public void ReplaceOneParameterTest3()
        {
            Expression<Func<float, float>> expr = x => (int)Math.Truncate(x * 2 + x * 3);
            Expression<Func<float, float>> exprReplacedValid = x => (int)Math.Truncate(A.F1 * 2 + A.F1 * 3);
            var exprReplaced = LambdaParametersReplacer.ReplaceLambdaParameters(expr, new[] { Expression.Property(null, typeof(A), nameof(A.F1)) });
            Assert.IsTrue(CompareExpressionsExtensions.AreEqual(exprReplaced, exprReplacedValid.Body));
        }

        [Test]
        public void ReplaceMultipleParametersTest1()
        {
            Expression<Func<float, int, float>> expr = (x, y) => x * 2 + y * 3;
            Expression<Func<float, int, float>> exprReplacedValid = (x, y) => A.F1 * 2 + A.F2 * 3;
            var exprReplaced = LambdaParametersReplacer.ReplaceLambdaParameters(expr, new[] { Expression.Property(null, typeof(A), nameof(A.F1)), Expression.Property(null, typeof(A), nameof(A.F2)) });
            Assert.IsTrue(CompareExpressionsExtensions.AreEqual(exprReplaced, exprReplacedValid.Body));
        }
    }
}
