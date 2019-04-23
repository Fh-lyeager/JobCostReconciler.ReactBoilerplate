using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using JobCostReconciliation.Services;
using JobCostReconciliation.Model;
using JobCostReconciliation.Model.Enums;

namespace JobCostReconciliation.UI.Controllers
{
    [Route("admin/api/[controller]")]
    public class WorkflowQueueController : ControllerBase
    {
        // GET: api/WorkflowQueue
        [HttpGet]
        public IEnumerable<WorkflowQueue> Get()
        {
            WorkflowQueueService _workflowQueueService = new WorkflowQueueService();
            List<WorkflowQueue> list = _workflowQueueService.List();

            return Enumerable.Range(1, list.Count - 1).Select(i => new WorkflowQueue
            {
                Id = i,
                RequestBody = list[i].RequestBody,
                UtcDateTime = list[i].UtcDateTime,
                Response = list[i].Response,
                Status = list[i].Status
            });
        }

        // GET: api/WorkflowQueue?status=Error
        [HttpGet]
        [Route("{status}")]
        public IEnumerable<WorkflowQueue> Get(string status)
        {
            if (!(Enum.TryParse(status, out QueueItemStatusType queueItemStatusType)))
            {
                return null;
            }

            WorkflowQueueService _workflowQueueService = new WorkflowQueueService();
            List<WorkflowQueue> list = _workflowQueueService.List(queueItemStatusType);

            return Enumerable.Range(1, list.Count - 1).Select(i => new WorkflowQueue
            {
                Id = i,
                RequestBody = list[i].RequestBody,
                UtcDateTime = list[i].UtcDateTime,
                Response = list[i].Response,
                Status = list[i].Status
            });
        }
    }
}
