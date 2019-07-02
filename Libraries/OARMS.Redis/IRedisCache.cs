//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-06-29 10:08:49
//    说明：Redis缓存操作接口
//    版权所有：个人
//***********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OARMS.Redis
{
    public interface IRedisCache :IDisposable
    {
        /// <summary>
        /// 获取key数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="acquire"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        T Get<T>(string key, Func<T> acquire, int? cacheTime = null);
        /// <summary>
        /// 设置key数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        void Set(string key, object data, int cacheTime);
        /// <summary>
        /// 判断是否存在key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsExist(string key);
        /// <summary>
        /// 移除key数据
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 通过正则表达式删除
        /// </summary>
        /// <param name="pattern"></param>
        void RemoveByPattern(string pattern);
        /// <summary>
        /// 清除
        /// </summary>
        void Clear();
    }
}
