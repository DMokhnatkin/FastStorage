using System.Linq;
using FastStorage.Expressions.EqualityComparer;
using NUnit.Framework;

namespace FastStorage.Test.Expressions.EqualityComparer
{
    [TestFixture]
    public class ExpressionEqualityComparerTest
    {
        class A
        {
            public int P1 { get; set; }

            public float P2 { get; set; }

            public A P3 { get; set; }
        }

        private IQueryable<A> _queryable = new A[0].AsQueryable();

        [Test]
        public void Test1()
        {
            var expr1 = _queryable.Where(x => x.P1 > 2).Expression.ToComparable();
            var expr2 = _queryable.Where(x => x.P1 > 2).Expression.ToComparable();
            var expr3 = _queryable.Where(x => x.P2 > 2).Expression.ToComparable();
            var expr4 = _queryable.Where(x => x.P1 > 3).Expression.ToComparable();

            var comparer = new ExpressionEqualityComparer();
            Assert.IsTrue(comparer.Equals(expr1, expr2));
            Assert.IsFalse(comparer.Equals(expr1, expr3));
            Assert.IsFalse(comparer.Equals(expr1, expr4));
        }

        [Test]
        public void Test2()
        {
            var expr1 = _queryable.Where(x => (x.P1 + 3) > 2).Expression.ToComparable();
            var expr2 = _queryable.Where(x => (x.P1 + 3) > 2).Expression.ToComparable();
            var expr3 = _queryable.Where(x => (x.P2 + 3) > 2).Expression.ToComparable();
            var expr4 = _queryable.Where(x => (x.P1 + 2) > 2).Expression.ToComparable();
            var expr5 = _queryable.Where(x => (x.P1 + 3) > 3).Expression.ToComparable();

            var comparer = new ExpressionEqualityComparer();
            Assert.IsTrue(comparer.Equals(expr1, expr2));
            Assert.IsFalse(comparer.Equals(expr1, expr3));
            Assert.IsFalse(comparer.Equals(expr1, expr4));
            Assert.IsFalse(comparer.Equals(expr1, expr5));
        }

        [Test]
        public void Test3()
        {
            var expr1 = _queryable.Where(x => (x.P1 + 3) > 2).Select(x => x.P3).Expression.ToComparable();
            var expr2 = _queryable.Where(x => (x.P1 + 3) > 2).Select(x => x.P3).Expression.ToComparable();
            var expr3 = _queryable.Where(x => (x.P1 + 3) > 2).Select(x => x.P1).Expression.ToComparable();
            var expr4 = _queryable.Where(x => (x.P1 + 3) > 2).Select(x => x.P3.P1).Expression.ToComparable();
            var expr5 = _queryable.Where(x => (x.P1 + 3) > 3).Select(x => x.P3).Expression.ToComparable();

            var comparer = new ExpressionEqualityComparer();
            Assert.IsTrue(comparer.Equals(expr1, expr2));
            Assert.IsFalse(comparer.Equals(expr1, expr3));
            Assert.IsFalse(comparer.Equals(expr1, expr4));
            Assert.IsFalse(comparer.Equals(expr1, expr5));
        }
    }
}
