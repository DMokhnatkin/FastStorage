using System;
using System.Linq;
using FastStorage.Builders;
using FastStorage.Indices;
using FastStorage.Linq;
using JetBrains.Annotations;
using NUnit.Framework;

namespace FastStorage.Test.Linq
{
    [TestFixture]
    public class RestrictionOperatorsTest
    {
        private class TestData1
        {
            public int Id { get; set; }

            public int UnitsInStock { get; set; }

            public decimal UnitPrice { get; set; }

            public string Region { get; set; }
        }
        
        [NotNull]
        private TestData1[] GenerateTestArray()
        {
            return new[]
            {
                new TestData1
                {
                    Id = 0,
                    UnitsInStock = 0,
                    UnitPrice = 4m,
                    Region = "WA"
                },
                new TestData1
                {
                    Id = 1,
                    UnitsInStock = 2,
                    UnitPrice = 1m,
                    Region = "LA"
                },
                new TestData1
                {
                    Id = 2,
                    UnitsInStock = 0,
                    UnitPrice = 5m,
                    Region = "WA"
                },
                new TestData1
                {
                    Id = 3,
                    UnitsInStock = 1,
                    UnitPrice = 10m,
                    Region = "aba"
                },
                new TestData1
                {
                    Id = 4,
                    UnitsInStock = 10,
                    UnitPrice = 8m,
                    Region = "acvd"
                },
            };
        }

        [Test]
        public void WhereTest1()
        {
            var fastCollection =
                new[] {5, 4, 1, 3, 9, 8, 6, 7, 2, 0}
                    .AddIndex(x => x, new RedBlackTreeIndexFactory())
                    .Build();
            var res = fastCollection.Where(x => x < 5).ToArray();
            CollectionAssert.AreEqual(new []{ 4, 1, 3, 2, 0}, res);
        }

        [Test]
        public void WhereTest2()
        {
            var fastCollection = GenerateTestArray()
                .AddIndex(x => x.UnitsInStock, new HashTableIndexFactory())
                .Build();
            var res = fastCollection.Where(x => x.UnitsInStock == 0).ToArray();
            CollectionAssert.AreEqual(new[] { 0, 2 }, res.Select(x => x.Id).ToArray());
        }

        [Test]
        public void WhereTest3()
        {
            var fastCollection = GenerateTestArray()
                .AddIndex(x => x.UnitsInStock, new RedBlackTreeIndexFactory())
                .AddIndex(x => x.UnitPrice, new RedBlackTreeIndexFactory())
                .Build();
            
            
            var res = fastCollection
                .Where(x => x.UnitsInStock > 0 && x.UnitPrice > 3.0m)
                .ToArray();
            
            CollectionAssert.AreEqual(new[] { 3, 4 }, res.Select(x => x.Id).ToArray());
        }

        [Test]
        public void WhereTest4()
        {
            var fastCollection = GenerateTestArray()
                .AddIndex(x => x.Region, new HashTableIndexFactory())
                .Build();
            var res = fastCollection
                .Where(x => x.Region == "WA")
                .ToArray();
            CollectionAssert.AreEqual(new[] { 0, 2 }, res.Select(x => x.Id).ToArray());
        }

        [Test]
        public void WhereTest5()
        {
            throw new NotImplementedException();
//            var fastCollection =
//                new[] {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"}
//                    .ToFastCollection();
//            var res = fastCollection.Where((digit, index) => digit.Length < index);
//            CollectionAssert.AreEqual(new[] { "five", "six", "seven", "eight", "nine" }, res);
        }
    }
}
