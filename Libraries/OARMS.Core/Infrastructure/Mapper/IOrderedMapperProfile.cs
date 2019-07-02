//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 14:13:51
//    说明：
//    版权所有：个人
//***********************************************************

namespace OARMS.Core.Infrastructure.Mapper
{
    /// <summary>
    /// 映射配置文件的注册接口
    /// </summary>
    public interface IOrderedMapperProfile
    {
        /// <summary>
        /// 获取配置信息的实现顺序
        /// </summary>
        int Order { get; }
    }
}
