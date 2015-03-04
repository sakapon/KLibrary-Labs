using System;
using System.Collections.Generic;

namespace KLibrary.Labs
{
    public static class Comparison
    {
        public static IComparer<T> CreateComparer<T>(Func<T, T, int> compare)
        {
            return new DelegateComparer<T>(compare);
        }

        public static IEqualityComparer<T> CreateEqualityComparer<T>(Func<T, T, bool> equals)
        {
            return new DelegateEqualityComparer<T>(equals);
        }
    }

    class DelegateComparer<T> : Comparer<T>
    {
        Func<T, T, int> _compare;

        public DelegateComparer(Func<T, T, int> compare)
        {
            if (compare == null) throw new ArgumentNullException("compare");
            _compare = compare;
        }

        public override int Compare(T x, T y)
        {
            return _compare(x, y);
        }
    }

    class DelegateEqualityComparer<T> : EqualityComparer<T>
    {
        Func<T, T, bool> _equals;

        public DelegateEqualityComparer(Func<T, T, bool> equals)
        {
            if (equals == null) throw new ArgumentNullException("equals");
            _equals = equals;
        }

        public override bool Equals(T x, T y)
        {
            return _equals(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return obj != null ? obj.GetHashCode() : 0;
        }
    }
}
