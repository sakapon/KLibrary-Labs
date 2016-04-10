using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    public static class TestHelper
    {
        [DebuggerStepThrough]
        public static void AreCollectionCollectionEqual(ICollection<System.Collections.ICollection> expected, ICollection<System.Collections.ICollection> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            foreach (var _ in expected.Zip(actual, (e, a) => new { e, a }))
                CollectionAssert.AreEqual(_.e, _.a);
        }

        [DebuggerStepThrough]
        public static void AreDictionaryCollectionEqual<TKey, TValue>(ICollection<Dictionary<TKey, TValue>> expected, ICollection<Dictionary<TKey, TValue>> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            foreach (var _ in expected.Zip(actual, (e, a) => new { e, a }))
                AreDictionaryEqual(_.e, _.a);
        }

        [DebuggerStepThrough]
        public static void AreDictionaryEqual<TKey, TValue>(Dictionary<TKey, TValue> expected, Dictionary<TKey, TValue> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            foreach (var _ in expected.Zip(actual, (e, a) => new { e, a }))
                AreKeyValuePairEqual(_.e, _.a);
        }

        [DebuggerStepThrough]
        public static void AreKeyValuePairEqual<TKey, TValue>(KeyValuePair<TKey, TValue> expected, KeyValuePair<TKey, TValue> actual)
        {
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}
