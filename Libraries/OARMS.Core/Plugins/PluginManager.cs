//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 15:47:46
//    说明：插件机制--插件管理实现类
//    版权所有：个人
//***********************************************************

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Newtonsoft.Json;
using OARMS.Core.ComponentModel;
using OARMS.Core.Configuration;
using OARMS.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace OARMS.Core.Plugins
{
    public class PluginManager
    {
        #region 字段
        private static readonly IOarmsFileProvider _fileProvider;
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        private static readonly List<string> _baseAppLibraries;
        private static string _shadowCopyFolder;
        private static string _reserveShadowCopyFolder;

        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        static PluginManager()
        {
            //在DI注入还没有初始化之前使用默认的Provider
            _fileProvider = CommonHelper.DefaultFileProvider;

            //获取根目录下的所有动态链接库
            _baseAppLibraries = _fileProvider.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Select(f => _fileProvider.GetFileName(f)).ToList();

            //获取所有网站目录下面所有的动态链接库
            if (!AppDomain.CurrentDomain.BaseDirectory.Equals(Environment.CurrentDirectory, StringComparison.InvariantCultureIgnoreCase))
                _baseAppLibraries.AddRange(_fileProvider.GetFiles(Environment.CurrentDirectory, "*.dll").Select(f => _fileProvider.GetFileName(f)));
            
            //根据提供的目录名获取其目录下的所有动态链接库
            var refsPathName = _fileProvider.Combine(Environment.CurrentDirectory, OarmsPluginDefaults.RefsPathName);
            if (_fileProvider.DirectoryExists(refsPathName))
                _baseAppLibraries.AddRange(_fileProvider.GetFiles(refsPathName, "*.dll").Select(f => _fileProvider.GetFileName(f)));
        }

        /// <summary>
        /// 获取描述文件
        /// </summary>
        /// <param name="pluginFolder"></param>
        /// <returns></returns>
        private static IEnumerable<KeyValuePair<string, PluginDescriptor>> GetDescriptionFilesAndDescriptors(string pluginFolder)
        {
            if (pluginFolder == null)
                throw new ArgumentNullException(nameof(pluginFolder));

            //创建列表用于存放（文件信息，解析的插件描述信息）
            var result = new List<KeyValuePair<string, PluginDescriptor>>();

            //向列表中添加显示顺序和路径
            foreach (var descriptionFile in _fileProvider.GetFiles(pluginFolder, OarmsPluginDefaults.DescriptionFileName, false))
            {
                if (!IsPackagePluginFolder(_fileProvider.GetDirectoryName(descriptionFile)))
                    continue;

                //解析文件
                var pluginDescriptor = GetPluginDescriptorFromFile(descriptionFile);
                //填充列表
                result.Add(new KeyValuePair<string, PluginDescriptor>(descriptionFile, pluginDescriptor));
            }
            //进行显示顺序排序
            result.Sort((firstPair, nextPair) => firstPair.Value.DisplayOrder.CompareTo(nextPair.Value.DisplayOrder));
            return result;
        }
        /// <summary>
        /// 获取安装的插件信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static IList<string> GetInstalledPluginNames(string filePath)
        {
            if (!_fileProvider.FileExists(filePath))
            {
                filePath = _fileProvider.MapPath(OarmsPluginDefaults.ObsoleteInstalledPluginsFilePath);
                if (!_fileProvider.FileExists(filePath))
                    return new List<string>();
                var pluginSystemNames = new List<string>();
                using (var reader = new StringReader(_fileProvider.ReadAllText(filePath, Encoding.UTF8)))
                {
                    string pluginName;
                    while ((pluginName = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(pluginName))
                            pluginSystemNames.Add(pluginName.Trim());
                    }
                }
                //保存新安装的插件名称
                SaveInstalledPluginNames(pluginSystemNames, _fileProvider.MapPath(OarmsPluginDefaults.InstalledPluginsFilePath));

                //同时删除旧插件
                _fileProvider.DeleteFile(filePath);

                return pluginSystemNames;
            }
            var text = _fileProvider.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new List<string>();

            //从JSON文件中获取插件名称
            return JsonConvert.DeserializeObject<IList<string>>(text);
        }
        /// <summary>
        /// 保存安装的插件信息到文件中
        /// </summary>
        /// <param name="pluginSystemNames"></param>
        /// <param name="filePath"></param>
        private static void SaveInstalledPluginNames(IList<string> pluginSystemNames, string filePath)
        {
            //保存信息到文件中
            var text = JsonConvert.SerializeObject(pluginSystemNames, Formatting.Indented);
            _fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// 从插件描述文件中获取插件描述
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static PluginDescriptor GetPluginDescriptorFromFile(string filePath)
        {
            var text = _fileProvider.ReadAllText(filePath,Encoding.UTF8);
            return GetPluginDescriptorFromText(text);
        }

        /// <summary>
        /// 从描述文本中获取插件描述
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static PluginDescriptor GetPluginDescriptorFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new PluginDescriptor();

            var descriptor = JsonConvert.DeserializeObject<PluginDescriptor>(text);

            if (!descriptor.SupportedVersions.Any())
                descriptor.SupportedVersions.Add("2.00");

            return descriptor;
        }

        /// <summary>
        /// 判断是否插件文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static bool IsPackagePluginFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return false;
            var parent = _fileProvider.GetParentDirectory(folder);

            if (string.IsNullOrEmpty(parent))
                return false;

            if (!_fileProvider.GetDirectoryNameOnly(parent).Equals(OarmsPluginDefaults.PathName, StringComparison.InvariantCultureIgnoreCase))
                return false;
            return true;
        }
        /// <summary>
        /// 判断是否已加载程序集文件
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>True if assembly file is already loaded; otherwise false</returns>
        private static bool IsAlreadyLoaded(string filePath)
        {
            //在基目录中搜索库文件名（忽略已存在的库）
            //之所以要这样做，是由于并非所有的库文件都是在程序启动后立即加载
            if (_baseAppLibraries.Any(sli => sli.Equals(_fileProvider.GetFileName(filePath), StringComparison.InvariantCultureIgnoreCase)))
                return true;
            //不比较程序集的完整名称，只比较文件名称
            try
            {
                var fileNameWithoutExt = _fileProvider.GetFileNameWithoutExtension(filePath);
                if (string.IsNullOrEmpty(fileNameWithoutExt))
                    throw new Exception($"无法获取{_fileProvider.GetFileName(filePath)}文件的拓展名");

                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var assemblyName = a.FullName.Split(',').FirstOrDefault();
                    if (fileNameWithoutExt.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("无法验证已加载的程序集" + exc);
            }

            return false;
        }

        /// <summary>
        /// 执行部署文件
        /// </summary>
        /// <param name="plug">插件信息</param>
        /// <param name="applicationPartManager">应用程序部件管理器</param>
        /// <param name="config">系统配置</param>
        /// <param name="shadowCopyPath">复制路径</param>
        /// <returns>Assembly</returns>
        private static Assembly PerformFileDeploy(string plug, ApplicationPartManager applicationPartManager, OarmsConfig config, string shadowCopyPath = "")
        {
            var parent = string.IsNullOrEmpty(plug) ? string.Empty : _fileProvider.GetParentDirectory(plug);

            if (string.IsNullOrEmpty(parent))
                throw new InvalidOperationException($"{_fileProvider.GetFileName(plug)}插件存在的目录是在系统允许的OarmsCommerce文件夹层次结构之外");

            if (!config.UsePluginsShadowCopy)
                return RegisterPluginDefinition(config, applicationPartManager, plug);

            //为了避免可能出现的异常，将库文件复制到 ~/Plugins/bin/ 目录中
            if (string.IsNullOrEmpty(shadowCopyPath))
                shadowCopyPath = _shadowCopyFolder;

            _fileProvider.CreateDirectory(shadowCopyPath);
            var shadowCopiedPlug = ShadowCopyFile(plug, shadowCopyPath);

            Assembly shadowCopiedAssembly = null;

            try
            {
                shadowCopiedAssembly = RegisterPluginDefinition(config, applicationPartManager, shadowCopiedPlug);
            }
            catch (FileLoadException)
            {
                if (!config.CopyLockedPluginAssembilesToSubdirectoriesOnStartup || !shadowCopyPath.Equals(_shadowCopyFolder))
                    throw;
            }

            return shadowCopiedAssembly ?? PerformFileDeploy(plug, applicationPartManager, config, _reserveShadowCopyFolder);
        }

        /// <summary>
        /// 注册插件定义
        /// </summary>
        /// <param name="config">系统配置</param>
        /// <param name="applicationPartManager">应用程序部件管理器</param>
        /// <param name="plug">插件信息</param>
        /// <returns>Assembly</returns>
        private static Assembly RegisterPluginDefinition(OarmsConfig config, ApplicationPartManager applicationPartManager, string plug)
        {
            //we can now register the plugin definition
            Assembly pluginAssembly;
            try
            {
                pluginAssembly = Assembly.LoadFrom(plug);
            }
            catch (FileLoadException)
            {
                if (config.UseUnsafeLoadAssembly)
                {
                    //如果应用程序已经从Web复制，则会将其标记为Web应用程序
                    //即便它是在本机计算机中，也可以通过更改文件的属性来更改插件的名称
                    //或者可以使用<loadFromRemoteSources>来标记授予程序集完全信任权限，作为替代的方法
                    //也可以使用unsafeloadFrom方法加载操作系统标记为的本地程序集
                    //从Web中加载已经加载了的文件
                    //http://go.microsoft.com/fwlink/?LinkId=155569 获取更多信息
                    pluginAssembly = Assembly.UnsafeLoadFrom(plug);
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("添加到应用程序部件: '{0}'", pluginAssembly.FullName);
            applicationPartManager.ApplicationParts.Add(new AssemblyPart(pluginAssembly));

            return pluginAssembly;
        }

        /// <summary>
        /// 将插件复制到插件的复制目录中
        /// </summary>
        /// <param name="pluginFilePath">插件路径</param>
        /// <param name="shadowCopyPlugFolder">复制目录路径</param>
        /// <returns>插件文件的复制目录路径</returns>
        private static string ShadowCopyFile(string pluginFilePath, string shadowCopyPlugFolder)
        {
            var shouldCopy = true;
            var shadowCopiedPlug = _fileProvider.Combine(shadowCopyPlugFolder, _fileProvider.GetFileName(pluginFilePath));

            //检查插件副本是否存在，如果存在，检查是否更新插件，不存在，确认是否不复制
            if (_fileProvider.FileExists(shadowCopiedPlug))
            {
                //系统中最好使用LastWriteTimeUTC方法，但是并不是所有的系统文件都具有改属性，或许比较文件的哈希会更好点
                var areFilesIdentical = _fileProvider.GetCreationTime(shadowCopiedPlug).ToUniversalTime().Ticks >= _fileProvider.GetCreationTime(pluginFilePath).ToUniversalTime().Ticks;
                if (areFilesIdentical)
                {
                    Debug.WriteLine("无法复制; 文件似乎是一样的: '{0}'", _fileProvider.GetFileName(shadowCopiedPlug));
                    shouldCopy = false;
                }
                else
                {
                    //删除已存在的文件
                    Debug.WriteLine("发现新插件; 删除就插件: '{0}'", _fileProvider.GetFileName(shadowCopiedPlug));
                    _fileProvider.DeleteFile(shadowCopiedPlug);
                }
            }

            if (!shouldCopy)
                return shadowCopiedPlug;

            try
            {
                _fileProvider.FileCopy(pluginFilePath, shadowCopiedPlug, true);
            }
            catch (IOException)
            {
                Debug.WriteLine(shadowCopiedPlug + " 已锁定, 正在尝试重新命名");
                //当文件被锁定时会出现如下情况：
                //因为某些原因的存在，devenv有时会锁定插件文件，而另一个原因是，允许重命名的这些文件释放了锁
                try
                {
                    var oldFile = shadowCopiedPlug + Guid.NewGuid().ToString("N") + ".old";
                    _fileProvider.FileMove(shadowCopiedPlug, oldFile);
                }
                catch (IOException exc)
                {
                    throw new IOException(shadowCopiedPlug + " 重命名师表, 无法初始化插件", exc);
                }
                //现在重新尝试复制
                _fileProvider.FileCopy(pluginFilePath, shadowCopiedPlug, true);
            }

            return shadowCopiedPlug;
        }

        #region Methods

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="applicationPartManager">应用程序部件管理器</param>
        /// <param name="config">系统配置</param>
        public static void Initialize(ApplicationPartManager applicationPartManager, OarmsConfig config)
        {
            if (applicationPartManager == null)
                throw new ArgumentNullException(nameof(applicationPartManager));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            using (new WriteLockDisposable(Locker))
            {
                // 在该处添加详细的异常处理， 这是由于在应用程序启动时发生的，可能会阻止应用程序的启动
                var pluginFolder = _fileProvider.MapPath(OarmsPluginDefaults.Path);
                _shadowCopyFolder = _fileProvider.MapPath(OarmsPluginDefaults.ShadowCopyPath);
                _reserveShadowCopyFolder = _fileProvider.Combine(_fileProvider.MapPath(OarmsPluginDefaults.ShadowCopyPath), $"{OarmsPluginDefaults.ReserveShadowCopyPathName}{DateTime.Now.ToFileTimeUtc()}");

                var referencedPlugins = new List<PluginDescriptor>();
                var incompatiblePlugins = new List<string>();

                try
                {
                    var installedPluginSystemNames = GetInstalledPluginNames(_fileProvider.MapPath(OarmsPluginDefaults.InstalledPluginsFilePath));

                    Debug.WriteLine("正在创建影像的文件夹并查询dll");
                    //确保文件夹已创建
                    _fileProvider.CreateDirectory(pluginFolder);
                    _fileProvider.CreateDirectory(_shadowCopyFolder);

                    //获取bin下面的所有文件
                    var binFiles = _fileProvider.GetFiles(_shadowCopyFolder, "*", false);
                    if (config.ClearPluginShadowDirectoryOnStartup)
                    {
                        //清除复制的副本文件
                        foreach (var f in binFiles)
                        {
                            if (_fileProvider.GetFileName(f).Equals("placeholder.txt", StringComparison.InvariantCultureIgnoreCase))
                                continue;

                            Debug.WriteLine("删除中 " + f);
                            try
                            {
                                //忽然index.htm
                                var fileName = _fileProvider.GetFileName(f);
                                if (fileName.Equals("index.htm", StringComparison.InvariantCultureIgnoreCase))
                                    continue;

                                _fileProvider.DeleteFile(f);
                            }
                            catch (Exception exc)
                            {
                                Debug.WriteLine("删除文件时出错 " + f + ". Exception: " + exc);
                            }
                        }

                        //删除所有保留的文件夹
                        foreach (var directory in _fileProvider.GetDirectories(_shadowCopyFolder, OarmsPluginDefaults.ReserveShadowCopyPathNamePattern))
                        {
                            try
                            {
                                _fileProvider.DeleteDirectory(directory);
                            }
                            catch
                            {
                                //do nothing
                            }
                        }
                    }

                    //加载描述文件
                    foreach (var dfd in GetDescriptionFilesAndDescriptors(pluginFolder))
                    {
                        var descriptionFile = dfd.Key;
                        var pluginDescriptor = dfd.Value;

                        //确保插件的版本是有效的
                        if (!pluginDescriptor.SupportedVersions.Contains(OarmsVersion.CurrentVersion, StringComparer.InvariantCultureIgnoreCase))
                        {
                            incompatiblePlugins.Add(pluginDescriptor.SystemName);
                            continue;
                        }

                        //验证插件命名
                        if (string.IsNullOrWhiteSpace(pluginDescriptor.SystemName))
                            throw new Exception($"'{descriptionFile}' 没有系统命名. 重新为插件进行唯一命名并重新编译.");
                        if (referencedPlugins.Contains(pluginDescriptor))
                            throw new Exception($"'{pluginDescriptor.SystemName}' 插件命名在系统中已存在");

                        //设置已安装属性
                        pluginDescriptor.Installed = installedPluginSystemNames
                            .FirstOrDefault(x => x.Equals(pluginDescriptor.SystemName, StringComparison.InvariantCultureIgnoreCase)) != null;

                        try
                        {
                            var directoryName = _fileProvider.GetDirectoryName(descriptionFile);
                            if (string.IsNullOrEmpty(directoryName))
                                throw new Exception($"无法为插件'{_fileProvider.GetFileName(descriptionFile)}'描述文件解析目录");

                            //获取插件中所有DLL的列表(不在bin中)
                            var pluginFiles = _fileProvider.GetFiles(directoryName, "*.dll", false)
                                //确保没有注册复制的副本插件
                                .Where(x => !binFiles.Select(q => q).Contains(x))
                                .Where(x => IsPackagePluginFolder(_fileProvider.GetDirectoryName(x)))
                                .ToList();

                            //其他插件描述信息
                            var mainPluginFile = pluginFiles
                                .FirstOrDefault(x => _fileProvider.GetFileName(x).Equals(pluginDescriptor.AssemblyFileName, StringComparison.InvariantCultureIgnoreCase));

                            //插件目录错误
                            if (mainPluginFile == null)
                            {
                                incompatiblePlugins.Add(pluginDescriptor.SystemName);
                                continue;
                            }

                            pluginDescriptor.OriginalAssemblyFile = mainPluginFile;

                            //复制主插件副本
                            pluginDescriptor.ReferencedAssembly = PerformFileDeploy(mainPluginFile, applicationPartManager, config);

                            //加载所有其他引用的程序集
                            foreach (var plugin in pluginFiles
                                .Where(x => !_fileProvider.GetFileName(x).Equals(_fileProvider.GetFileName(mainPluginFile), StringComparison.InvariantCultureIgnoreCase))
                                .Where(x => !IsAlreadyLoaded(x)))
                                PerformFileDeploy(plugin, applicationPartManager, config);

                            //初始化插件类型（每个插件只能是一个程序集）
                            foreach (var t in pluginDescriptor.ReferencedAssembly.GetTypes())
                                if (typeof(IPlugin).IsAssignableFrom(t))
                                    if (!t.IsInterface)
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            pluginDescriptor.PluginType = t;
                                            break;
                                        }

                            referencedPlugins.Add(pluginDescriptor);
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            //添加插件名称，便于快速识别有问题的插件
                            var msg = $"Plugin '{pluginDescriptor.FriendlyName}'. ";
                            foreach (var e in ex.LoaderExceptions)
                                msg += e.Message + Environment.NewLine;

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                        catch (Exception ex)
                        {
                            //添加插件名称，便于快速识别有问题的插件
                            var msg = $"Plugin '{pluginDescriptor.FriendlyName}'. {ex.Message}";

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    for (var e = ex; e != null; e = e.InnerException)
                        msg += e.Message + Environment.NewLine;

                    var fail = new Exception(msg, ex);
                    throw fail;
                }

                ReferencedPlugins = referencedPlugins;
                IncompatiblePlugins = incompatiblePlugins;
            }
        }

        /// <summary>
        /// 标记已安装的插件
        /// </summary>
        /// <param name="systemName">插件名称</param>
        public static void MarkPluginAsInstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException(nameof(systemName));

            var filePath = _fileProvider.MapPath(OarmsPluginDefaults.InstalledPluginsFilePath);

            //如果文件不存在，则创建文件
            _fileProvider.CreateFile(filePath);

            //获取已安装的插件名称
            var installedPluginSystemNames = GetInstalledPluginNames(filePath);

            //将不存在的插件添加到插件列表中去
            var alreadyMarkedAsInstalled = installedPluginSystemNames.Any(pluginName => pluginName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
            if (!alreadyMarkedAsInstalled)
                installedPluginSystemNames.Add(systemName);

            //保存已安装的插件名称到文件中去
            SaveInstalledPluginNames(installedPluginSystemNames, filePath);
        }

        /// <summary>
        /// 标记卸载的插件
        /// </summary>
        /// <param name="systemName">插件名称</param>
        public static void MarkPluginAsUninstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException(nameof(systemName));

            var filePath = _fileProvider.MapPath(OarmsPluginDefaults.InstalledPluginsFilePath);

            //如果文件不存在，则创建文件
            _fileProvider.CreateFile(filePath);

            //获取已安装的插件名称
            var installedPluginSystemNames = GetInstalledPluginNames(filePath);

            //从插件列表中移除已存在的指定插件
            var alreadyMarkedAsInstalled = installedPluginSystemNames.Any(pluginName => pluginName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
            if (alreadyMarkedAsInstalled)
                installedPluginSystemNames.Remove(systemName);

            //保存已安装的插件名称到文件中去
            SaveInstalledPluginNames(installedPluginSystemNames, filePath);
        }

        /// <summary>
        /// 标记卸载的插件
        /// </summary>
        public static void MarkAllPluginsAsUninstalled()
        {
            var filePath = _fileProvider.MapPath(OarmsPluginDefaults.InstalledPluginsFilePath);
            if (_fileProvider.FileExists(filePath))
                _fileProvider.DeleteFile(filePath);
        }

        /// <summary>
        /// 在同一程序集中查找与插件相同的插件描述
        /// </summary>
        /// <param name="typeInAssembly">Type</param>
        /// <returns>存在的返回插件描述信息; 否则返回 null</returns>
        public static PluginDescriptor FindPlugin(Type typeInAssembly)
        {
            if (typeInAssembly == null)
                throw new ArgumentNullException(nameof(typeInAssembly));

            return ReferencedPlugins?.FirstOrDefault(plugin =>
                plugin.ReferencedAssembly != null &&
                plugin.ReferencedAssembly.FullName.Equals(typeInAssembly.Assembly.FullName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 将插件描述保存到插件描述文件里去
        /// </summary>
        /// <param name="pluginDescriptor">插件描述</param>
        public static void SavePluginDescriptor(PluginDescriptor pluginDescriptor)
        {
            if (pluginDescriptor == null)
                throw new ArgumentException(nameof(pluginDescriptor));

            //get the description file path
            if (pluginDescriptor.OriginalAssemblyFile == null)
                throw new Exception($"无法加载插件{pluginDescriptor.SystemName}的原始程序集路径.");

            var filePath = _fileProvider.Combine(_fileProvider.GetDirectoryName(pluginDescriptor.OriginalAssemblyFile), OarmsPluginDefaults.DescriptionFileName);
            if (!_fileProvider.FileExists(filePath))
                throw new Exception($"{pluginDescriptor.SystemName}插件描述文件不存在. {filePath}");

            //save the file
            var text = JsonConvert.SerializeObject(pluginDescriptor, Formatting.Indented);
            _fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// 从硬盘中删除插件目录
        /// </summary>
        /// <param name="pluginDescriptor">插件描述</param>
        /// <returns>删除成功返回True</returns>
        public static bool DeletePlugin(PluginDescriptor pluginDescriptor)
        {
            //未设置插件描述
            if (pluginDescriptor == null)
                return false;

            //检查插件是否已安装
            if (pluginDescriptor.Installed)
                return false;

            var directoryName = _fileProvider.GetDirectoryName(pluginDescriptor.OriginalAssemblyFile);

            if (_fileProvider.DirectoryExists(directoryName))
                _fileProvider.DeleteDirectory(directoryName);

            return true;
        }

        /// <summary>
        /// 获取插件LOGO URL
        /// </summary>
        /// <param name="pluginDescriptor">插件描述</param>
        /// <returns>Logo URL</returns>
        public static string GetLogoUrl(PluginDescriptor pluginDescriptor)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            var pluginDirectory = _fileProvider.GetDirectoryName(pluginDescriptor.OriginalAssemblyFile);
            if (string.IsNullOrEmpty(pluginDirectory))
                return null;

            var logoExtension = OarmsPluginDefaults.SupportedLogoImageExtensions
                .FirstOrDefault(ext => _fileProvider.FileExists(_fileProvider.Combine(pluginDirectory, $"{OarmsPluginDefaults.LogoFileName}.{ext}")));
            if (string.IsNullOrWhiteSpace(logoExtension))
                return null; //找不到任何支持的拓展名的LOGO文件

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            var logoUrl = $"{webHelper.GetStoreLocation()}plugins/{_fileProvider.GetDirectoryNameOnly(pluginDirectory)}/{OarmsPluginDefaults.LogoFileName}.{logoExtension}";
            return logoUrl;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置已经被复制副本的所有引用的集合
        /// </summary>
        public static IEnumerable<PluginDescriptor> ReferencedPlugins { get; set; }

        /// <summary>
        /// 获取或设置与当前版本不兼容的插件集合
        /// </summary>
        public static IEnumerable<string> IncompatiblePlugins { get; set; }

        #endregion
    }
}
