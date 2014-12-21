using System;

namespace KLibrary.Labs.Pipeline
{
    public static class Case
    {
        public static CaseForAction<T> ForAction<T>(Func<T, bool> condition, Action<T> action)
        {
            if (condition == null) throw new ArgumentNullException("condition");

            return new CaseForAction<T>
            {
                Condition = condition,
                Action = action,
            };
        }

        public static CaseForAction<T> ForAction<T>(T condition, Action<T> action)
        {
            return ForAction(o => object.Equals(o, condition), action);
        }

        public static CaseForFunc<T, TResult> ForFunc<T, TResult>(Func<T, bool> condition, Func<T, TResult> func)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            if (func == null) throw new ArgumentNullException("func");

            return new CaseForFunc<T, TResult>
            {
                Condition = condition,
                Func = func,
            };
        }

        public static CaseForFunc<T, TResult> ForFunc<T, TResult>(T condition, Func<T, TResult> func)
        {
            return ForFunc(o => object.Equals(o, condition), func);
        }
    }

    public class CaseForAction<T>
    {
        public Func<T, bool> Condition { get; internal set; }
        public Action<T> Action { get; internal set; }
    }

    public class CaseForFunc<T, TResult>
    {
        public Func<T, bool> Condition { get; internal set; }
        public Func<T, TResult> Func { get; internal set; }
    }
}
