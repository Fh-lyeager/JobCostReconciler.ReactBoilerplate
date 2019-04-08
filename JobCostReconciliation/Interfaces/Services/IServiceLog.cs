using System;
using System.Collections.Generic;
using System.Data;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IServiceLog
    {
        void AppendLog(string Message = "", string Filename = "", Exception Ex = null);
        void AppendLogMessage(string Message);
        //void LogMessage(string Message = "", string Filename = "", Exception Ex = null);
        string WriteToFile(DataTable dataTable, string Filename = "");
        string WriteToFile(List<Reconciliation> reconciliationRecords, string Filename = "");
        void InsertLog(int workflowRID, string sqlOperation, string sqlQuery, string status);
    }
}
