using System;
using HLSSafariProxy_ASP.NET.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HLSSafariProxy_ASP.NET.Services
{
    public class UrlService : IUrlService
    {
        private const string UNKNOWN_HOST_NAME = "UNKNOWN-HOST";
        private const string MULTIPLE_HOST_NAME = "MULTIPLE-HOST";
        private const string COMMA = ",";
        
        public string GetSecondLevelManifestProxyUrl(HttpRequest httpRequest, string secondLevelManifestEndpoint)
        {
            var baseUrl = GetUri(httpRequest).GetLeftPart(UriPartial.Authority);
            return baseUrl + $"/{secondLevelManifestEndpoint}";
        }

        private Uri GetUri(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Scheme))
                throw new ArgumentException("Http request Scheme is not specified");

            return new Uri(string.Concat(
                request.Scheme,
                "://",
                request.Host.HasValue ? (request.Host.Value.IndexOf(COMMA, StringComparison.Ordinal) > 0 ? MULTIPLE_HOST_NAME : request.Host.Value) : UNKNOWN_HOST_NAME,
                request.PathBase.HasValue ? request.PathBase.Value : string.Empty,
                request.Path.HasValue ? request.Path.Value : string.Empty,
                request.QueryString.HasValue ? request.QueryString.Value : string.Empty));
        }
    }
}