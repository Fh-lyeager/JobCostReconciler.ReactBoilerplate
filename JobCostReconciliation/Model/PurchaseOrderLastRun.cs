using System;
using JobCostReconciliation.Model.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Model
{
    public class PurchaseOrderLastRun
    {
        public int NextRunId { get; set; }
        public DateTime RunComplete { get; set; }
        public QueueItemStatusType Status { get; set; }
    }
}
