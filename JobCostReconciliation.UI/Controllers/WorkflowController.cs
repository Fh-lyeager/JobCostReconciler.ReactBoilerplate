using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using JobCostReconciliation.Services;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.UI.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class WorkflowController : ControllerBase
    {
        // GET: api/Workflow
        [HttpGet]
        public List<Workflow> Get()
        {
            try
            {
                WorkflowService _workflowService = new WorkflowService();
                return _workflowService.ListWorkflows();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/Workflow/5
        [HttpGet("{jobNumber}", Name = "Get")]
        public Workflow Get(string jobNumber)
        {
            try
            {
                WorkflowService _workflowService = new WorkflowService();
                return _workflowService.GetWorkflow(jobNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
