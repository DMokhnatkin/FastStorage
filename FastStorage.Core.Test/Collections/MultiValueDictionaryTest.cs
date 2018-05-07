using System.Linq;
using FastStorage.Core.Collections;
using Xunit;

namespace FastStorage.Core.Test.Collections
{
    public class MultiValueDictionaryTest
    {
        [Fact]
        public void AddTest()
        {
            var t = new MultiValueDictionary<int, int>();
            t.Add(1, 3);
            t.Add(1, 4);
            t.Add(1, 5);
            t.Add(2, 3);
            t.Add(2, 4);

            Assert.Equal(new []{ 3, 4, 5 }, t[1].OrderBy(x => x));
            Assert.Equal(new []{ 3, 4 }, t[2].OrderBy(x => x));
        }

        [Fact]
        public void RemoveTest()
        {
            var t = new MultiValueDictionary<int, int>();
            t.Add(1, 3);
            t.Add(1, 4);
            t.Add(1, 5);
            t.Add(2, 3);
            t.Add(2, 4);

            t.Remove(1);
            Assert.False(t.ContainsKey(1));
            t.Remove(2);
            Assert.False(t.ContainsKey(1));
            Assert.True(!t.Any());
        }

        [Fact]
        public void RemoveTest2()
        {
            var t = new MultiValueDictionary<int, int>();
            t.Add(1, 3);
            t.Add(1, 4);
            t.Add(1, 5);
            t.Add(2, 3);
            t.Add(2, 4);

            t.Remove(1, 3);
            Assert.Equal(new[] { 4, 5 }, t[1].OrderBy(x => x));
            t.Remove(1, 5);
            Assert.Equal(new[] { 4 }, t[1].OrderBy(x => x));
            t.Remove(2, 4);
            Assert.Equal(new[] { 3 }, t[2].OrderBy(x => x));
            t.Remove(1, 4);
            Assert.False(t.ContainsKey(1));
        }

        [Fact]
        public void FullCountTest()
        {
            var t = new MultiValueDictionary<int, int>();
            Assert.Equal(0, t.FullCount);
            t.Add(1, 3);
            t.Add(1, 3);
            t.Add(2, 3);
            Assert.Equal(3, t.FullCount);
            t.Add(1, 4);
            Assert.Equal(4, t.FullCount);
            t.Remove(1, 3);
            Assert.Equal(3, t.FullCount);
        }
    }
}
