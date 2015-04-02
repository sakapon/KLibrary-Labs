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
        Dictionary<PropertyChangedEventHandler, PropertyChangedEventHandler> _propertyChangedMasks = new Dictionary<PropertyChangedEventHandler, PropertyChangedEventHandler>();

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
            add
            {
                if (_propertyChangedMasks.ContainsKey(value)) return;

                _propertyChangedMasks[value] = (o, e) => value(this, e);
                _source.PropertyChanged += _propertyChangedMasks[value];
            }
            remove
            {
                if (!_propertyChangedMasks.ContainsKey(value)) return;

                _source.PropertyChanged -= _propertyChangedMasks[value];
                _propertyChangedMasks.Remove(value);
            }
            // データ バインディングでは sender が一致しなければならないため、以下のコードでは不可です。
            //add { _source.PropertyChanged += value; }
            //remove { _source.PropertyChanged -= value; }
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
