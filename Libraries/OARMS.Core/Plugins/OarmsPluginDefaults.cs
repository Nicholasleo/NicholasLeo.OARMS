//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 18:15:52
//    说明：
//    版权所有：个人
//***********************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace OARMS.Core.Plugins
{
    public partial class OarmsPluginDefaults
    {
        /// <summary>
        /// 获取系统已安装的插件的文件路径
        /// </summary>
        public static string ObsoleteInstalledPluginsFilePath => "~/App_Data/InstalledPlugins.txt";

        /// <summary>
        /// 获取系统已安装的插件的名称
        /// </summary>
        public static string InstalledPluginsFilePath => "~/App_Data/installedPlugins.json";

        /// <summary>
        /// 插件目录
        /// </summary>
        public static string Path => "~/Plugins";

        /// <summary>
        /// 插件目录名称
        /// </summary>
        public static string PathName => "Plugins";

        /// <summary>
        /// 插件存在的目录
        /// </summary>
        public static string ShadowCopyPath => "~/Plugins/bin";

        /// <summary>
        /// 获取插件refs目录
        /// </summary>
        public static string RefsPathName => "refs";

        /// <summary>
        /// 获取插件描述
        /// </summary>
        public static string DescriptionFileName => "plugin.json";

        /// <summary>
        /// 获取插件LOGO名称
        /// </summary>
        public static string LogoFileName => "logo";

        /// <summary>
        /// 获取插件保留的文件夹名称
        /// </summary>
        public static string ReserveShadowCopyPathName => "reserve_bin_";

        /// <summary>
        /// 获取插件保留的文件夹名称模式
        /// </summary>
        public static string ReserveShadowCopyPathNamePattern => "reserve_bin_*";

        /// <summary>
        /// 获取LOGO文件支持的拓展名
        /// </summary>
        public static List<string> SupportedLogoImageExtensions => new List<string> { "jpg", "png", "gif" };

        /// <summary>
        /// 获取上次文件的临时目录
        /// </summary>
        public static string UploadsTempPath => "~/App_Data/TempUploads";

        /// <summary>
        /// Gets the name of the file containing information about the uploaded items
        /// </summary>
        public static string UploadedItemsFileName => "uploadedItems.json";

        /// <summary>
        /// 获取主题目录
        /// </summary>
        public static string ThemesPath => "~/Themes";

        /// <summary>
        /// 获取主题描述文件名
        /// </summary>
        public static string ThemeDescriptionFileName => "theme.json";
    }
}
