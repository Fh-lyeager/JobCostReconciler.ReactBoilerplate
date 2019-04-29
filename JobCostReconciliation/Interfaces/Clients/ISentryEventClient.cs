using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Interfaces.Clients
{
    public interface ISentryEventClient
    {
        void LogError(Exception exception);
        void LogMessage(string message, Sentry.Protocol.SentryLevel level = Sentry.Protocol.SentryLevel.Info);
    }
}
