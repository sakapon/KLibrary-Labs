using System;
using System.ComponentModel;

namespace KLibrary.Labs.Reactive.Models
{
    public static class ObservableProperty
    {
        public static IObservableProperty<TSource> Create<TSource>()
        {
            return new ObservableProperty<TSource>();
        }

        public static IObservableProperty<TSource> Create<TSource>(TSource defaultValue)
        {
            return new ObservableProperty<TSource>(defaultValue);
        }

        public static IObservableGetProperty<TSource> CreateGet<TSource>(Func<TSource> getValue)
        {
            return new ObservableGetProperty<TSource>(getValue);
        }

        public static IObservableGetProperty<TSource> CreateGet<TSource>(Func<TSource> getValue, TSource defaultValue)
        {
            return new ObservableGetProperty<TSource>(getValue, defaultValue);
        }

        public static IObservableGetProperty<TSource> CreateGetDirect<TSource>(Func<TSource> getValue)
        {
            return new DirectGetProperty<TSource>(getValue);
        }

        public static IObservableEvent<TSource> CreateEvent<TSource>()
        {
            return new ObservableEvent<TSource>();
        }

        public static IObservable<TSource> ToObservableMask<TSource>(this IObservable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new ObservableMask<TSource>(source);
        }

        public static IObservableGetProperty<TSource> ToGetPropertyMask<TSource>(this IObservableProperty<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new ObservableGetPropertyMask<TSource>(source);
        }

        public static IObservableGetProperty<TSource> ToGetProperty<TSource>(this IObservable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new FollowingGetProperty<TSource>(source);
        }

        public static IObservableGetProperty<TSource> ToGetProperty<TSource>(this IObservable<TSource> source, TSource defaultValue)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new FollowingGetProperty<TSource>(source, defaultValue);
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

            return source.Subscribe(new ActionObserver<TSource>(o => property.OnNext()));
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
