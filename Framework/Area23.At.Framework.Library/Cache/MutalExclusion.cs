using Area23.At.Framework.Library.Cqr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Cache
{
    internal static class MutalExclusion
    {

        const string CACHE_MUTEX_NAME = "CacheMutalExclusion";
        private static readonly object _outerLock = new object(), _lock = new object();

        private static Mutex _cacheMutalExclusion = null;

        internal static Mutex CacheMutalExclusion { get => _cacheMutalExclusion; }


        static MutalExclusion()
        {
            _cacheMutalExclusion = null;
        }

        internal static Mutex CreateCacheMutalExlusion(bool returnExistingMutexIfNotReleased = false)
        {
            if (_cacheMutalExclusion != null && _cacheMutalExclusion.SafeWaitHandle != null && !_cacheMutalExclusion.SafeWaitHandle.IsClosed && !_cacheMutalExclusion.SafeWaitHandle.IsInvalid)
            {
                if (returnExistingMutexIfNotReleased)
                    return _cacheMutalExclusion;
            }

            Thread.Sleep(16);
            _cacheMutalExclusion = new Mutex(true, CACHE_MUTEX_NAME);

            return _cacheMutalExclusion;
        }

        /// <summary>
        /// Release Mutax exclusion, that not 2 chat programs could be started at same machine
        /// </summary>
        internal static void ReleaseCloseDisposeMutex()
        {
            Exception ex = null;
            Microsoft.Win32.SafeHandles.SafeWaitHandle safeWaitHandle = null;
            IntPtr safeMutextWin32Handle = IntPtr.Zero;

            lock (_outerLock)
            {
                if (_cacheMutalExclusion != null)
                {
                    lock (_lock)
                    {
                        safeWaitHandle = _cacheMutalExclusion.GetSafeWaitHandle();
                        safeMutextWin32Handle = safeWaitHandle.DangerousGetHandle();
                        if (safeWaitHandle != null && !safeWaitHandle.IsClosed)
                        {
                            try
                            {
                                _cacheMutalExclusion.ReleaseMutex();
                                //    safeWaitHandle.DangerousRelease();
                            }
                            catch (Exception exRelease)
                            {
                                ex = new CqrException("Releasing Mutex failed", exRelease);
                                CqrException.SetLastException(ex);
                            }
                            try
                            {
                                _cacheMutalExclusion.Close();
                                //    safeWaitHandle.Close();
                            }
                            catch (Exception exClose)
                            {
                                ex = new CqrException("Closing Mutex failed", exClose);
                                CqrException.SetLastException(ex);
                            }
                        }

                        try
                        {
                            _cacheMutalExclusion.Dispose();
                            //    safeWaitHandle.Dispose();
                        }
                        catch (Exception exDispose)
                        {
                            ex = new CqrException("Disposing Mutex failed", exDispose);
                            CqrException.SetLastException(ex);
                        }
                    }
                }

                try
                {
                    _cacheMutalExclusion = null;
                }
                catch (Exception exNull)
                {
                    ex = new CqrException("Setting Mutex to null failed", exNull);
                    CqrException.SetLastException(ex);
                }
                finally
                {
                    if (ex != null)
                    {
                        CqrException.SetLastException(new CqrException("Disposing mutex and safeWaitHandle throwed exception.", ex));
                    }
                }
            }

            return;
        }

    }

}
