//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-06-29 14:31:33
//    说明：系统配置类型模型
//    版权所有：个人
//***********************************************************

namespace OARMS.Core.Configuration
{
    public partial class OarmsConfig
    {
        /// <summary>
        /// 获取或设置生产环境中是否显示完整错误
        /// 在系统开发环境中会被忽略
        /// </summary>
        public bool DisplayFullErrorStack { get; set; }

        /// <summary>
        /// 获取或设置Azure Blob Storage 的连接字符串
        /// </summary>
        public string AzureBlobStorageConnectionString { get; set; }
        /// <summary>
        /// 获取或设置 Azure BLOB 容器名称
        /// </summary>
        public string AzureBlobStorageContainerName { get; set; }
        /// <summary>
        /// 获取或设置 Azure BLOB 容器终结点
        /// </summary>
        public string AzureBlobStorageEndPoint { get; set; }

        /// <summary>
        /// 获取或设置Redis是否进行缓存（不是内存缓存中的默认值）
        /// </summary>
        public bool RedisCachingEnabled { get; set; }
        /// <summary>
        /// 获取或设置Redis连接字符串
        /// 系统启用Redis缓存时启用
        /// </summary>
        public string RedisCachingConnectionString { get; set; }
        /// <summary>
        /// 获取或设置是否将系统配置信息保存到Redis数据库中
        /// </summary>
        public bool PersistDataProtectionKeysToRedis { get; set; }
        /// <summary>
        /// 获取或设置用户代理字符串
        /// </summary>
        public string UserAgentStringsPath { get; set; }
        /// <summary>
        /// Gets or sets path to database with crawler only user agent strings
        /// </summary>
        public string CrawlerOnlyUserAgentStringsPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a store owner can install sample data during installation
        /// </summary>
        public bool DisableSampleDataDuringInstallation { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to use fast installation. 
        /// By default this setting should always be set to "False" (only for advanced users)
        /// </summary>
        public bool UseFastInstallationService { get; set; }
        /// <summary>
        /// Gets or sets a list of plugins ignored during nopCommerce installation
        /// </summary>
        public string PluginsIgnoredDuringInstallation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should ignore startup tasks
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to clear /Plugins/bin directory on application startup
        /// </summary>
        public bool ClearPluginShadowDirectoryOnStartup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy "locked" assemblies from /Plugins/bin directory to temporary subdirectories on application startup
        /// </summary>
        public bool CopyLockedPluginAssembilesToSubdirectoriesOnStartup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to load an assembly into the load-from context, bypassing some security checks.
        /// </summary>
        public bool UseUnsafeLoadAssembly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy plugins library to the /Plugins/bin directory on application startup
        /// </summary>
        public bool UsePluginsShadowCopy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use backwards compatibility with SQL Server 2008 and SQL Server 2008R2
        /// </summary>
        public bool UseRowNumberForPaging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to store TempData in the session state.
        /// By default the cookie-based TempData provider is used to store TempData in cookies.
        /// </summary>
        public bool UseSessionStateTempDataProvider { get; set; }
    }
}
