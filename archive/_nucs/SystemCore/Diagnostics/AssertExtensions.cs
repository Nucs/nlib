using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nucs.Diagnostics
{
    public static class AssertExtensions {
        public static void AreEqual<T>(this IEnumerable<T> list, params object[] comparison) {
            var l = list.ToArray();
            if (l.Length != comparison.Length) 
                Assert.Fail("Length of the given IEnumerable does not equal to the number of comparable items given");
            for (var i = 0; i < l.Length; i++) {
                Assert.AreEqual(l[i], comparison[i]);
            }
        }
    }
}
