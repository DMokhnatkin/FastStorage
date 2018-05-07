//using System.Collections.Generic;
//using FastStorage.Common;
//using FastStorage.Indecies;
//using JetBrains.Annotations;
//using NUnit.Framework;

//namespace FastStorage.Test.IndexTests
//{
//    [TestFixture]
//    public class KeyIndexTest
//    {
//        [NotNull]
//        private readonly IndexItem<string, string>[] _sampleData1 = {
//            new IndexItem<string, string>("testKey1", "testData1"),
//            new IndexItem<string, string>("testKey3", "testData3"),
//            new IndexItem<string, string>("testKey2", "testData2"), 
//        };

//        [NotNull]
//        private readonly IndexItem<int, string>[] _sampleData2 = {
//            new IndexItem<int, string>(1, "testData1"),
//            new IndexItem<int, string>(3, "testData3"),
//            new IndexItem<int, string>(2, "testData2"),
//        };

//        [Test]
//        public void FillTest1()
//        {
//            var keyIndex = new KeyIndexImpl<string, string>();

//            keyIndex.FillStorage(_sampleData1);

//            Assert.True(keyIndex.Get(_sampleData1[0].Key) == _sampleData1[0].Value);
//            Assert.True(keyIndex.Get(_sampleData1[1].Key) == _sampleData1[1].Value);
//            Assert.True(keyIndex.Get(_sampleData1[2].Key) == _sampleData1[2].Value);
//        }

//        [Test]
//        public void FillTest2()
//        {
//            var keyIndex = new KeyIndexImpl<int, string>();

//            keyIndex.FillStorage(_sampleData2);

//            Assert.True(keyIndex.Get(_sampleData2[0].Key) == _sampleData2[0].Value);
//            Assert.True(keyIndex.Get(_sampleData2[1].Key) == _sampleData2[1].Value);
//            Assert.True(keyIndex.Get(_sampleData2[2].Key) == _sampleData2[2].Value);
//        }

//        [Test]
//        public void GetSetTest1()
//        {
//            var keyIndex = new KeyIndexImpl<string, string>();

//            Assert.IsNull(keyIndex.Get(_sampleData1[0].Key));
//            Assert.IsNull(keyIndex.Get(_sampleData1[1].Key));
//            Assert.IsNull(keyIndex.Get(_sampleData1[2].Key));

//            keyIndex.Set(_sampleData1[0].Key, _sampleData1[0].Value);
//            keyIndex.Set(_sampleData1[1].Key, _sampleData1[1].Value);
//            keyIndex.Set(_sampleData1[2].Key, _sampleData1[2].Value);

//            Assert.True(keyIndex.Get(_sampleData1[0].Key) == _sampleData1[0].Value);
//            Assert.True(keyIndex.Get(_sampleData1[1].Key) == _sampleData1[1].Value);
//            Assert.True(keyIndex.Get(_sampleData1[2].Key) == _sampleData1[2].Value);
//        }

//        [Test]
//        public void GetSetTest2()
//        {
//            var keyIndex = new KeyIndexImpl<int, string>();

//            Assert.IsNull(keyIndex.Get(_sampleData2[0].Key));
//            Assert.IsNull(keyIndex.Get(_sampleData2[1].Key));
//            Assert.IsNull(keyIndex.Get(_sampleData2[2].Key));

//            keyIndex.Set(_sampleData2[0].Key, _sampleData2[0].Value);
//            keyIndex.Set(_sampleData2[1].Key, _sampleData2[1].Value);
//            keyIndex.Set(_sampleData2[2].Key, _sampleData2[2].Value);

//            Assert.True(keyIndex.Get(_sampleData2[0].Key) == _sampleData2[0].Value);
//            Assert.True(keyIndex.Get(_sampleData2[1].Key) == _sampleData2[1].Value);
//            Assert.True(keyIndex.Get(_sampleData2[2].Key) == _sampleData2[2].Value);
//        }
//    }
//}
