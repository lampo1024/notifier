using Notifier.Core.Extensions;

namespace Notifier.Core
{
    /// <summary>
    /// 随机字符生成器
    /// </summary>
    public class RandomBuilder
    {
        /// <summary>
        /// 生成用户ID
        /// </summary>
        /// <returns></returns>
        public static string CreateUserId()
        {
            return RandomExtension.GetRandomizer(8, true, false, true, true);
        }
    }
}
