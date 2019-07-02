//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-02 13:24:10
//    说明：系统基础类
//    版权所有：个人
//***********************************************************
using System;
using System.Threading;

namespace OARMS.Core.ComponentModel
{

    /// <summary>
    /// 对资源访问方法实现提供锁
    /// </summary>
    /// <remarks>
    /// 作为系统基础类
    /// </remarks>
    public class WriteLockDisposable : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// 对 <see cref="WriteLockDisposable"/> 类初始化一个实例.
        /// </summary>
        /// <param name="rwLock">读写锁</param>
        public WriteLockDisposable(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _rwLock.EnterWriteLock();
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitWriteLock();
        }
    }
}
