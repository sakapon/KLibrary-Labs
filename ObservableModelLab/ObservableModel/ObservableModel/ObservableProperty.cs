using System;
using System.Reactive.Linq;

namespace KLibrary.Labs.ObservableModel
{
    /// <summary>
    /// Provides a set of static methods for the observable model.
    /// </summary>
    public static class ObservableProperty
    {
        /// <summary>
        /// Creates an instance of IObservable-based settable property.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An <see cref="ISettableProperty&lt;TSource&gt;"/> object.</returns>
        public static ISettableProperty<TSource> CreateSettable<TSource>(TSource defaultValue)
        {
            return CreateSettable(defaultValue, false);
        }

        public static ISettableProperty<TSource> CreateSettable<TSource>(TSource defaultValue, bool notifiesUnchanged)
        {
            return new SettableProperty<TSource>(defaultValue, notifiesUnchanged);
        }

        /// <summary>
        /// Creates an instance of IObservable-based get-only property.
        /// The value is initialized with the return value of <paramref name="getValue"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <param name="getValue">The function to get a new property value.</param>
        /// <returns>An <see cref="IGetOnlyProperty&lt;TSource&gt;"/> object.</returns>
        public static IGetOnlyProperty<TSource> CreateGetOnly<TSource>(Func<TSource> getValue)
        {
            return CreateGetOnly(getValue, false);
        }

        public static IGetOnlyProperty<TSource> CreateGetOnly<TSource>(Func<TSource> getValue, bool notifiesUnchanged)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            return new CachingGetOnlyProperty<TSource>(getValue, getValue(), notifiesUnchanged);
        }

        /// <summary>
        /// Creates an instance of IObservable-based get-only property with the specified default value.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <param name="getValue">The function to get a new property value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An <see cref="IGetOnlyProperty&lt;TSource&gt;"/> object.</returns>
        public static IGetOnlyProperty<TSource> CreateGetOnlyWithDefault<TSource>(Func<TSource> getValue, TSource defaultValue)
        {
            return CreateGetOnlyWithDefault(getValue, defaultValue, false);
        }

        public static IGetOnlyProperty<TSource> CreateGetOnlyWithDefault<TSource>(Func<TSource> getValue, TSource defaultValue, bool notifiesUnchanged)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            return new CachingGetOnlyProperty<TSource>(getValue, defaultValue, notifiesUnchanged);
        }

        public static IGetOnlyProperty<TSource> ToGetOnly<TSource>(this IObservable<TSource> source, TSource defaultValue)
        {
            return ToGetOnly(source, defaultValue, false);
        }

        public static IGetOnlyProperty<TSource> ToGetOnly<TSource>(this IObservable<TSource> source, TSource defaultValue, bool notifiesUnchanged)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new FollowingGetOnlyProperty<TSource>(source, defaultValue, notifiesUnchanged);
        }

        public static IGetOnlyProperty<TResult> SelectToGetOnly<TSource, TResult>(this ISettableProperty<TSource> source, Func<TSource, TResult> selector)
        {
            return SelectToGetOnly(source, selector, false);
        }

        public static IGetOnlyProperty<TResult> SelectToGetOnly<TSource, TResult>(this ISettableProperty<TSource> source, Func<TSource, TResult> selector, bool notifiesUnchanged)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new FollowingGetOnlyProperty<TResult>(source.Select(selector), selector(source.Value), notifiesUnchanged);
        }

        public static IGetOnlyProperty<TResult> SelectToGetOnly<TSource, TResult>(this IGetOnlyProperty<TSource> source, Func<TSource, TResult> selector)
        {
            return SelectToGetOnly(source, selector, false);
        }

        public static IGetOnlyProperty<TResult> SelectToGetOnly<TSource, TResult>(this IGetOnlyProperty<TSource> source, Func<TSource, TResult> selector, bool notifiesUnchanged)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new FollowingGetOnlyProperty<TResult>(source.Select(selector), selector(source.Value), notifiesUnchanged);
        }

        public static IObservable<TSource> ToObservableMask<TSource>(this IObservable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new ObservableMask<TSource>(source);
        }

        public static IGetOnlyProperty<TSource> ToGetOnlyMask<TSource>(this ISettableProperty<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new GetOnlyPropertyMask<TSource>(source);
        }

        public static IObservable<TSource> Do<TSource, TProperty>(this IObservable<TSource> source, IGetOnlyProperty<TProperty> property)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (property == null) throw new ArgumentNullException("property");

            return source.Do(o => property.OnNext());
        }

        public static IDisposable Subscribe<TSource, TProperty>(this IObservable<TSource> source, IGetOnlyProperty<TProperty> property)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (property == null) throw new ArgumentNullException("property");

            return source.Subscribe(o => property.OnNext());
        }
    }
}
