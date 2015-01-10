using System;

namespace KLibrary.Labs.Reactive.Models
{
    public interface IObservableProperty
    {
        IDisposable Subscribe(Action onValueChanged);
    }
}
