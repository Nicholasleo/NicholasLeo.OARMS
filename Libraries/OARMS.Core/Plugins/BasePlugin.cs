//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-02 16:20:22
//    说明：
//    版权所有：个人
//***********************************************************

namespace OARMS.Core.Plugins
{
    /// <summary>
    /// 插件抽象类
    /// </summary>
    public abstract class BasePlugin : IPlugin
    {
        public PluginDescriptor PluginDescriptor { get; set; }

        public virtual string GetConfigurationPageUrl()
        {
            return null;
        }

        public virtual void Install()
        {
            PluginManager.MarkPluginAsInstalled(PluginDescriptor.SystemName);
        }

        public virtual void Uninstall()
        {
            PluginManager.MarkPluginAsUninstalled(PluginDescriptor.SystemName);
        }
    }
}
