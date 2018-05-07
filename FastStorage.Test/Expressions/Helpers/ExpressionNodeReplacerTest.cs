using System;
using System.Linq.Expressions;
using FastStorage.Expressions.EqualityComparer;
using FastStorage.Expressions.Helpers;
using NUnit.Framework;

namespace FastStorage.Test.Expressions.Helpers
{
    [TestFixture]
    public class ExpressionNodeReplacerTest
    {
        static class A
        {
            public static float F1 { get; set; }

            public static int F2 { get; set; }
        }

        [Test]
        public void ReplaceTest1()
        {
            Expression<Func<float, float>> expr = x => x * 2 + 3;
            Expression<Func<float, float>> exprReplacedValid = x => x * 8 + 3;
            var exprReplaced = ExpressionNodeReplacer.ReplaceNode(expr, expression =>
            {
                if (expression.NodeType == ExpressionType.Constant &&
                    ((ConstantExpression) expression).Value is float val &&
                    val == 2)
                    return Expression.Constant(8.0f);
                return null;
            });
            Assert.IsTrue(CompareExpressionsExtensions.AreEqual(exprReplaced, exprReplacedValid));
        }

        [Test]
        public void ReplaceTest2()
        {
            Expression<Func<float, float>> expr = x => (int)Math.Truncate(x * 2 + x * 3);
            Expression<Func<float, float>> exprReplacedValid = x => (int)Math.Truncate(x * 2 + x * 9);
            var exprReplaced = ExpressionNodeReplacer.ReplaceNode(expr, expression =>
            {
                if (expression != null &&
                    expression.NodeType == ExpressionType.Constant &&
                    ((ConstantExpression) expression).Value is float val &&
                    val == 3)
                    return Expression.Constant(9.0f);
                return null;
            });
            Assert.IsTrue(CompareExpressionsExtensions.AreEqual(exprReplaced, exprReplacedValid));
        }
    }
}