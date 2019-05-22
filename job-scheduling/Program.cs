using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace job_scheduling
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var result = ExecuteSchedulingJob();
            result.Wait();
            Console.WriteLine("Goodbye World");
        }

        private static async Task<HttpResponseMessage> ExecuteSchedulingJob()
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>(); // must also define a project guid for secrets in the .cspro – add tag <UserSecretsId> containing a guid
            var Configuration = builder.Build();
            var secretPassword = Configuration["SSG_PASSWORD"];
            var secretUserName = Configuration["SSG_USERNAME"];

            HttpClient httpClient = null;
            try
            {
                string dynamicsOdataUri = "https://wsgw.dev.jag.gov.bc.ca/victim/api/data/v9.0";

                string ssgUsername = secretUserName;
                string ssgPassword = secretPassword;

                httpClient = new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(ssgUsername, ssgPassword) });
                httpClient.BaseAddress = new Uri(string.Join("/", dynamicsOdataUri, "vsd_RunScheduler"));
                httpClient.Timeout = new TimeSpan(1, 0, 0); // 1 hour timeout
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "vsd_RunScheduler");

                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonResult = response.Content.ReadAsStringAsync().Result;
                }

                return response;
            }
            finally
            {
                if (httpClient != null)
                    httpClient.Dispose();
            }
        }

    }
}
