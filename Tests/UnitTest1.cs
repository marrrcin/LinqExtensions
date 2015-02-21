using System;
using System.Diagnostics;
using System.Linq;
using LinqExtensions.CustomExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanFlatten()
        {
            var array = new dynamic[]
            {
                new[] {"lol", "xyz", "abc"},
                new dynamic[] {"lol2", new dynamic[] {"nested1", new[] {"nested2","nested2-other"}}},
                "trolo"
            };

            var result = array.Flatten<string>().ToList();

            Assert.AreEqual(8,result.Count);
            Assert.AreEqual("lol",result[0]);
            Assert.AreEqual("xyz", result[1]);
            Assert.AreEqual("lol2",result[3]);
            Assert.AreEqual("nested1",result[4]);
            Assert.AreEqual("nested2-other",result[6]);
            Assert.AreEqual("trolo",result[7]);

        }

        [TestMethod]
        public void CanFlattenNonRecursive()
        {
            var array = new dynamic[]
            {
                new[] {"lol", "xyz", "abc"},
                new dynamic[] {"lol2", new dynamic[] {"nested1", new[] {"nested2","nested2-other"}}},
                "trolo"
            };

            var result = array.Flatten<string>(false).ToList();

            Assert.AreEqual(8, result.Count);
            Assert.AreEqual("lol", result[1]);
            Assert.AreEqual("xyz", result[2]);
            Assert.AreEqual("lol2", result[4]);
            Assert.AreEqual("nested1", result[5]);
            Assert.AreEqual("nested2-other", result[7]);
            Assert.AreEqual("trolo", result[0]);

        }

        [TestMethod]
        public void CanMerge2Arrays()
        {
            var array1 = Enumerable.Range(0, 10).Where(i => (i & 1) == 1).ToList();
            var array2 = Enumerable.Range(0, 10).Where(i => (i & 1) == 0).ToList();

            var result = LinqCollections.Merge((a, b) => a - b, array1, array2).ToList();

            Assert.AreEqual(array1.Count + array2.Count, result.Count);
            for (int i = 1; i < result.Count; i++)
            {
                Assert.IsTrue(result[i-1]<=result[i]);
            }
        }

        [TestMethod]
        public void CanMerge3Arrays()
        {
            var array1 = Enumerable.Range(0, 15).Where(i=>i%3==0).ToList();
            var array2 = Enumerable.Range(0, 15).Where(i=>i%3==1).ToList();
            var array3 = Enumerable.Range(0, 15).Where(i=>i%3==2).ToList();

            var result = LinqCollections.Merge((a, b) => a - b, array1, array2,array3).ToList();

            Assert.AreEqual(array1.Count + array2.Count + array3.Count, result.Count);
            for (int i = 1; i < result.Count; i++)
            {
                Assert.IsTrue(result[i - 1] <= result[i]);
            }
        }

        [TestMethod]
        public void CanMergeSimple()
        {
            var array1 = Enumerable.Range(0, 1000).Select(i=>2).ToList();
            var array2 = Enumerable.Range(0, 1000).Select(i=>1).ToList();

            var clock = new Stopwatch();
            clock.Reset();
            clock.Start();
            var result = LinqCollections.Merge((a, b) => a - b, array1, array2).ToList();
            clock.Stop();

            Assert.AreEqual(array1.Count + array2.Count, result.Count);
            for (int i = 1; i < result.Count; i++)
            {
                Assert.IsTrue(result[i - 1] <= result[i]);
            }
        }
    }
}
