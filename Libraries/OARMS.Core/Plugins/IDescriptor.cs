//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 16:22:13
//    说明：
//    版权所有：个人
//***********************************************************

namespace OARMS.Core.Plugins
{
    /// <summary>
    /// 应用程序拓展描述（插件或主题）
    /// </summary>
    public interface IDescriptor
    {
        /// <summary>
        /// 获取或设置系统名称
        /// </summary>
        string SystemName { get; set; }

        /// <summary>
        /// 获取或设置友元名称
        /// </summary>
        string FriendlyName { get; set; }
    }
}
