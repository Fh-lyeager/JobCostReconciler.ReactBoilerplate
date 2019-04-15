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

        public WorkflowService()
        {
            _workflowRepository = new WorkflowRepository();
        }

        public WorkflowService(IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        public Workflow MapWorkflow(DataRow workflowDataRow)
        {
            Workflow workflow = new Workflow();

            workflow.WFlowRID = (int)workflowDataRow["WFlowRID"];
            workflow.WFTempRID = (int)workflowDataRow["WFTempRID"];
            workflow.Name = workflowDataRow["Name"].ToString();
            workflow.RefObjType = workflowDataRow["RefObjType"].ToString();
            workflow.RefObjRID = (int)workflowDataRow["RefObjRID"];
            workflow.Status = workflowDataRow["Status"].ToString();
            workflow.LastUpdated = (DateTime)workflowDataRow["LastUpdated"];
            workflow.CreationDate = (DateTime)workflowDataRow["CreationDate"];
            workflow.RetryCount = (int)workflowDataRow["RetryCount"];

            return workflow;
        }


        public Workflow GetSapphireWorkflow(string jobNumber = "")
        {
            DataTable sapphireWorkflow = GetSapphireWorkflowByJobNumber(jobNumber);

            Workflow workflow = new Workflow();
            if (sapphireWorkflow.AsEnumerable().Any())
            {
                workflow = MapWorkflow(sapphireWorkflow.Rows[0]);
            }

            return workflow;
        }

        public List<Workflow> ListSapphireWorkflows()
        {
            var sapphireWorkflows = GetSapphireWorkflows();

            List<Workflow> workflowList = new List<Workflow>();

            if (!(sapphireWorkflows is null) && sapphireWorkflows.AsEnumerable().Any())
            {
                foreach (var sapphireWorkflow in sapphireWorkflows.AsEnumerable())
                {
                    Workflow workflow = MapWorkflow(sapphireWorkflow);
                    workflowList.Add(workflow);
                }
            }

            return workflowList;
        }

        private DataTable GetSapphireWorkflowByJobNumber(string jobNumber)
        {
            return _workflowRepository.GetSapphireWorkflow(jobNumber);
        }

        private DataTable GetSapphireWorkflows()
        {
            WorkflowRepository workflowRepository = new WorkflowRepository();
            return workflowRepository.GetSapphireWorkflow();
        }
    }
}
