using System.Collections.Generic;
using System.Linq;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Data.Repositories;
using JobCostReconciliation.Model;
using JobCostReconciliation.Model.Enums;

namespace JobCostReconciliation.Services
{
    public class WorkflowQueueService : IWorkflowQueueService
    {
        private readonly IWorkflowQueueRepository _workflowQueueRepository;

        public WorkflowQueueService()
        {
            _workflowQueueRepository = new WorkflowQueueRepository();
        }

        public WorkflowQueueService(IWorkflowQueueRepository workflowQueueRepository)
        {
            _workflowQueueRepository = workflowQueueRepository;
        }

        public List<WorkflowQueue> List()
        {
            return _workflowQueueRepository.List();
        }

        public List<WorkflowQueue> List(QueueItemStatusType queueItemStatusType)
        {
            return _workflowQueueRepository.List(queueItemStatusType);
        }
    }
}
