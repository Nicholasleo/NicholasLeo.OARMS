//***********************************************************
//    作者：Nicholas Leo
//    E-Mail:nicholasleo1030@163.com
//    GitHub:https://github.com/nicholasleo
//    时间：2019-07-01 10:32:01
//    说明：
//    版权所有：个人
//***********************************************************
using System.Runtime.CompilerServices;

namespace OARMS.Core.Infrastructure
{
    public class EngineContext
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create()
        {
            return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new OarmsEngine());
        }

        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        public static IEngine Current
        {
            get {
                if (Singleton<IEngine>.Instance == null)
                {
                    Create();
                }
                return Singleton<IEngine>.Instance;
            }
        }
    }
}
