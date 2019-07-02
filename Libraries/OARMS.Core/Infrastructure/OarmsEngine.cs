//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 10:20:04
//    说明：
//    版权所有：个人
//***********************************************************
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OARMS.Core.Configuration;
using OARMS.Core.Infrastructure.DependencyManagement;
using OARMS.Core.Infrastructure.Mapper;
using OARMS.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OARMS.Core.Infrastructure
{
    /// <summary>
    /// OARMS引擎
    /// </summary>
    public class OarmsEngine : IEngine
    {
        /// <summary>
        /// 获取或设置服务控制器
        /// </summary>
        private IServiceProvider _serviceProvider { get; set; }

        /// <summary>
        /// 服务控制器
        /// </summary>
        public virtual IServiceProvider ServiceProvider => _serviceProvider;

        /// <summary>
        /// 获取服务控制器
        /// </summary>
        /// <returns></returns>
        protected IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var content = accessor.HttpContext;
            return content?.RequestServices ?? ServiceProvider;
        }
        /// <summary>
        /// 运行启动任务
        /// </summary>
        /// <param name="typeFinder">类型查找器</param>
        protected virtual void RunStartupTasks(ITypeFinder typeFinder)
        {
            //从其他程序集中查找启动任务控制器
            var startupTasks = typeFinder.FindClassesOfType<IStartupTask>();
            //创建启动示例并对其进行排序
            //启动该接口不依赖于插件是否安装，都会去启动
            //否则DbContext的初始化工作无法完成，插件将无法进行工作
            var instances = startupTasks
                .Select(startupTask => (IStartupTask)Activator.CreateInstance(startupTask))
                .OrderBy(startupTask => startupTask.Order);

            foreach (var task in instances)
                task.Execute();
        }
        /// <summary>
        /// 通过Autofac注册程序集
        /// </summary>
        /// <param name="config"></param>
        /// <param name="services"></param>
        /// <param name="typeFinder"></param>
        /// <returns></returns>
        protected virtual IServiceProvider RegisterDependencies(OarmsConfig config,IServiceCollection services,ITypeFinder typeFinder)
        {
            var containerBuilder = new ContainerBuilder();
            //注册系统引擎
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();
            //注册类型查找器
            containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            //通过程序集查找依赖注入的提供器
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            //创建依赖注入的实例并对其进行排序
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);
            //遍历所有注入的程序集
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(containerBuilder, typeFinder, config);
            //已注册的程序集通过Autofac容器将进行注入
            containerBuilder.Populate(services);
            //创建服务提供器
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());
            return _serviceProvider;
        }

        /// <summary>
        /// 创建并且配置AutoMapper
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeFinder"></param>
        protected virtual void AddAutoMapper(IServiceCollection services, ITypeFinder typeFinder)
        {
            //find mapper configurations provided by other assemblies
            var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                .Where(mapperConfiguration => PluginManager.FindPlugin(mapperConfiguration)?.Installed ?? true) //ignore not installed plugins
                .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            //register
            AutoMapperConfiguration.Init(config);
        }


        public void ConfigureRequestPipeline(IApplicationBuilder application)
        {
            throw new NotImplementedException();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        public object ResolveUnregistered(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
