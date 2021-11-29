using System.Text.RegularExpressions;

namespace Notifier.Server.Extensions
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
