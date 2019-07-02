//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 18:54:44
//    说明：
//    版权所有：个人
//***********************************************************

namespace OARMS.Core.Cache
{
    public static partial class OarmsCachingDefaults
    {
        public static int CacheTime => 60;
        public static string RedisDataProtectionKey => "Oarms.DataProtectionKeys";
    }
}
