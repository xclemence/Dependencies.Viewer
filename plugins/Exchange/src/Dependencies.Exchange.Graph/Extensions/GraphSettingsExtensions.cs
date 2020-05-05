using System;
using Dependencies.Exchange.Graph.Settings;

namespace Dependencies.Exchange.Graph.Extensions
{
    internal static class GraphSettingsExtensions
    {
        public static bool IsValide(this GraphSettings settings) =>
            Uri.TryCreate(settings.ServiceUri, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
