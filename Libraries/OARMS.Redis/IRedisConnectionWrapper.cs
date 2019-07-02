//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-06-29 14:23:40
//    说明：
//    版权所有：个人
//***********************************************************
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OARMS.Redis
{
    public interface IRedisConnectionWrapper
    {
        /// <summary>
        /// 获取Redis数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IDatabase GetDatabase(int? db = null);
        /// <summary>
        /// 获取Redis服务
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        IServer GetServer(EndPoint endPoint);
        /// <summary>
        /// 获取服务终结点
        /// </summary>
        /// <returns></returns>
        EndPoint[] GetEndPoints();
        /// <summary>
        /// 清空数据库所有key
        /// </summary>
        /// <param name="db"></param>
        void FlushDatabase(int? db = null);
    }
}
