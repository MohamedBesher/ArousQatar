using System;

namespace Saned.ArousQatar.Data.Persistence.Infrastructure
{
    public class Disposable : IDisposable
    {
        private bool _isDisposed;

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
                DisposeCore();

            _isDisposed = true;
        }


        //override this to dispose custom objects
        protected virtual void DisposeCore()
        {

        }
    }
}
