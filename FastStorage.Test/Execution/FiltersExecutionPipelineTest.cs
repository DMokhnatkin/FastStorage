using System.Linq;
using FastStorage.Collection;
using FastStorage.Execution;
using FastStorage.Expressions.Filters;
using FastStorage.Expressions.OperationTargets;
using NUnit.Framework;

namespace FastStorage.Test.Execution
{
    [TestFixture]
    public class FiltersExecutionPipelineTest
    {
        [Test]
        public void Test1()
        {
            var fastCollection = new FastCollection<int>();
            var filter1 = new ComparisonFilter(OperationTargetBuilder.BuildForRoot(fastCollection), ComparisonFilterType.Less, 3);
            var filter2 = new ComparisonFilter(OperationTargetBuilder.BuildForRoot(fastCollection), ComparisonFilterType.Greater, 6);
            
            var filter3 = new CompoundFilter(
                CompoundFilter.CompoundFilterType.And,
                filter1, 
                filter2);
            
            var executionGroups = FiltersExecutionPipeline.Build(filter3).GetExecutionGroups();
            Assert.AreEqual(0, executionGroups.ElementAt(0).Level);
            CollectionAssert.AreEquivalent(new []{ filter1, filter2 }, executionGroups.ElementAt(0).Filters);
            Assert.AreEqual(1, executionGroups.ElementAt(1).Level);
            CollectionAssert.AreEquivalent(new []{ filter3 }, executionGroups.ElementAt(1).Filters);
        }
        
        [Test]
        public void Test2()
        {
            var fastCollection = new FastCollection<int>();
            var filter1 = new ComparisonFilter(OperationTargetBuilder.BuildForRoot(fastCollection), ComparisonFilterType.Less, 4);
            var filter2 = new ComparisonFilter(OperationTargetBuilder.BuildForRoot(fastCollection), ComparisonFilterType.Greater, 6);
            var filter3 = new CompoundFilter(
                CompoundFilter.CompoundFilterType.And,
                filter1, filter2);
            var filter4 = new ComparisonFilter(OperationTargetBuilder.BuildForRoot(fastCollection), ComparisonFilterType.Less, 2);
            var filter5 = new ComparisonFilter(OperationTargetBuilder.BuildForRoot(fastCollection), ComparisonFilterType.Less, 7);
            var filter7 = new CompoundFilter(
                CompoundFilter.CompoundFilterType.And,
                filter4, filter5);
            var filter6 = new ComparisonFilter(OperationTargetBuilder.BuildForRoot(fastCollection), ComparisonFilterType.Less, 8);
            var filter8 = new CompoundFilter(
                CompoundFilter.CompoundFilterType.And,
                filter3, filter7, filter6);
            
            var executionGroups = FiltersExecutionPipeline.Build(filter8).GetExecutionGroups();
            Assert.AreEqual(0, executionGroups.ElementAt(0).Level);
            CollectionAssert.AreEquivalent(new []{ filter1, filter2, filter4, filter5, filter6 }, executionGroups.ElementAt(0).Filters);
            Assert.AreEqual(1, executionGroups.ElementAt(1).Level);
            Assert.AreEqual(new []{ filter3, filter7 }, executionGroups.ElementAt(1).Filters);
            Assert.AreEqual(2, executionGroups.ElementAt(2).Level);
            Assert.AreEqual(new []{ filter8 }, executionGroups.ElementAt(2).Filters);
        }
    }
}