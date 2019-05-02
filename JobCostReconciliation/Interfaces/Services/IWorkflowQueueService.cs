using System.Collections.Generic;
using System.Linq;
using JobCostReconciliation.Model;
using JobCostReconciliation.Model.Enums;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IWorkflowQueueService
    {
        List<WorkflowQueue> List();
        List<WorkflowQueue> List(QueueItemStatusType queueItemStatusType);
    }
}
