using System;

namespace KLibrary.Labs.Pipeline
{
    public static class ObjectHelper
    {
        public static T DoAction<T>(this T obj, Action<T> action)
        {
            if (action != null) action(obj);
            return obj;
        }

        #region Action - Branch

        public static T DoAction<T>(this T obj, Func<T, bool> condition, Action<T> trueAction, Action<T> falseAction)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            var a = condition(obj) ? trueAction : falseAction;
            if (a != null) a(obj);
            return obj;
        }

        public static T DoAction<T>(this T obj, T condition, Action<T> trueAction, Action<T> falseAction)
        {
            return DoAction(obj, o => object.Equals(o, condition), trueAction, falseAction);
        }

        public static T DoAction<T>(this T obj, Action<T> defaultAction, params CaseForAction<T>[] cases)
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

        public static T DoAction<T>(this T obj, Func<T, bool> condition, Action<T> action)
        {
            return DoAction(obj, condition, action, null);
        }

        public static T DoAction<T>(this T obj, T condition, Action<T> action)
        {
            return DoAction(obj, condition, action, null);
        }

        public static T DoAction<T>(this T obj, params CaseForAction<T>[] cases)
        {
            return DoAction(obj, null, cases);
        }

        #endregion

        public static TResult DoFunc<T, TResult>(this T obj, Func<T, TResult> func)
        {
            if (func == null) throw new ArgumentNullException("func");

            return func(obj);
        }

        #region Func - Branch

        public static TResult DoFunc<T, TResult>(this T obj, Func<T, bool> condition, Func<T, TResult> trueFunc, Func<T, TResult> falseFunc)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            if (trueFunc == null) throw new ArgumentNullException("trueFunc");
            if (falseFunc == null) throw new ArgumentNullException("falseFunc");

            return (condition(obj) ? trueFunc : falseFunc)(obj);
        }

        public static TResult DoFunc<T, TResult>(this T obj, T condition, Func<T, TResult> trueFunc, Func<T, TResult> falseFunc)
        {
            return DoFunc(obj, o => object.Equals(o, condition), trueFunc, falseFunc);
        }

        public static TResult DoFunc<T, TResult>(this T obj, Func<T, TResult> defaultFunc, params CaseForFunc<T, TResult>[] cases)
        {
            if (defaultFunc == null) throw new ArgumentNullException("defaultFunc");
            if (cases == null) throw new ArgumentNullException("cases");

            foreach (var @case in cases)
            {
                if (@case.Condition(obj))
                {
                    return @case.Func(obj);
                }
            }
            return defaultFunc(obj);
        }

        #endregion

        #region Func - Partial Branch

        public static T Fallback<T>(this T obj, Func<T, bool> condition, Func<T, T> fallback)
        {
            if (fallback == null) throw new ArgumentNullException("fallback");

            return DoFunc(obj, condition, fallback, Usual.Identity);
        }

        public static T Fallback<T>(this T obj, T condition, Func<T, T> fallback)
        {
            if (fallback == null) throw new ArgumentNullException("fallback");

            return DoFunc(obj, condition, fallback, Usual.Identity);
        }

        public static T Fallback<T>(this T obj, params CaseForFunc<T, T>[] cases)
        {
            return DoFunc(obj, Usual.Identity, cases);
        }

        #endregion
    }
}
