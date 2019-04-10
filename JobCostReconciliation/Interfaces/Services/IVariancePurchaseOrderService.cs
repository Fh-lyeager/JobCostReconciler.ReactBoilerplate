using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IVariancePurchaseOrderService
    {
        void ReconcileJobVPOApprovalDates(string dataObject = "JobVPOsBySapphirePONumber");
        void ReconcileJobVPOApprovedAndCompletedDates(string dataObject = "JobVPOsBySapphirePONumber");
    }

}
