//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 11:35:59
//    说明：引擎接口--依赖注册接口
//    版权所有：个人
//***********************************************************
using Autofac;
using OARMS.Core.Configuration;

namespace OARMS.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 依赖注册接口
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// 注册接口和服务
        /// </summary>
        /// <param name="builder">容器</param>
        /// <param name="typeFinder">类型查找器</param>
        /// <param name="config">系统配置</param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, OarmsConfig config);
        /// <summary>
        /// 获取依赖注入的实现顺序
        /// </summary>
        int Order { get; }
    }
}
