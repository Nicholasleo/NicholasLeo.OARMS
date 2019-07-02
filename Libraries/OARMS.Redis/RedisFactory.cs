//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-06-29 12:14:11
//    说明：Redis实现工厂
//    版权所有：个人
//***********************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace OARMS.Redis
{
    public class RedisFactory
    {
        private static IRedisCache _IRedisCache = null;
        static RedisFactory()
        {
        }

        public IRedisCache CreateInstance()
        {
            if (_IRedisCache == null)
            {
            }
            return _IRedisCache;
        }
    }
}
