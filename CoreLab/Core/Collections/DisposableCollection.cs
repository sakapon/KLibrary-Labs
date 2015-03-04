using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KLibrary.Labs.Collections
{
    public class DisposableCollection : Collection<IDisposable>, IDisposable
    {
        public DisposableCollection() { }

        public DisposableCollection(IList<IDisposable> list) : base(list) { }

        ~DisposableCollection()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var disposable in this)
                {
                    try
                    {
                        if (disposable != null) disposable.Dispose();
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
