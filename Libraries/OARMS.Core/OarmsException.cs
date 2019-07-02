//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 18:02:26
//    说明：
//    版权所有：个人
//***********************************************************
using System;
using System.Runtime.Serialization;

namespace OARMS.Core
{
    public class OarmsException : Exception
    {
        /// <summary>
        /// 初始化一个新的异常实例
        /// </summary>
        public OarmsException()
        {
        }

        /// <summary>
        /// 初始化一个新的异常实例，并指定异常信息
        /// </summary>
        /// <param name="message"></param>
        public OarmsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 初始化一个新的异常实例，并指定异常信息
        /// </summary>
        /// <param name="messageFormat">异常信息格式</param>
        /// <param name="args">异常信息参数</param>
        public OarmsException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        /// <summary>
        /// 初始化一个新的异常实例，并指定序列化数据异常信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected OarmsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// 用指定的错误消息和对导致此异常的内部异常的引用初始化异常类的新实例。
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="innerException">内部异常</param>
        public OarmsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
