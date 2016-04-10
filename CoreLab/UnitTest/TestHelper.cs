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
    }
}
