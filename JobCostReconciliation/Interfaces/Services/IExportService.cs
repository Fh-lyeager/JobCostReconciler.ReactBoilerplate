using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IExportService
    {
        void PrepareDataForExport(DataTable dataTable, string dataObject, string sqlOperation = "INSERT");
        void PrepareDataRowForExport(DataRow row, string dataObject, string sqlOperation = "INSERT");
        void WriteSQLToFile(string sqlText, string dataObject, string sqlOperation = "INSERT");
        string BuildSQLInsert(string DataObject, DataRow row, string Company = "");
        DataTable ConvertToDataTable<T>(IEnumerable<T> data);
    }
}
