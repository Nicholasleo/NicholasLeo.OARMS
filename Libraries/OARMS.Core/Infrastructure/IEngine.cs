//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 10:20:55
//    说明：引擎接口--引擎
//    版权所有：个人
//***********************************************************
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace OARMS.Core.Infrastructure
{
    /// <summary>
    /// 引擎接口
    /// 实现该接口的类可以访问系统大多数功能
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// 初始化引擎
        /// </summary>
        /// <param name="services"></param>
        void Initialize(IServiceCollection services);
        /// <summary>
        /// 添加或配置服务信息
        /// </summary>
        /// <param name="services">服务描述集合</param>
        /// <param name="configuration">应用配置</param>
        /// <returns></returns>
        IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration);
        /// <summary>
        /// 配置HTTP请求管道
        /// </summary>
        /// <param name="application"></param>
        void ConfigureRequestPipeline(IApplicationBuilder application);
        /// <summary>
        /// 解析依赖注入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;
        /// <summary>
        /// 解析依赖注入
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(Type type);
        /// <summary>
        /// 解析依赖注入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();
        /// <summary>
        /// 解析未注册的依赖
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object ResolveUnregistered(Type type);
    }
}
