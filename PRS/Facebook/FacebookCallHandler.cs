using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PRS.Facebook
{
    public class FacebookCallHandler : HttpClientHandler

    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.RequestUri.AbsolutePath.Contains("/oauth"))
            {
                request.RequestUri = new Uri(request.RequestUri.AbsoluteUri.Replace("?access_token", "&access_token"));
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
