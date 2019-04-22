using System;
using System.Collections.Generic;
//using System.Web.Http;
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
        public List<WorkflowQueue> Get()
        {
            WorkflowQueueService _workflowQueueService = new WorkflowQueueService();
            return _workflowQueueService.List();
        }

        // GET: api/WorkflowQueue?status=Error
        [HttpGet]
        [Route("{status}")]
        public List<WorkflowQueue> Get(string status)
        {
            if (!(Enum.TryParse(status, out QueueItemStatusType queueItemStatusType)))
            {
                return null;
            }

            WorkflowQueueService _workflowQueueService = new WorkflowQueueService();
            return _workflowQueueService.List(queueItemStatusType);
        }
    }
}
