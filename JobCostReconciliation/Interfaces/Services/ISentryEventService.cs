using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface ISentryEventService
    {
        void LogError(Exception exception);
        void LogCustomInfoEvent(string messageForEvent, dynamic eventObj);
    }
}
