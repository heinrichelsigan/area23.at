using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Win32Api
{

    /// <summary>
    /// static windows registry accessor with synchronous and asynchronous members
    /// </summary>
    public class RegistryAccessor
    {
        #region private fields

        private static RegistryHive _hive = RegistryHive.ClassesRoot;
        private static RegistryKey _hiveKey = null, _subKey = null;
        private static DateTime _lastRead = DateTime.MinValue;
        private static DateTime _lastWrite = DateTime.MinValue;

        private static Mutex _regOpMutex;
        private static int _threadCnt = 0;

        #endregion private fields

        #region public properties

        public static DateTime LastAccess { get => _lastRead; }

        public static DateTime LastModify { get => _lastWrite; }

        #endregion public properties


        /// <summary>
        /// static ctor
        /// </summary>
        static RegistryAccessor()
        {
            _hive = RegistryHive.ClassesRoot;
            _hiveKey = null;
            _subKey = null;
            _threadCnt = 0;
            TimeSpan oneDay = new TimeSpan(1, 0, 0, 0, 0);
        }


        /// <summary>
        /// GetRegistryEntry gets value for specified name from registry segment
        /// </summary>
        /// <param name="regHive">registry root hive</param>
        /// <param name="subKeyName">subKey in registry root hive segment</param>
        /// <param name="regName">unique name inside subKey registry segment</param>
        /// <returns>object value</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static object GetRegistryEntry(RegistryHive regHive, string subKeyName, string regName)
        {
            object o = null;
            _threadCnt = 0;

            if (string.IsNullOrEmpty(subKeyName) || string.IsNullOrEmpty(regName))
                throw new InternalErrorException($"subkey {subKeyName} or regName {regName} is null or empty");

            try
            {
                if ((_regOpMutex = new Mutex(true, Constants.MUTEX_REGOPS)) != null)
                {
                    while (_regOpMutex != null && !_regOpMutex.GetSafeWaitHandle().IsClosed &&
                        !_regOpMutex.GetSafeWaitHandle().IsInvalid && !_regOpMutex.WaitOne(64, false))
                    {
                        Thread.Sleep(64);
                        if ((++_threadCnt) > 7)
                            throw new AccessViolationException($"GetRegistryValueByName(RegistryHive regHive = {regHive.ToString()}, string subKeyName = {subKeyName}, string regName = {regName}) is locked by Mutex {Constants.MUTEX_REGOPS}.");
                    }
                }

                _lastRead = DateTime.Now;
                if (regHive == RegistryHive.LocalMachine)
                    _subKey = Registry.LocalMachine.OpenSubKey(subKeyName, false);
                else
                {
                    _hiveKey = RegistryKey.OpenBaseKey(regHive, RegistryView.Registry64);
                    _subKey = (_hiveKey != null) ? _hiveKey.OpenSubKey(subKeyName, RegistryRights.ReadKey) : null;
                }

                if (_subKey == null)
                    throw new InvalidOperationException($"Error reading registry from hive {regHive} subkey {subKeyName} name {regName}");

                o = _subKey.GetValue(regName);

            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("RegistryAccessor", "GetRegistryEntry(...)", ex);
                throw;
            }
            finally
            {
                if (_subKey != null)
                    _subKey.Close();
                if (_hiveKey != null)
                    _hiveKey.Close();
                if (_regOpMutex != null)
                {
                    _regOpMutex.ReleaseMutex();
                    if (!_regOpMutex.GetSafeWaitHandle().IsClosed)
                        _regOpMutex.Close();
                }
            }

            return o;
        }

        /// <summary>
        /// SetRegistryEntry sets a value for a name in registry 
        /// </summary>        
        /// <param name="value">value to set</param>
        /// <param name="regHive">registry root hive</param>
        /// <param name="subKeyName">subKey in root hive registry segment</param>
        /// <param name="regName">name in subKey registry segment</param>
        /// <returns>void means nothing</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetRegistryEntry(object value, RegistryHive regHive, string subKeyName, string regName)
        {
            _hive = regHive;
            _threadCnt = 0;

            if (string.IsNullOrEmpty(subKeyName) || string.IsNullOrEmpty(regName))
                throw new InternalErrorException($"subkey {subKeyName} or regName {regName} is null or empty");

            try
            {
                if ((_regOpMutex = new Mutex(true, Constants.MUTEX_REGOPS)) != null)
                {
                    while (_regOpMutex != null && !_regOpMutex.GetSafeWaitHandle().IsClosed &&
                        !_regOpMutex.GetSafeWaitHandle().IsInvalid && !_regOpMutex.WaitOne(32, false))
                    {
                        Thread.Sleep(32);
                        if ((++_threadCnt) > 7)
                            throw new AccessViolationException($"GetRegistryValueByName(RegistryHive regHive = {regHive.ToString()}, string subKeyName = {subKeyName}, string regName = {regName}) is locked by Mutex {Constants.MUTEX_REGOPS}.");
                    }
                }

                _lastWrite = DateTime.Now;
                _hiveKey = RegistryKey.OpenBaseKey(regHive, RegistryView.Registry32);

                _subKey = (_hiveKey != null) ?
                    _hiveKey.OpenSubKey(subKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl) :
                    null;

                if (_subKey != null)
                    _subKey.SetValue(regName, value);

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error setting value={value} in registry for hive {regHive} subkey {subKeyName} name {regName}", ex);
            }
            finally
            {
                if (_subKey != null)
                    _subKey.Close();
                if (_hiveKey != null)
                    _hiveKey.Close();
                if (_regOpMutex != null)
                {
                    _regOpMutex.ReleaseMutex();
                    if (!_regOpMutex.GetSafeWaitHandle().IsClosed)
                        _regOpMutex.Close();
                }
            }

            return;
        }

        internal static bool DeleteSubTree(RegistryHive regHive, string subTree)
        {
            bool retValue = false;
            _hive = regHive;
            _threadCnt = 0;

            if (string.IsNullOrEmpty(subTree))
                throw new InternalErrorException($"subkey {subTree} is null or empty");

            try
            {
                Area23Log.LogOriginMsg("RegistryAccessor", $"creating mutex {Constants.MUTEX_REGOPS} for delete operation in regHive {regHive} subTree {subTree}");
                if ((_regOpMutex = new Mutex(true, Constants.MUTEX_REGOPS)) != null)
                {
                    while (_regOpMutex != null && !_regOpMutex.GetSafeWaitHandle().IsClosed && !
                        _regOpMutex.GetSafeWaitHandle().IsInvalid && !_regOpMutex.WaitOne(64, false))
                    {
                        Thread.Sleep(64);
                        if ((++_threadCnt) > 7)
                            throw new AccessViolationException($"GetRegistryValueByName(RegistryHive regHive = {regHive.ToString()}, string subTree = {subTree} is locked by Mutex {Constants.MUTEX_REGOPS}.");
                    }
                }

                _lastWrite = DateTime.Now;

                _hiveKey = RegistryKey.OpenBaseKey(regHive, RegistryView.Registry64);
                _subKey = (_hiveKey != null) ? _hiveKey.OpenSubKey(subTree, false) : null;

                if (_subKey != null)
                {
                    _subKey.DeleteSubKeyTree(subTree);
                    retValue = true;
                }
            }
            catch (Exception ex)
            {
                retValue = false;
                throw new InvalidOperationException($"Error deleting subkey tree in registry for hive {regHive} subkey {subTree}", ex);
            }
            finally
            {
                if (_subKey != null)
                    _subKey.Close();
                if (_hiveKey != null)
                    _hiveKey.Close();
                if (_regOpMutex != null)
                {
                    _regOpMutex.ReleaseMutex();
                    if (!_regOpMutex.GetSafeWaitHandle().IsClosed)
                        _regOpMutex.Close();
                }
            }

            return retValue;

        }


        /// <summary>
        /// GetRegistryEntryAsync async gets an registry name value inside subkey asynchronous
        /// </summary>
        /// <param name="regHive">registry root hive</param>
        /// <param name="subKeyName">registry subkey name</param>
        /// <param name="regName">registry name ref</param>
        /// <returns><see cref="Task{object}"/></returns>
        public static async Task<object> GetRegistryEntryAsync(RegistryHive regHive, string subKeyName, string regName)
        {
            Task<object> readRegValueTask = (Task<object>)await Task<object>.Run<object>(() =>
            {
                object as0 = GetRegistryEntry(regHive, subKeyName, regName);
                return as0;
            });

            return readRegValueTask;
        }

        /// <summary>
        /// SetRegistryEntryAsync async sets object value in registry 
        /// </summary>
        /// <param name="value"><see cref="object"/> value to set</param>
        /// <param name="regHive">registry root hive</param>
        /// <param name="subKeyName">registry subkey name</param>
        /// <param name="regName">registry name ref</param>
        public static async void SetRegistryEntryAsync(object value, RegistryHive regHive, string subKeyName, string regName)
        {
            await Task.Run(new Action(() =>
            {
                SetRegistryEntry(value, regHive, subKeyName, regName);
            }));
        }

        /// <summary>
        /// AwaitTaskAsync
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <returns><see cref="Task{TResult}"/></returns>
        internal static async Task<TResult> AwaitTaskAsync<TResult>(Task<TResult> task)
        {
            var tcs = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            var t = await Task.WhenAny(task, tcs.Task).ConfigureAwait(false);
#pragma warning disable VSTHRD103 // Call async methods when in an async method
            return t.GetAwaiter().GetResult();
#pragma warning restore VSTHRD103 // Call async methods when in an async method
        }


    }

}
