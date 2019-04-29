using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobCostReconciliation.Data.Clients;
using JobCostReconciliation.Interfaces.Clients;
using JobCostReconciliation.Interfaces.Services;


namespace JobCostReconciliation.Services
{
    public class SentryEventService : ISentryEventService
    {
        private readonly ISentryEventClient _sentryEventClient;

        public SentryEventService()
        {
            _sentryEventClient = new SentryEventClient();
        }

        public DataTable GetEvents()
        {
            DataTable dataTable = new DataTable();

            


            return dataTable;
        }
    }
}
