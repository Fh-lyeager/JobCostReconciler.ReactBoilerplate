using System.Collections.Generic;
using System.Data;
using System.Linq;
using JobCostReconciliation.Interfaces.Clients;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Data.Contexts;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Data.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly ISqlClient _sqlClient;

        public WorkflowRepository()
        {

        }

        public WorkflowRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public List<Workflow> GetSapphireWorkflow(int homeRID = 0)
        {
            using (var _db = new SapphireDbContext())
            {
                return _db.Workflows
                    .Where(w => w.RefObjType == "Home"
                        && w.Name == "Home Estimate Status Approved"
                        && ((!(homeRID.Equals(0)) && w.RefObjRID.Equals(homeRID))
                        || (homeRID.Equals(0)))
                    ).ToList();
            }
        }

        /*
        public DataTable GetSapphireWorkflow(string jobNumber = "")
        {
            string sql = GetSapphireWorkflowQueryString(jobNumber);
            var connString = ConfigurationManager.ConnectionStrings["SapphireDbContext"].ConnectionString;
            SqlClient sqlClient = new SqlClient();
            DataTable dataTable = sqlClient.QueryADO(sql, connString);

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
        */
    }
}
