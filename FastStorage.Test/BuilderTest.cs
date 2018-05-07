using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FastStorage.Test
{
    [TestFixture]
    public class BuilderTest
    {
        private class SampleData
        {
            public int Val1 { get; set; }
            public string Val2 { get; set; }
            public DateTime Val3 { get; set; }
        }

        [Test]
        public void ApiTest()
        {
            //var builder =
            //    new FastCollectionBuilder<SampleData>()
            //    .UseStorageAlgorithm(data => data.Val1, new HashTableDefinition<int>())
            //    .UseStorageAlgorithm(data => data.Val2, new SuffixTreeDefinition());

            //var collection = builder.Build();
        }

        [Test]
        public void IndexBuilderApiTest()
        {
            //var sourceCollection = new []
            //{
            //    new SampleData(),
            //    new SampleData()
            //};

            //uint[] ind = {0};
            //var builder = new IndexBuilder();
            //var val1Index = builder
            //    .CreateIndex<int, uint>()
            //    .UseStorageAlgorithm(new KeyIndexDefenition<int, uint>())
            //    .ForCollection(sourceCollection, key => key.Val1, data => ind[0]++)
            //    .Build();

            //var res = val1Index.Get(13);

            //ind[0] = 0;
            //var builder2 = new IndexBuilder();
            //var val2Index = builder2
            //    .CreateIndex<string, uint>()
            //    .UseStorageAlgorithm(new SubstringEntranceIndexDefenition<uint>())
            //    .ForCollection(sourceCollection, key => key.Val2, data => ind[0]++)
            //    .Build();

            //var t = val2Index.FindEntrance("test");
        }
    }
}
