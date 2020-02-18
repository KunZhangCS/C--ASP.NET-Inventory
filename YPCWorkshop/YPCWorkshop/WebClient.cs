using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace YPCWorkshop
{
    public class WebClient
    {
        public static HttpClient ApiClient = new HttpClient();

        static WebClient()
        {
            ApiClient.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["WebApiUrl"]);
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}