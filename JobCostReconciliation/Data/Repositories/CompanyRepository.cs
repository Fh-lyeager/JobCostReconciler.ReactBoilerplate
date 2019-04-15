using System;
using System.Data;
using JobCostReconciliation.Data.Clients;
using JobCostReconciliation.Interfaces.Repositories;

namespace JobCostReconciliation.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        public string GetCompanyByJob(string jobNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(jobNumber)) return null;

                var sql = "exec CS_getCompanyByJob('" + jobNumber + "');";
                PervasiveClient pervasiveClient = new PervasiveClient();
                DataTable jobCompanyData = pervasiveClient.QueryPervasiveADO(sql);

                if (jobCompanyData is null || jobCompanyData.Rows.Count == 0) { return null; }
                return jobCompanyData.Rows[0]["CompanyName"].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}