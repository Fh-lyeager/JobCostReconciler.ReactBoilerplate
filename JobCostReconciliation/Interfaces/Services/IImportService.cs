using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IImportService
    {
        void ImportPOSyncData();
        void ImportPOSyncData(bool truncate = false);
    }
}
