using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Model;
using JobCostReconciliation.Data.Repositories;

namespace JobCostReconciliation.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IJobRepository _jobRepository;

        public WorkflowService()
        {
            _workflowRepository = new WorkflowRepository();
            _jobRepository = new JobRepository();
        }

        public WorkflowService(IWorkflowRepository workflowRepository, IJobRepository jobRepository)
        {
            _workflowRepository = workflowRepository;
            _jobRepository = jobRepository;
        }

        //public Workflow MapWorkflow(DataRow workflowDataRow)
        //{
        //    Workflow workflow = new Workflow();

        //    workflow.WFlowRID = (int)workflowDataRow["WFlowRID"];
        //    workflow.WFTempRID = (int)workflowDataRow["WFTempRID"];
        //    workflow.Name = workflowDataRow["Name"].ToString();
        //    workflow.RefObjType = workflowDataRow["RefObjType"].ToString();
        //    workflow.RefObjRID = (int)workflowDataRow["RefObjRID"];
        //    workflow.Status = workflowDataRow["Status"].ToString();
        //    workflow.LastUpdated = (DateTime)workflowDataRow["LastUpdated"];
        //    workflow.CreationDate = (DateTime)workflowDataRow["CreationDate"];
        //    workflow.RetryCount = (int)workflowDataRow["RetryCount"];

        //    return workflow;
        //}


        public Workflow GetWorkflow(string jobNumber = "")
        {
            return GetSapphireWorkflowByJobNumber(jobNumber);
        }

        public List<Workflow> ListWorkflows()
        {
            return _workflowRepository.GetSapphireWorkflow();
        }

        private Workflow GetSapphireWorkflowByJobNumber(string jobNumber)
        {
            // get homeRID for this jobNumber
            int homeRID = _jobRepository.GetHomeRIDByJobNumber(jobNumber);
            return _workflowRepository.GetSapphireWorkflow(homeRID).Single();
        }
    }
}
