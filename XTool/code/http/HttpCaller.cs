using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public static class HttpCaller
    {
        public static ApiCall Execute(ApiRequest request)
        {
            ApiCall call = new ApiCall() { Request = request, Response = new ApiResponse(),RanAt = DateTime.Now , Id = Guid.NewGuid()};
            call.Key = String.Format("{0} {1}", request.Url, call.RanAt.ToString("HH:mm"));
            string url = call.Request.BaseUrl;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(call.Request.BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "special agent");
                HttpResponseMessage response = client.GetAsync(call.Request.Url, HttpCompletionOption.ResponseContentRead).Result;

                call.Response.ResponseCode = response.StatusCode;
                if (!response.IsSuccessStatusCode)
                {
                    call.Response.ResponseText = response.ReasonPhrase;
                                       
                }
                else
                {
                    call.Response.ResponseBody = response.Content.ReadAsStringAsync().Result;
                }

            }
            return call;
        }


    }
}
