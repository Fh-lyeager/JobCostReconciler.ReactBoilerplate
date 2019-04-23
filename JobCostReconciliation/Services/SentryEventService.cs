using System;
using System.Configuration;
using SharpRaven;
using SharpRaven.Data;
using JobCostReconciliation.Interfaces.Services;

namespace JobCostReconciliation.Services
{
    public class SentryEventService : ISentryEventService
    {
        private readonly RavenClient _ravenClient;

        public SentryEventService()
        {
            _ravenClient = new RavenClient(ConfigurationManager.AppSettings["Sentry"]);
        }

        public void LogError(Exception exception)
        {
            _ravenClient.Capture(new SentryEvent(exception));
        }

        /*  USAGE
        *  dynamic eventObj = new ExpandoObject();
        *  eventObj.SapphireRecordsLoaded = nextBatchToProcess.Count;
        *  _sentryEventService.LogCustomInfoEvent($"Records written to queue for LastRun {mostRecentNextRun.RunComplete}", eventObj);
       */
        public void LogCustomInfoEvent(string messageForEvent, dynamic eventObj)
        {
            _ravenClient.Capture(new SentryEvent(messageForEvent)
            {
                Level = ErrorLevel.Info,
                Extra = eventObj
            });
        }
    }
}
