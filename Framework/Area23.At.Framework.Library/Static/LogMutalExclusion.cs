using Area23.At.Framework.Library.Cqr;
using System;
using System.Threading;

namespace Area23.At.Framework.Library.Static
{

    /// <summary>
    /// static <see cref="Mutex"/> for mutal exclusion, you can only use it once for one mutal exclusion caae, cause it's static 
    /// get <see cref="Mutex"/> by calling <see cref="CreateMutalExlusion(string, bool)"/> 
    /// release <see cref="Mutex"/> by calling <see cref="ReleaseCloseDisposeMutex"/>
    /// </summary>
    internal static class LogMutalExclusion 
    {
        private static readonly object _outerLock = new object(), _lock = new object();
     
        private static Mutex _theMutex = null;

        /// <summary>
        /// Gets the Mutal Exclusion
        /// </summary>
        internal static Mutex TheMutex { get => _theMutex; }

        /// <summary>
        /// static ctor
        /// </summary>
        static LogMutalExclusion()
        {
            _theMutex = null;
        }

        /// <summary>
        /// Gets existing mutex or creates a new <see cref="Mutex"/> 
        /// </summary>
        /// <param name="mutexUniqueName">unique string identifier for the mutal exlusion</param>
        /// <param name="useExistingMutex">if true, existing and valid <see cref="Mutex"/> will be returned, 
        /// otherwise a new <see cref="Mutex"/> will be created; default <see cref="false"/></param>
        /// <returns><see cref="Mutex"/></returns>
        internal static Mutex CreateMutalExlusion(string mutexUniqueName = "MutalExclusion", bool useExistingMutex = false)
        {
            if (useExistingMutex && _theMutex != null && _theMutex.SafeWaitHandle != null && 
                !_theMutex.SafeWaitHandle.IsClosed && !_theMutex.SafeWaitHandle.IsInvalid) 
                    return _theMutex;

            // Thread.Sleep(16);
            _theMutex = new Mutex(true, mutexUniqueName);

            return _theMutex;
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
                if (_theMutex != null)
                {
                    lock (_lock)
                    {
                        safeWaitHandle = _theMutex.GetSafeWaitHandle();
                        safeMutextWin32Handle = safeWaitHandle.DangerousGetHandle();
                        if (safeWaitHandle != null && !safeWaitHandle.IsClosed)
                        {
                            try
                            {
                                _theMutex.ReleaseMutex();
                                //    safeWaitHandle.DangerousRelease();
                            }
                            catch (Exception exRelease)
                            {
                                ex = new CqrException("Releasing Mutex failed", exRelease);
                                CqrException.SetLastException(ex);
                            }
                            try
                            {
                                _theMutex.Close();
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
                            _theMutex.Dispose();
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
                    _theMutex = null;
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
