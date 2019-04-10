using JobCostReconciliation.Model;
using System.Data;
using System.Collections.Generic;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IPervasiveService
    {
        void SavePO(POHeader poHeader);
    }
}
