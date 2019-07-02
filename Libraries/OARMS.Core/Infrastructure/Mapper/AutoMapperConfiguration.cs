//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 14:16:07
//    说明：
//    版权所有：个人
//***********************************************************
using AutoMapper;

namespace OARMS.Core.Infrastructure.Mapper
{
    /// <summary>
    /// AutoMapper配置类
    /// </summary>
    public class AutoMapperConfiguration
    {
        /// <summary>
        /// 映射
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// 映射配置
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; private set; }

        /// <summary>
        /// 初始化映射
        /// </summary>
        /// <param name="config">Mapper configuration</param>
        public static void Init(MapperConfiguration config)
        {
            MapperConfiguration = config;
            Mapper = config.CreateMapper();
        }
    }
}
