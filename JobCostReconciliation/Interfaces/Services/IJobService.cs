using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IJobService
    {
        string FormatJobNumber(string jobNumber);
        string GetJobNumberByHomeRID(int homeRID);
    }
}
