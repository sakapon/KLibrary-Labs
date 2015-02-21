using System;
using System.Diagnostics;

namespace KLibrary.Labs
{
    public static class Disposable
    {
        public static IDisposable Create(Action dispose)
        {
            return new DelegateDisposable(dispose, false);
        }

        public static IDisposable Create(Action dispose, bool onFinalizing)
        {
            return new DelegateDisposable(dispose, onFinalizing);
        }
    }

    [DebuggerDisplay("IsDisposed: {_isDisposed}")]
    class DelegateDisposable : IDisposable
    {
        Action _dispose;
        bool _onFinalizing;
        bool _isDisposed;

        public DelegateDisposable(Action dispose, bool onFinalizing)
        {
            if (dispose == null) throw new ArgumentNullException("dispose");
            _dispose = dispose;
            _onFinalizing = onFinalizing;
        }

        ~DelegateDisposable()
        {
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_onFinalizing && !disposing) return;
            if (_isDisposed) return;
            _isDisposed = true;

            try
            {
                _dispose();
            }
            catch (Exception) { }
        }
    }
}
