using Notifier.Core.Extensions;

namespace Notifier.Core
{
    /// <summary>
    /// code builder
    /// </summary>
    public class CodeBuilder
    {
        /// <summary>
        /// generate user code
        /// </summary>
        /// <returns></returns>
        public static string CreateUserCode()
        {
            return RandomExtension.GetRandomizer(8, true, false, true, true);
        }
    }
}
