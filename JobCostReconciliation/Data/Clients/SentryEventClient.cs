using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentry;
using JobCostReconciliation.Interfaces.Clients;

namespace JobCostReconciliation.Data.Clients
{
    public class SentryEventClient : ISentryEventClient
    {
        private readonly SentryClient _sentryClient;

        public string Environment { get; set; } = SetEnvironment();

        public SentryEventClient()
        {
            var dsn = new Dsn(ConfigurationManager.AppSettings["SentryDsn"]);

            SentryOptions sentryOptions = new SentryOptions
            {
                Dsn = dsn,
                Environment = this.Environment
            };

            _sentryClient = new SentryClient(sentryOptions);
        }

        public void LogError(Exception exception)
        {
            _sentryClient.CaptureException(exception);
        }

        public void LogMessage(string message, Sentry.Protocol.SentryLevel level = Sentry.Protocol.SentryLevel.Info)
        {
            _sentryClient.CaptureMessage(message, level);
        }

        public static string SetEnvironment()
        {
            var MachineName = System.Environment.MachineName;
            if (MachineName.ToString().ToUpper() == ConfigurationManager.AppSettings["ProductionMachineName"].ToString().ToUpper())
            {
                return "PRODUCTION";
            }

            if (MachineName.ToString().ToUpper() == ConfigurationManager.AppSettings["StagingMachineName"].ToString().ToUpper())
            {
                return "STAGING";
            }

            return "DEVELOPMENT";
        }
    }
}
