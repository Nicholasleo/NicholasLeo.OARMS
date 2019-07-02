//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 10:45:08
//    说明：引擎接口--类型查找器
//    版权所有：个人
//***********************************************************
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OARMS.Core.Infrastructure
{
    /// <summary>
    /// 提供OARMS引擎中的所有服务类型信息
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 查找类型
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="onlyConcreteClasses">是否指定具体类</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);
        /// <summary>
        /// 查找类型
        /// </summary>
        /// <param name="assignTypeFrom">分配的类型</param>
        /// <param name="onlyConcreteClasses">是否指定具体类</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);
        /// <summary>
        /// 查找类型
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="assemblies">程序集</param>
        /// <param name="onlyConcreteClasses">是否指定具体类</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
        /// <summary>
        /// 查找类型
        /// </summary>
        /// <param name="assignTypeFrom">分配的类型</param>
        /// <param name="assemblies">程序集</param>
        /// <param name="onlyConcreteClasses">是否指定具体类</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
        /// <summary>
        /// 获取当前实现的程序集
        /// </summary>
        /// <returns>程序集的列表</returns>
        IList<Assembly> GetAssemblies();
    }
}
