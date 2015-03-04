using System;

namespace KLibrary.Labs.Pipeline
{
    public static class Usual
    {
        public static void DoAction(Action action)
        {
            action();
        }

        public static T DoFunc<T>(Func<T> func)
        {
            return func();
        }

        public static T Identity<T>(T obj)
        {
            return obj;
        }

        public static bool IsNull<T>(T obj) where T : class
        {
            return obj == null;
        }

        public static bool IsNotNull<T>(T obj) where T : class
        {
            return obj != null;
        }

        public static bool IsNullOrWhiteSpace(string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static bool IsNotNullNorWhiteSpace(string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        public static bool AreEqual<T>(T obj1, T obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool AreNotEqual<T>(T obj1, T obj2)
        {
            return !object.Equals(obj1, obj2);
        }
    }
}
