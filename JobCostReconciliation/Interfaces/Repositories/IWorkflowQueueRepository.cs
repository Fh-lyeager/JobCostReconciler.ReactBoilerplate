using JobCostReconciliation.Model;
using JobCostReconciliation.Model.Enums;
using System.Collections.Generic;
using System.Linq;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IWorkflowQueueRepository
    {
        List<WorkflowQueue> List();
        List<WorkflowQueue> List(QueueItemStatusType queueItemStatusType);
    }
}
