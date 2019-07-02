//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 11:05:59
//    说明：引擎接口--启动任务接口
//    版权所有：个人
//***********************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace OARMS.Core.Infrastructure
{
    /// <summary>
    /// 启动任务接口
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        void Execute();
        /// <summary>
        /// 获取执行任务的顺序
        /// </summary>
        int Order { get; }
    }
}
