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
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An <see cref="ISettableProperty&lt;T&gt;"/> object.</returns>
        public static ISettableProperty<T> CreateSettable<T>(T defaultValue)
        {
            return CreateSettable(defaultValue, false);
        }

        public static ISettableProperty<T> CreateSettable<T>(T defaultValue, bool notifiesUnchanged)
        {
            return new SettableProperty<T>(defaultValue, notifiesUnchanged);
        }

        /// <summary>
        /// Creates an instance of IObservable-based get-only property.
        /// The value is initialized with the return value of <paramref name="getValue"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="getValue">The function to get a new property value.</param>
        /// <returns>An <see cref="IGetOnlyProperty&lt;T&gt;"/> object.</returns>
        public static IGetOnlyProperty<T> CreateGetOnly<T>(Func<T> getValue)
        {
            return CreateGetOnly(getValue, false);
        }

        public static IGetOnlyProperty<T> CreateGetOnly<T>(Func<T> getValue, bool notifiesUnchanged)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            return new CachingGetOnlyProperty<T>(getValue, getValue(), notifiesUnchanged);
        }

        /// <summary>
        /// Creates an instance of IObservable-based get-only property with the specified default value.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="getValue">The function to get a new property value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An <see cref="IGetOnlyProperty&lt;T&gt;"/> object.</returns>
        public static IGetOnlyProperty<T> CreateGetOnlyWithDefault<T>(Func<T> getValue, T defaultValue)
        {
            return CreateGetOnlyWithDefault(getValue, defaultValue, false);
        }

        public static IGetOnlyProperty<T> CreateGetOnlyWithDefault<T>(Func<T> getValue, T defaultValue, bool notifiesUnchanged)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            return new CachingGetOnlyProperty<T>(getValue, defaultValue, notifiesUnchanged);
        }

        /// <summary>
        /// Creates an instance of IObservable-based get-only property from the predecessor sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <param name="source">The source sequence of elements.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An <see cref="IGetOnlyProperty&lt;TSource&gt;"/> object.</returns>
        public static IGetOnlyProperty<TSource> ToGetOnly<TSource>(this IObservable<TSource> source, TSource defaultValue)
        {
            return ToGetOnly(source, defaultValue, false);
        }

        public static IGetOnlyProperty<TSource> ToGetOnly<TSource>(this IObservable<TSource> source, TSource defaultValue, bool notifiesUnchanged)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new FollowingGetOnlyProperty<TSource>(source, defaultValue, notifiesUnchanged);
        }

        /// <summary>
        /// Creates an instance of IObservable-based get-only property from the predecessor settable property.
        /// </summary>
        /// <typeparam name="TSource">The type of the source property.</typeparam>
        /// <typeparam name="TResult">The type of the property.</typeparam>
        /// <param name="source">The source property.</param>
        /// <param name="selector">The transform function.</param>
        /// <returns>An <see cref="IGetOnlyProperty&lt;TResult&gt;"/> object.</returns>
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

        /// <summary>
        /// Creates an instance of IObservable-based get-only property from the predecessor get-only property.
        /// </summary>
        /// <typeparam name="TSource">The type of the source property.</typeparam>
        /// <typeparam name="TResult">The type of the property.</typeparam>
        /// <param name="source">The source property.</param>
        /// <param name="selector">The transform function.</param>
        /// <returns>An <see cref="IGetOnlyProperty&lt;TResult&gt;"/> object.</returns>
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

        /// <summary>
        /// Creates a mask to capsulate the source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source sequence of elements.</param>
        /// <returns>An <see cref="IObservable&lt;TSource&gt;"/> object.</returns>
        [Obsolete("Use AsObservable method.")]
        public static IObservable<TSource> ToObservableMask<TSource>(this IObservable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new ObservableMask<TSource>(source);
        }

        /// <summary>
        /// Creates a mask to capsulate the source settable property.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <param name="source">The source property.</param>
        /// <returns>An <see cref="IGetOnlyProperty&lt;TSource&gt;"/> object.</returns>
        public static IGetOnlyProperty<TSource> ToGetOnlyMask<TSource>(this ISettableProperty<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new GetOnlyPropertyMask<TSource>(source);
        }

        /// <summary>
        /// Notifies the property to update the value, for each element.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="source">The source sequence of elements.</param>
        /// <param name="property">The get-only property.</param>
        /// <returns>A new source sequence.</returns>
        public static IObservable<TSource> Do<TSource, TProperty>(this IObservable<TSource> source, IGetOnlyProperty<TProperty> property)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (property == null) throw new ArgumentNullException("property");

            return source.Do(o => property.OnNext());
        }

        /// <summary>
        /// Notifies the property to update the value, for each element.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="source">The source sequence of elements.</param>
        /// <param name="property">The get-only property.</param>
        /// <returns>An <see cref="IDisposable"/> object used to unsubscribe from the source sequence.</returns>
        public static IDisposable Subscribe<TSource, TProperty>(this IObservable<TSource> source, IGetOnlyProperty<TProperty> property)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (property == null) throw new ArgumentNullException("property");

            return source.Subscribe(o => property.OnNext());
        }
    }
}
