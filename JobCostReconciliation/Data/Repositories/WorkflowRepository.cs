using System.Configuration;
using System.Data;
using JobCostReconciliation.Interfaces.Clients;
using JobCostReconciliation.Interfaces.Repositories;

namespace JobCostReconciliation.Data.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly ISqlClient _sqlClient;

        public WorkflowRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public DataTable GetSapphireWorkflow(string jobNumber = "")
        {
            string sql = GetSapphireWorkflowQueryString(jobNumber);
            var connString = ConfigurationManager.ConnectionStrings["SapphireDbContext"].ConnectionString;
            DataTable dataTable = _sqlClient.QueryADO(sql, connString);

            if (dataTable == null || dataTable.Rows.Count == 0) return null;
            return dataTable;
        }

        private string GetSapphireWorkflowQueryString(string jobNumber = "")
        {
            string select = string.Empty;
            string where = string.Empty;

            if (!string.IsNullOrEmpty(jobNumber))
            {
                select = " TOP 1 ";
                where = "AND Jobs.JobID LIKE '" + jobNumber + "%' ";
            }
            else
            {
                where = "AND Workflow.Status = 'Completed' ";
            }

            return "SELECT" + select + " Workflow.* " +
                   "FROM WFlows Workflow " +
                   "     INNER JOIN Jobs ON Workflow.RefObjRID = Jobs.HomeRID " +
                   "WHERE " +
                   "     Workflow.[Name] = 'Home Estimate Status Approved' " +
                   where +
                   "ORDER BY CreationDate DESC";
        }
        
    }
}
