//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.InteropServices;
//using FastStorage.Builders;
//using FastStorage.Indices;
//using FastStorage.Linq;
//using NUnit.Framework;
//
//namespace FastStorage.Test.Linq
//{
//    [TestFixture]
//    public class KmuTests
//    {
//        private class TestClass
//        {
//            public string F1 { get; set; }
//
//            public string F2 { get; set; }
//        }
//        
//        [Test]
//        public void Test()
//        {
//            // Исходные данные
//            var data = Enumerable.Range(1, 10000000).Select(x => new TestClass
//            {
//                F1 = Guid.NewGuid().ToString(), 
//                F2 = Guid.NewGuid().ToString()
//            }).ToArray();
//            var target = data[9999999];
//            
//            /* Original */
//            var originalExecution = new Stopwatch();
//            originalExecution.Start();
//            
//            // Запрос по ключу 1
//            var res1 = data
//                .Where(x => x == target)
//                .Select(x => x.F1 + x.F2)
//                .ToArray();
//            // Запрос по ключу 2
//            var res2 = data
//                .Where(x => x?.F1 + x?.F2 == "test")
//                .ToArray();
//            
//            originalExecution.Stop();
//            Console.WriteLine($"originalExecution: {originalExecution.Elapsed.TotalSeconds}");
//            /**/
//            
//            /* Default optimization */
//            // Оптимизация поиска строки по ключу 1
//            Dictionary<TestClass, string> stringByTestClass = data.ToDictionary(
//                keySelector => keySelector,
//                valSelector => valSelector?.F1 + valSelector?.F2);
//            
//            // Оптимизация поиска по ключу 2
//            Dictionary<string, TestClass> testClassByString = data.ToDictionary(
//                keySelector => keySelector?.F1 + keySelector?.F2, 
//                valSelector => valSelector);
//            
//            var defOptimizationExec = new Stopwatch();
//            defOptimizationExec.Start();
//            
//            // Поиск по ключу 1
//            stringByTestClass.TryGetValue(target, out var res3);
//            // Поиск по ключу 2
//            testClassByString.TryGetValue(target.F1 + target.F2, out var res4);
//            
//            defOptimizationExec.Stop();
//            Console.WriteLine($"defOptimizationExec: {defOptimizationExec.Elapsed.TotalSeconds}");
//            /**/
//            
//            /* Fast collection */
//            // Создание индексов
//            var fastCollection = data
//                .AddIndex(x => x.F1, new HashTableIndexFactory())
//                .Build();
//
//            var fastCollectionExec = new Stopwatch();
//            fastCollectionExec.Start();
//            
//            // Поиск по ключу 1
////            var res5 = fastCollection
////                .Where(x => x == null)
////                .Select(x => x.F1 + x.F2)
////                .ToArray();
//            
//            var res5 = fastCollection
//                .Where(x => x.F1 == "test")
//                .Select(x => x.F1 + x.F2)
//                .ToArray();
//            // Поиск по ключу 2
//            var res6 = fastCollection
//                .Where(x => x.F1 == "test")
//                .Select(x => x.F1 + x.F2)
//                .ToArray();
//           
//            fastCollectionExec.Stop();
//            Console.WriteLine($"fastCollectionExec: {fastCollectionExec.Elapsed.TotalSeconds}");
//            /**/
//        }
//    }
//}