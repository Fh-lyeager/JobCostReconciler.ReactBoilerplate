using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
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
        public IEnumerable<WorkflowQueueRequest> Get()
        {
            try
            {
                WorkflowQueueService _workflowQueueService = new WorkflowQueueService();
                List<WorkflowQueue> list = _workflowQueueService.List().AsEnumerable()
                    .OrderByDescending(d => d.Id)
                    .ToList();

                return Enumerable.Range(1, list.Count - 1).Select(i => new WorkflowQueueRequest
                {
                    Id = list[i].Id,
                    RequestBody = list[i].RequestBody,
                    UtcDateTime = list[i].UtcDateTime,
                    Response = list[i].Response,
                    Status = list[i].Status,
                    WFlowRID = JObject.Parse(list[i].RequestBody).ToObject<SapphireWorkflowRequestObject>().WFlowRID,
                    RefObjRID = JObject.Parse(list[i].RequestBody).ToObject<SapphireWorkflowRequestObject>().RefObjRID
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/WorkflowQueue?status=Error
        [HttpGet]
        [Route("{status}")]
        public IEnumerable<WorkflowQueue> Get(string status)
        {
            try
            {
                if (!(Enum.TryParse(status, out QueueItemStatusType queueItemStatusType)))
                {
                    return null;
                }

                WorkflowQueueService _workflowQueueService = new WorkflowQueueService();
                List<WorkflowQueue> list = _workflowQueueService.List(queueItemStatusType);

                return Enumerable.Range(1, list.Count - 1).Select(i => new WorkflowQueue
                {
                    Id = list[i].Id,
                    RequestBody = list[i].RequestBody,
                    UtcDateTime = list[i].UtcDateTime,
                    Response = list[i].Response,
                    Status = list[i].Status
                });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
