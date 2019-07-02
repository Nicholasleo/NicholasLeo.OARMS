//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 15:54:56
//    说明：
//    版权所有：个人
//***********************************************************
using Newtonsoft.Json;
using OARMS.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OARMS.Core.Plugins
{
    public class PluginDescriptor : IDescriptor, IComparable<PluginDescriptor>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public PluginDescriptor()
        {
            this.SupportedVersions = new List<string>();
            this.LimitedToStores = new List<int>();
            this.LimitedToCustomerRoles = new List<int>();
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="assembly"></param>
        public PluginDescriptor(Assembly assembly) : this()
        {
            this.ReferencedAssembly = assembly;
        }

        /// <summary>
        /// 获取插件的实例
        /// </summary>
        /// <returns>插件实例</returns>
        public IPlugin Instance()
        {
            return Instance<IPlugin>();
        }

        /// <summary>
        /// 获取插件的实例
        /// </summary>
        /// <typeparam name="T">插件类型</typeparam>
        /// <returns>插件实例</returns>
        public virtual T Instance<T>() where T : class, IPlugin
        {
            object instance = null;
            try
            {
                instance = EngineContext.Current.Resolve(PluginType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (instance == null)
                instance = EngineContext.Current.ResolveUnregistered(PluginType);

            var typedInstance = instance as T;
            if (typedInstance != null)
                typedInstance.PluginDescriptor = this;
            return typedInstance;
        }

        /// <summary>
        /// 比较PluginDescriptor对象
        /// </summary>
        /// <param name="other">比较的PluginDescriptor对象</param>
        /// <returns></returns>
        public int CompareTo(PluginDescriptor other)
        {
            if (DisplayOrder != other.DisplayOrder)
                return DisplayOrder.CompareTo(other.DisplayOrder);

            return FriendlyName.CompareTo(other.FriendlyName);
        }
        /// <summary>
        /// 返回插件的字符串类型名称
        /// </summary>
        /// <returns>友元名称</returns>
        public override string ToString()
        {
            return FriendlyName;
        }

        /// <summary>
        /// 判断制定的PluginDescriptor对象是否有相同的系统名存在
        /// </summary>
        /// <param name="value">进行判断的PluginDescriptor实例</param>
        /// <returns>存在返回True,否则返回Fasle</returns>
        public override bool Equals(object value)
        {
            return SystemName?.Equals((value as PluginDescriptor)?.SystemName) ?? false;
        }

        /// <summary>
        /// 返回插件的Hash值
        /// </summary>
        /// <returns>32位hash值</returns>
        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }


        /// <summary>
        /// 获取或设置插件分组
        /// </summary>
        [JsonProperty(PropertyName = "Group")]
        public virtual string Group { get; set; }

        /// <summary>
        /// 获取或设置插件友元名称
        /// </summary>
        [JsonProperty(PropertyName = "FriendlyName")]
        public virtual string FriendlyName { get; set; }

        /// <summary>
        /// 获取或设置插件名称
        /// </summary>
        [JsonProperty(PropertyName = "SystemName")]
        public virtual string SystemName { get; set; }

        /// <summary>
        /// 获取或设置插件版本号
        /// </summary>
        [JsonProperty(PropertyName = "Version")]
        public virtual string Version { get; set; }

        /// <summary>
        /// 获取或设置插件支持的版本号
        /// </summary>
        [JsonProperty(PropertyName = "SupportedVersions")]
        public virtual IList<string> SupportedVersions { get; set; }

        /// <summary>
        /// 获取或设置插件作者
        /// </summary>
        [JsonProperty(PropertyName = "Author")]
        public virtual string Author { get; set; }

        /// <summary>
        /// 获取或设置插件显示顺序
        /// </summary>
        [JsonProperty(PropertyName = "DisplayOrder")]
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 获取或设置插件文件名称
        /// </summary>
        [JsonProperty(PropertyName = "FileName")]
        public virtual string AssemblyFileName { get; set; }

        /// <summary>
        /// 获取或设置插件描述信息
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        public virtual string Description { get; set; }

        /// <summary>
        /// 获取或设置存储插件列表中的标识
        /// </summary>
        [JsonProperty(PropertyName = "LimitedToStores")]
        public virtual IList<int> LimitedToStores { get; set; }

        /// <summary>
        /// 获取或设置插件角色权限，为空则权限为所有
        /// </summary>
        [JsonProperty(PropertyName = "LimitedToCustomerRoles")]
        public virtual IList<int> LimitedToCustomerRoles { get; set; }

        /// <summary>
        /// 获取或设置插件是否已经安装
        /// </summary>
        [JsonIgnore]
        public virtual bool Installed { get; set; }

        /// <summary>
        /// 获取或设置插件类型
        /// </summary>
        [JsonIgnore]
        public virtual Type PluginType { get; set; }

        /// <summary>
        /// 获取或设置插件的原生程序集文件
        /// </summary>
        [JsonIgnore]
        public virtual string OriginalAssemblyFile { get; internal set; }

        /// <summary>
        /// 获取或设置已经复制且在运行的插件程序集
        /// </summary>
        [JsonIgnore]
        public virtual Assembly ReferencedAssembly { get; internal set; }
    }
}
