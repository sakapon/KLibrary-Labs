using System;

namespace KLibrary.Labs
{
    public static class Disposable
    {
        public static IDisposable FromAction(Action action)
        {
            return new DisposableAction(action, false);
        }

        public static IDisposable FromAction(Action action, bool onFinalizing)
        {
            return new DisposableAction(action, onFinalizing);
        }
    }

    class DisposableAction : IDisposable
    {
        Action _action;
        bool _onFinalizing;
        bool _isDisposed;

        public DisposableAction(Action action, bool onFinalizing)
        {
            if (action == null) throw new ArgumentNullException("action");
            _action = action;
            _onFinalizing = onFinalizing;
        }

        ~DisposableAction()
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
                _action();
            }
            catch (Exception) { }
        }
    }
}
