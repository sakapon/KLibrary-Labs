using System;

namespace KLibrary.Labs.Pipeline
{
    public static class ObjectHelper
    {
        public static T Do<T>(this T obj, Action<T> action)
        {
            if (action != null) action(obj);
            return obj;
        }

        #region Action - Branch

        public static T Do<T>(this T obj, Func<T, bool> condition, Action<T> trueAction, Action<T> falseAction)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            var a = condition(obj) ? trueAction : falseAction;
            if (a != null) a(obj);
            return obj;
        }

        public static T Do<T>(this T obj, T condition, Action<T> trueAction, Action<T> falseAction)
        {
            return Do(obj, o => object.Equals(o, condition), trueAction, falseAction);
        }

        public static T Do<T>(this T obj, Action<T> defaultAction, params CaseForAction<T>[] cases)
        {
            if (cases == null) throw new ArgumentNullException("cases");

            foreach (var @case in cases)
            {
                if (@case.Condition(obj))
                {
                    if (@case.Action != null) @case.Action(obj);
                    return obj;
                }
            }
            if (defaultAction != null) defaultAction(obj);
            return obj;
        }

        #endregion

        #region Action - Partial Branch

        public static T Do<T>(this T obj, Func<T, bool> condition, Action<T> action)
        {
            return Do(obj, condition, action, null);
        }

        public static T Do<T>(this T obj, T condition, Action<T> action)
        {
            return Do(obj, condition, action, null);
        }

        public static T Do<T>(this T obj, params CaseForAction<T>[] cases)
        {
            return Do(obj, null, cases);
        }

        #endregion

        public static TResult Map<T, TResult>(this T obj, Func<T, TResult> mapping)
        {
            if (mapping == null) throw new ArgumentNullException("mapping");

            return mapping(obj);
        }

        #region Func - Branch

        public static TResult Map<T, TResult>(this T obj, Func<T, bool> condition, Func<T, TResult> trueMapping, Func<T, TResult> falseMapping)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            if (trueMapping == null) throw new ArgumentNullException("trueMapping");
            if (falseMapping == null) throw new ArgumentNullException("falseMapping");

            return (condition(obj) ? trueMapping : falseMapping)(obj);
        }

        public static TResult Map<T, TResult>(this T obj, T condition, Func<T, TResult> trueMapping, Func<T, TResult> falseMapping)
        {
            return Map(obj, o => object.Equals(o, condition), trueMapping, falseMapping);
        }

        public static TResult Map<T, TResult>(this T obj, Func<T, TResult> defaultMapping, params CaseForFunc<T, TResult>[] cases)
        {
            if (defaultMapping == null) throw new ArgumentNullException("defaultMapping");
            if (cases == null) throw new ArgumentNullException("cases");

            foreach (var @case in cases)
            {
                if (@case.Condition(obj))
                {
                    return @case.Func(obj);
                }
            }
            return defaultMapping(obj);
        }

        #endregion

        #region Func - Partial Branch

        public static T Fallback<T>(this T obj, Func<T, bool> condition, Func<T, T> fallback)
        {
            if (fallback == null) throw new ArgumentNullException("fallback");

            return Map(obj, condition, fallback, Usual.Identity);
        }

        public static T Fallback<T>(this T obj, T condition, Func<T, T> fallback)
        {
            if (fallback == null) throw new ArgumentNullException("fallback");

            return Map(obj, condition, fallback, Usual.Identity);
        }

        public static T Fallback<T>(this T obj, params CaseForFunc<T, T>[] cases)
        {
            return Map(obj, Usual.Identity, cases);
        }

        #endregion
    }
}
