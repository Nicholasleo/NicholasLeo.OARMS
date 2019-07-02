//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 15:53:46
//    说明：
//    版权所有：个人
//***********************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace OARMS.Core.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        /// 获取配置页面Url
        /// </summary>
        /// <returns></returns>
        string GetConfigurationPageUrl();
        /// <summary>
        /// 获取或设置插件描述
        /// </summary>
        PluginDescriptor PluginDescriptor { get; set; }
        /// <summary>
        /// 插件安装
        /// </summary>
        void Install();
        /// <summary>
        /// 插件卸载
        /// </summary>
        void Uninstall();
    }
}
