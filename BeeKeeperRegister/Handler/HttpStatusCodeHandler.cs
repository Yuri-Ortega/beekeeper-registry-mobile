using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Handler
{
    public class HttpStatusCodeHandler
    {
        //GetHttpResponse
        public async Task<HttpResponseMessage> GetHttpResponse(HttpClient httpClient, string requestURI)
        {
            var accessToken = await SecureStorage.GetAsync("access_token");

            if (string.IsNullOrWhiteSpace(accessToken))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            return await httpClient.GetAsync(requestURI);
        }

        //GetHttpResponseWithout Token
        public async Task<HttpResponseMessage> GetHttpResponseWithoutToken(HttpClient httpClient, string requestURI)
        {
            var response = await httpClient.GetAsync(requestURI);

            return response.StatusCode != HttpStatusCode.BadGateway ? response
                : new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
