using System.Linq;
using FastStorage.Collection;
using FastStorage.Expressions.Filters;
using FastStorage.Expressions.OperationTargets;
using FastStorage.Linq;
using NUnit.Framework;

namespace FastStorage.Test.Expressions.Filters
{
    [TestFixture]
    public class FiltersParserTest
    {
        class TestClassA
        {
            public int F1 { get; set; }
        }
        
        [Test]
        public void Test1()
        {
            var parser = new FiltersParser();
            var fastCollection = new FastCollection<TestClassA>();
            var parsed = parser.ParsePredicate((TestClassA x) => x.F1 > 3, OperationTargetBuilder.BuildForRoot(fastCollection));
            Assert.IsAssignableFrom<ComparisonFilter>(parsed);
            Assert.AreEqual(ComparisonFilterType.Greater, ((ComparisonFilter)parsed).ComparisonFilterType);
            Assert.AreEqual(3, ((ComparisonFilter)parsed).Value);
            Assert.AreSame(fastCollection, ((ComparisonFilter)parsed).OperationTarget.RootFastCollection);
        }
        
        [Test]
        public void Test2()
        {
            // Embedded in predicate fast collection. Not in current version
            var parser = new FiltersParser();
            var fastCollection = new FastCollection<TestClassA>();
            //var parsed = parser.ParsePredicate((TestClassA x) => fastCollection.Where(y => y.F1 > 4).Select(y => y.F1).Contains(x.F1), OperationTargetBuilder.BuildForRoot(fastCollection));
//            Assert.IsAssignableFrom<ComparisonFilter>(parsed);
//            Assert.AreEqual(ComparisonFilterType.Greater, ((ComparisonFilter)parsed).ComparisonFilterType);
//            Assert.AreEqual(3, ((ComparisonFilter)parsed).Value);
//            Assert.AreSame(fastCollection, ((ComparisonFilter)parsed).OperationTarget.RootFastCollection);
        }
        
        [Test]
        public void AndCompoundTest()
        {
            var parser = new FiltersParser();
            var fastCollection = new FastCollection<TestClassA>();
            var parsed = parser.ParsePredicate((TestClassA x) => (x.F1 > 3) && (x.F1 <= 10), OperationTargetBuilder.BuildForRoot(fastCollection));
            Assert.IsAssignableFrom<CompoundFilter>(parsed);
            Assert.AreEqual(CompoundFilter.CompoundFilterType.And, ((CompoundFilter)parsed).OperationType);
            
            var filter1 = (ComparisonFilter) ((CompoundFilter) parsed).Operands.ElementAt(0);
            Assert.AreEqual(ComparisonFilterType.Greater, filter1.ComparisonFilterType);
            Assert.AreEqual(3, filter1.Value);
            
            var filter2 = (ComparisonFilter) ((CompoundFilter) parsed).Operands.ElementAt(1);
            Assert.AreEqual(ComparisonFilterType.LessOrEqual, filter2.ComparisonFilterType);
            Assert.AreEqual(10, filter2.Value);
        }
    }
}