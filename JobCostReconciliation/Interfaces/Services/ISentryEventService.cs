using System;
using System.Data;
using System.Threading.Tasks;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface ISentryEventService
    {
        Task<String> GetEvents();
    }
}
