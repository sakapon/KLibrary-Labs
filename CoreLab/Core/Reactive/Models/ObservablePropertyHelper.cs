﻿using System;
using System.ComponentModel;

namespace KLibrary.Labs.Reactive.Models
{
    /// <summary>
    /// Provides a set of static methods for the observable property model.
    /// </summary>
    public static class ObservableProperty
    {
        /// <summary>
        /// Creates an instance of <see cref="IObservableProperty&lt;TSource&gt;"/> with the default value of <typeparamref name="TSource"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <returns>An <see cref="IObservableProperty&lt;TSource&gt;"/> object.</returns>
        public static IObservableProperty<TSource> Create<TSource>()
        {
            return new ObservableProperty<TSource>();
        }

        /// <summary>
        /// Creates an instance of <see cref="IObservableProperty&lt;TSource&gt;"/> with the specified default value.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An <see cref="IObservableProperty&lt;TSource&gt;"/> object.</returns>
        public static IObservableProperty<TSource> Create<TSource>(TSource defaultValue)
        {
            return new ObservableProperty<TSource>(defaultValue);
        }

        /// <summary>
        /// Creates an instance of <see cref="IObservableGetProperty&lt;TSource&gt;"/> with the return value of <paramref name="getValue"/> as the default value.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <param name="getValue">The function to get a new property value.</param>
        /// <returns>An <see cref="IObservableGetProperty&lt;TSource&gt;"/> object.</returns>
        public static IObservableGetProperty<TSource> CreateGet<TSource>(Func<TSource> getValue)
        {
            return new ObservableGetProperty<TSource>(getValue);
        }

        /// <summary>
        /// Creates an instance of <see cref="IObservableGetProperty&lt;TSource&gt;"/> with the specified default value.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <param name="getValue">The function to get a new property value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An <see cref="IObservableGetProperty&lt;TSource&gt;"/> object.</returns>
        public static IObservableGetProperty<TSource> CreateGet<TSource>(Func<TSource> getValue, TSource defaultValue)
        {
            return new ObservableGetProperty<TSource>(getValue, defaultValue);
        }

        /// <summary>
        /// Creates an instance of <see cref="IObservableGetProperty&lt;TSource&gt;"/>.
        /// The property value is not cached, so is evaluated for every access.
        /// </summary>
        /// <typeparam name="TSource">The type of the property.</typeparam>
        /// <param name="getValue">The function to get a new property value.</param>
        /// <returns>An <see cref="IObservableGetProperty&lt;TSource&gt;"/> object.</returns>
        public static IObservableGetProperty<TSource> CreateGetDirect<TSource>(Func<TSource> getValue)
        {
            return new DirectGetProperty<TSource>(getValue);
        }

        /// <summary>
        /// Creates an instance of <see cref="IObservableEvent&lt;TSource&gt;"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of objects to be provided.</typeparam>
        /// <returns>An <see cref="IObservableEvent&lt;TSource&gt;"/> object.</returns>
        public static IObservableEvent<TSource> CreateEvent<TSource>()
        {
            return new ObservableEvent<TSource>();
        }

        /// <summary>
        /// Creates a wrapper of an <see cref="IObservable&lt;TSource&gt;"/> object to restrict members to be accessed.
        /// </summary>
        /// <typeparam name="TSource">The type of objects to be provided.</typeparam>
        /// <param name="source">An <see cref="IObservable&lt;TSource&gt;"/> object.</param>
        /// <returns>A wrapped <see cref="IObservable&lt;TSource&gt;"/> object.</returns>
        public static IObservable<TSource> ToObservableMask<TSource>(this IObservable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new ObservableMask<TSource>(source);
        }

        /// <summary>
        /// Creates a wrapper of an <see cref="IObservableProperty&lt;TSource&gt;"/> object to restrict members to be accessed.
        /// </summary>
        /// <typeparam name="TSource">The type of objects to be provided.</typeparam>
        /// <param name="source">An <see cref="IObservableProperty&lt;TSource&gt;"/> object.</param>
        /// <returns>A wrapped <see cref="IObservableGetProperty&lt;TSource&gt;"/> object.</returns>
        public static IObservableGetProperty<TSource> ToGetPropertyMask<TSource>(this IObservableProperty<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new ObservableGetPropertyMask<TSource>(source);
        }

        /// <summary>
        /// Creates an <see cref="IObservableGetProperty&lt;TSource&gt;"/> from an <see cref="IObservable&lt;TSource&gt;"/>, with the default value of <typeparamref name="TSource"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of objects to be provided.</typeparam>
        /// <param name="source">An <see cref="IObservable&lt;TSource&gt;"/> object.</param>
        /// <returns>An <see cref="IObservableGetProperty&lt;TSource&gt;"/> object.</returns>
        public static IObservableGetProperty<TSource> ToGetProperty<TSource>(this IObservable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new FollowingGetProperty<TSource>(source);
        }

        /// <summary>
        /// Creates an <see cref="IObservableGetProperty&lt;TSource&gt;"/> from an <see cref="IObservable&lt;TSource&gt;"/>, with the specified default value.
        /// </summary>
        /// <typeparam name="TSource">The type of objects to be provided.</typeparam>
        /// <param name="source">An <see cref="IObservable&lt;TSource&gt;"/> object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An <see cref="IObservableGetProperty&lt;TSource&gt;"/> object.</returns>
        public static IObservableGetProperty<TSource> ToGetProperty<TSource>(this IObservable<TSource> source, TSource defaultValue)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new FollowingGetProperty<TSource>(source, defaultValue);
        }

        /// <summary>
        /// Creates an <see cref="IObservableGetProperty&lt;TResult&gt;"/> from another <see cref="IObservableProperty&lt;TSource&gt;"/>.
        /// The default value is determined by the value of <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of objects to be provided.</typeparam>
        /// <typeparam name="TResult">The type of mapped objects.</typeparam>
        /// <param name="source">An <see cref="IObservableProperty&lt;TSource&gt;"/> object.</param>
        /// <param name="mapping">A transform function to apply to each element.</param>
        /// <returns>An <see cref="IObservableGetProperty&lt;TResult&gt;"/> object.</returns>
        public static IObservableGetProperty<TResult> ToGetProperty<TSource, TResult>(this IObservableProperty<TSource> source, Func<TSource, TResult> mapping)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (mapping == null) throw new ArgumentNullException("mapping");

            return new FollowingGetProperty<TResult>(source.Map(mapping), mapping(source.Value));
        }

        /// <summary>
        /// Creates an <see cref="IObservableGetProperty&lt;TResult&gt;"/> from another <see cref="IObservableGetProperty&lt;TSource&gt;"/>.
        /// The default value is determined by the value of <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of objects to be provided.</typeparam>
        /// <typeparam name="TResult">The type of mapped objects.</typeparam>
        /// <param name="source">An <see cref="IObservableGetProperty&lt;TSource&gt;"/> object.</param>
        /// <param name="mapping">A transform function to apply to each element.</param>
        /// <returns>An <see cref="IObservableGetProperty&lt;TResult&gt;"/> object.</returns>
        public static IObservableGetProperty<TResult> ToGetProperty<TSource, TResult>(this IObservableGetProperty<TSource> source, Func<TSource, TResult> mapping)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (mapping == null) throw new ArgumentNullException("mapping");

            return new FollowingGetProperty<TResult>(source.Map(mapping), mapping(source.Value));
        }

        public static IObservable<TSource> Do<TSource, TProperty>(this IObservable<TSource> source, IObservableGetProperty<TProperty> property)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (property == null) throw new ArgumentNullException("property");

            return source.ChainNext<TSource, TSource>(obs => o =>
            {
                property.OnNext();
                obs.OnNext(o);
            });
        }

        public static IDisposable Subscribe<TSource, TProperty>(this IObservable<TSource> source, IObservableGetProperty<TProperty> property)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (property == null) throw new ArgumentNullException("property");

            return source.Subscribe(Observer2.Create<TSource>(o => property.OnNext()));
        }
    }

    class ObservableMask<T> : IObservable<T>
    {
        IObservable<T> _root;

        public ObservableMask(IObservable<T> root)
        {
            _root = root;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _root.Subscribe(observer);
        }
    }

    class ObservableGetPropertyMask<T> : IObservableGetProperty<T>
    {
        IObservableProperty<T> _root;

        public T Value
        {
            get { return _root.Value; }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _root.PropertyChanged += (o, e) => value(this, e); }
            remove { }
            //add { _root.PropertyChanged += value; }
            //remove { _root.PropertyChanged -= value; }
        }

        public ObservableGetPropertyMask(IObservableProperty<T> root)
        {
            _root = root;
        }

        public void OnNext()
        {
            throw new NotSupportedException();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _root.Subscribe(observer);
        }
    }
}
