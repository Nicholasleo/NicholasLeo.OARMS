//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-06-29 13:37:55
//    说明：工具集合类-读取配置
//    版权所有：个人
//***********************************************************
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OARMS.Utilities
{
    /// <summary>
    /// 获取系统配置
    /// </summary>
    public class Configuration
    {
        private static IConfigurationSection _appSection = null;
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AppSetting(string key)
        {
            string str = string.Empty;
            if (_appSection.GetSection(key) != null)
            {
                str = _appSection.GetSection(key).Value;
            }
            return str;
        }
        public static void SetAppSetting(IConfigurationSection section)
        {
            _appSection = section;
        }

        public static string GetSite(string siteName)
        {
            return AppSetting(siteName);
        }
    }
}
