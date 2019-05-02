using System;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Sentry;
using JobCostReconciliation.Interfaces.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JobCostReconciliation.Services
{
    public class SentryEventService : ISentryEventService
    {
        private readonly SentryClient _sentryClient;
        //private readonly IHttpHandler _httpHandler;

        private readonly HttpClient _httpClient;

        public SentryEventService()
        {
            _httpClient = new HttpClient();
            _sentryClient = new SentryClient(ConfigureSentryOptions());

        }

        public void LogError(Exception exception)
        {
            _sentryClient.CaptureException(exception);
        }

        public void LogMessage(string message, Sentry.Protocol.SentryLevel level = Sentry.Protocol.SentryLevel.Info)
        {
            _sentryClient.CaptureMessage(message, level);
        }

        public SentryOptions ConfigureSentryOptions()
        {
            try
            {
                var dsn = "https://d6c4cc26a4a14211bde1206019bca81f@sentry.io/1444156";

                SentryOptions sentryOptions = new SentryOptions
                {
                    Dsn = new Dsn(dsn.ToString())
                };
                return sentryOptions;
            }
            catch (Exception exception)
            {
                return new SentryOptions();
            }
        }

        public async Task<String> GetEvents()
        {
            //string url = "https://sentry.io/api/0/projects/fischer-homes/sapphire-workflow-processor/events/?sentry_key=d6c4cc26a4a14211bde1206019bca81f";
            string url = "https://sentry.io/api/0/projects/fischer-homes/sapphire-workflow-processor/events/";
            string auth = "Bearer e2c2124126af4c28970c75a54f6975159adaf54e9971416ea2434292c53f1180";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", auth);
            HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);

            var content = response.Content.ReadAsStringAsync().Result;

            return content;
        }

        
    }
}
