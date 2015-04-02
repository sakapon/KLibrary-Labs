using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace KLibrary.Labs.ObservableModel
{
    class ObservableMask<T> : IObservable<T>
    {
        IObservable<T> _source;

        public ObservableMask(IObservable<T> source)
        {
            _source = source;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _source.Subscribe(observer);
        }
    }

    class GetOnlyPropertyMask<T> : IGetOnlyProperty<T>
    {
        ISettableProperty<T> _source;

        public bool HasObservers
        {
            get { return _source.HasObservers; }
        }

        public T Value
        {
            get { return _source.Value; }
        }

        public bool NotifiesUnchanged
        {
            get { return _source.NotifiesUnchanged; }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _source.PropertyChanged += (o, e) => value(this, e); }
            remove { }
        }

        public GetOnlyPropertyMask(ISettableProperty<T> source)
        {
            _source = source;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _source.Subscribe(observer);
        }

        public void OnNext()
        {
            throw new NotSupportedException();
        }
    }
}
