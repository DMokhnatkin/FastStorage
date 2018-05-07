using FastStorage.Builders;
using FastStorage.Indices;
using FastStorage.Linq;
using NUnit.Framework;

namespace FastStorage.Test.Linq
{
    [TestFixture]
    public class LinqTest1
    {
        class P
        {
            public int A { get; set; }

            public float B { get; set; }

            public P2 C { get; set; }
        }

        class P2
        {
            public int D { get; set; }

            public int E { get; set; }
        }

        private readonly P[] _data = new[]
        {
            new P {A = 1, B = 5, C = new P2{ D = 1}},
            new P {A = 2, B = 4, C = new P2{ D = 2}},
            new P {A = 3, B = 3, C = new P2{ D = 3}},
            new P {A = 4, B = 2, C = new P2{ D = 4}},
            new P {A = 5, B = 1, C = new P2{ D = 5}}
        };

        [Test]
        public void Test1()
        {
            var fastCollection = _data
                .AddIndex(x => x.A, new RedBlackTreeIndexFactory(), new HashTableIndexFactory())
                .AddIndex(x => x.B, new RedBlackTreeIndexFactory())
                .AddIndex(x => x.A + x.B, new RedBlackTreeIndexFactory())
                .Build();
            
            var t = fastCollection
                .Where(x => x.A > 4)
                .Select(x => x.C)
                .Where(x => x.D > 2)
                .Select(x => x.E + x.D)
                .ToArray();
        }
    }
}