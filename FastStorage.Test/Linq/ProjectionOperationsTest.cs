using System;
using FastStorage.Builders;
using FastStorage.Indices;
using FastStorage.Linq;
using NUnit.Framework;

namespace FastStorage.Test.Linq
{
    [TestFixture]
    public class ProjectionOperatorsTest
    {
        private class TestData
        {
            public int Id { get; set; }

            public string ProductName { get; set; }

            public decimal UnitPrice { get; set; }

            public string Category { get; set; }
        }

        private TestData[] _testCollection = {
            new TestData
            {
                Id = 0,
                ProductName = "test0",
                Category = "c1",
                UnitPrice = 3.0m
            },
            new TestData
            {
                Id = 1,
                ProductName = "test1",
                Category = "c1",
                UnitPrice = 4.0m
            },
            new TestData
            {
                Id = 2,
                ProductName = "test2",
                Category = "c1",
                UnitPrice = 2.0m
            },
            new TestData
            {
                Id = 3,
                ProductName = "test3",
                Category = "c2",
                UnitPrice = 11.0m
            },
            new TestData
            {
                Id = 4,
                ProductName = "test4",
                Category = "c2",
                UnitPrice = 10.0m
            }
        };

        [Test]
        public void Select1()
        {
            var fastCollection =
                new[] {5, 4, 1, 3, 9, 8, 6, 7, 2, 0}
                    .ToFastCollection();

            var res = fastCollection.Select(x => x + 1).ToArray();
            CollectionAssert.AreEqual(new[] { 6, 5, 2, 4, 10, 9, 7, 8, 3, 1 }, res);
        }

        [Test]
        public void Select2()
        {
            var fastCollection =
                _testCollection.ToFastCollection();

            var res = fastCollection.Select(x => x.ProductName);
            CollectionAssert.AreEqual(new [] { "test0", "test1", "test2", "test3", "test4" }, res.AsEnumerable());
        }

        [Test]
        public void SelectTransformation()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] strings = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

            var fastCollection =
                numbers.ToFastCollection();

            var res = fastCollection.Select(x => strings[x]);
            CollectionAssert.AreEqual(
                new[] { "five", "four", "one", "three", "nine", "nine", "eight", "six", "seven", "two", "zero" }, 
                res.AsEnumerable());
        }

        [Test]
        public void SelectAnonymousTypes1()
        {
            string[] words = { "aPPLE", "BlUeBeRrY", "cHeRry" };

            var fastCollection = words.ToFastCollection();
            
            var res = fastCollection.Select(x => new { Upper = x.ToUpper(), Lower = x.ToLower() }).ToArray();
            CollectionAssert.AreEqual(new[]
            {
                new { Upper = "APPLE", Lower = "apple" },
                new { Upper = "BLUEBERRY", Lower = "blueberry" },
                new { Upper = "CHERRY", Lower = "cherry" },
            }, res);
        }

        [Test]
        public void SelectAnonymousTypes2()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var fastCollection = numbers.ToFastCollection();

            var res = fastCollection.Select(x => new { Digit = strings[x], Even = x % 2 == 0}).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new { Digit = "five", Even = false },
                new { Digit = "four", Even = true },
                new { Digit = "one", Even = false },
                new { Digit = "three", Even = false },
                new { Digit = "nine", Even = false },
                new { Digit = "eight", Even = true },
                new { Digit = "six", Even = true },
                new { Digit = "seven", Even = false },
                new { Digit = "two", Even = true },
                new { Digit = "zero", Even = true },

            }, res);
        }

        [Test]
        public void SelectAnonymousTypes3()
        {
            var fastCollection = _testCollection.ToFastCollection();

            var res = fastCollection.Select(x => new { x.ProductName, x.Category, Price = x.UnitPrice }).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new { ProductName = "test0", Category = "c1", Price = 3.0m },
                new { ProductName = "test1", Category = "c1", Price = 4.0m },
                new { ProductName = "test2", Category = "c1", Price = 2.0m },
                new { ProductName = "test3", Category = "c2", Price = 11.0m },
                new { ProductName = "test4", Category = "c2", Price = 10.0m },
            }, res);
        }

        [Test]
        public void SelectIndexed()
        {
            throw new NotImplementedException();
            
//            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
//
//            var fastCollection = numbers.ToFastCollection();
//
//            var res = fastCollection.Select((num, index) => new { Num = num, InPlace = (num == index) }).ToArray();
//
//            CollectionAssert.AreEqual(new[]
//            {
//                new { Num = 5, InPlace = false},
//                new { Num = 4, InPlace = false},
//                new { Num = 1, InPlace = false},
//                new { Num = 3, InPlace = true},
//                new { Num = 9, InPlace = false},
//                new { Num = 8, InPlace = false},
//                new { Num = 6, InPlace = true},
//                new { Num = 7, InPlace = true},
//                new { Num = 2, InPlace = false},
//                new { Num = 0, InPlace = false}
//            }, res);
        }

        [Test]
        public void SelectFiltered()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var fastCollection = numbers
                .AddIndex(x => x, new RedBlackTreeIndexFactory())
                .Build();

            var res = fastCollection.Where(x => x < 5).Select(x => digits[x]).ToArray();

            CollectionAssert.AreEqual(new[] { "four", "one", "three", "two", "zero" }, res);
        }

        [Test]
        public void SelectManyCompoundFrom1()
        {
            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            throw new NotImplementedException();

            //var pairs =
            //    from a in numbersA
            //    from b in numbersB
            //    where a < b
            //    select new { a, b };
            throw new NotImplementedException();
        }

        [Test]
        public void SelectManyCompoundFrom2()
        {
            throw new NotImplementedException();

            //var orders =
            //    from c in customers
            //    from o in c.Orders
            //    where o.Total < 500.00M
            //    select new { c.CustomerID, o.OrderID, o.Total };
            throw new NotImplementedException();
        }

        [Test]
        public void SelectManyCompoundFrom3()
        {
            throw new NotImplementedException();

            //var orders =
            //    from c in customers
            //    from o in c.Orders
            //    where o.OrderDate >= new DateTime(1998, 1, 1)
            //    select new { c.CustomerID, o.OrderID, o.OrderDate };
            throw new NotImplementedException();
        }

        [Test]
        public void SelectManyFromAssignment()
        {
            throw new NotImplementedException();

            //var orders =
            //    from c in customers
            //    from o in c.Orders
            //    where o.Total >= 2000.0M
            //    select new { c.CustomerID, o.OrderID, o.Total };
            throw new NotImplementedException();
        }

        [Test]
        public void SelectManyMultipleFrom()
        {
            throw new NotImplementedException();

            //var orders =
            //    from c in customers
            //    where c.Region == "WA"
            //    from o in c.Orders
            //    where o.OrderDate >= cutoffDate
            //    select new { c.CustomerID, o.OrderID };
            throw new NotImplementedException();
        }

        [Test]
        public void SelectManyIndexed()
        {
            throw new NotImplementedException();

            //var customerOrders =
            //    customers.SelectMany(
            //        (cust, custIndex) =>
            //            cust.Orders.Select(o => "Customer #" + (custIndex + 1) +
            //                                    " has an order with OrderID " + o.OrderID));
            throw new NotImplementedException();
        }
    }
}
