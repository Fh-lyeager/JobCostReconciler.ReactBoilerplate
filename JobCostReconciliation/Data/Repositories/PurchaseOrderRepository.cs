using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Interfaces.Clients;
using JobCostReconciliation.Services;
using JobCostReconciliation.Data.Clients;
using System.Configuration;
using System.Data;
using System;

namespace JobCostReconciliation.Data.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly IPervasiveClient _pervasiveClient;
        private readonly IServiceLog _serviceLog;
        private readonly IDateService _dateService;

        public PurchaseOrderRepository()
        {
            _pervasiveClient = new PervasiveClient();
            _serviceLog = new ServiceLog();
            _dateService = new DateService();
        }

        public PurchaseOrderRepository(IPervasiveClient pervasiveClient, IServiceLog serviceLog, IDateService dateService)
        {
            _pervasiveClient = pervasiveClient;
            _serviceLog = serviceLog;
            _dateService = dateService;
        }


        // ***********************
        // TODO: 
        //  need to turn this INSERT into pSQL Stored Procedure in order to get new po_no at time of insert
        // ***********************
        public string CreateJobPOSQLInsert(DataRow row, string company = "")
        {
            try
            {
                int po_no = int.TryParse(GetPONumber(row["JobID"].ToString(), company), out int result) ? result : 0;

                string sql = "INSERT INTO \"" + company + "\".PO_HEADER" +
                                "(po_no, NewJobNumber, job_no, activity, po_type, egm_amount, vpo_yes_no, vendor_id, " +
                                    "release_date, ReleaseTime, cancelled_date, CancelTime, payment_date, " +
                                    "payment_amount, subtotal, tax, total, taxable_amount, TaxRate, ApprovePaymentDate, " +
                                    "community, product, building, unit, eSubmittalDate, " +
                                    "UserID, LastModifiedDate, LastModifiedTime, " +
                                    "SapphirePONumber, SapphireObjID, SapphireObjRID) " +
                            "VALUES (" + po_no + ", " +
                                    "'" + row["NewJobNumber"].ToString() + "', " +
                                    "'" + row["job_no"].ToString() + "', " +
                                    "'" + row["activity"].ToString() + "', " +
                                    "'" + row["po_type"].ToString() + "', " +
                                    "0, " +
                                    "0, " +
                                    "'" + row["Vendor_ID"].ToString() + "', " +

                                    (_dateService.IsNullSapphireDate(row["DateReleased"]) ? "NULL" : "'" + _dateService.FormatDateForPervasive(row["DateReleased"]) + "'") + ", " +
                                    (_dateService.IsNullSapphireDate(row["DateReleased"]) ? "NULL" : "'" + _dateService.FormatTimeForPervasive(row["DateReleased"]) + "'") + ", " +
                                    (_dateService.IsNullSapphireDate(row["DateCancelled"]) ? "NULL" : "'" + _dateService.FormatDateForPervasive(row["DateCancelled"]) + "'") + ", " +
                                    (_dateService.IsNullSapphireDate(row["DateCancelled"]) ? "NULL" : "'" + _dateService.FormatTimeForPervasive(row["DateCancelled"]) + "'") + ", " +
                                    (_dateService.IsNullSapphireDate(row["DatePaid"]) ? "NULL" : "'" + _dateService.FormatDateForPervasive(row["DatePaid"]) + "'") + ", " +

                                    row["AmtPaid"].ToString() + ", " +
                                    row["AmtSubtotal"].ToString() + ", " +
                                    row["AmtTax"].ToString() + ", " +
                                    row["AmtTotal"].ToString() + ", " +
                                    row["AmtTaxable"].ToString() + ", " +
                                    row["TaxPercentage"].ToString() + ", " +
                                    (_dateService.IsNullSapphireDate(row["DateApproved"]) ? "NULL" : "'" + _dateService.FormatDateForPervasive(row["DateApproved"]) + "'") + ", " +

                                    "'" + row["job_no"].ToString().Substring(0, 3) + "', " +
                                    "'" + row["job_no"].ToString().Substring(3, 2) + "', " +
                                    "'" + row["job_no"].ToString().Substring(5, 3) + "', " +
                                    "'" + row["job_no"].ToString().Substring(8, 4) + "', " +
                                    (_dateService.IsNullSapphireDate(row["DateComplByVnd"]) ? "NULL" : "'" + _dateService.FormatDateForPervasive(row["DateComplByVnd"]) + "'") + ", " +

                                    "'" + "DI-Reconciliation" + "', " +
                                    " CURDATE(), " +
                                    " CURTIME(), " +
                                    "'" + row["SapphirePONumber"].ToString() + "', " +
                                    "'" + row["SapphireObjID"].ToString() + "', " +
                                    "" + (int)row["SapphireObjRID"] + ");";

                return sql;
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);
                return null;
            }
        }

        // Update PO_Header to set po_no and all possible values (except egm) from new JobPO record (passed in as DataRow)
        public string CreateJobPOSQLUpdate(DataRow row, string company = "")
        {
            try
            {
                int po_no = int.TryParse(GetPONumber(row["JobID"].ToString(), company), out int result) ? result : 0;

                string sql = String.Empty;
                sql = "UPDATE \"" + company + "\".PO_HEADER" +
                        " SET " +
                            " po_no = " + po_no + ", " +
                            " vendor_id = '" + row["Vendor_ID"] + "', " +
                            " po_type = 'Y'" + ", " +
                            (_dateService.IsNullSapphireDate(row["DateReleased"]) ? " release_date = NULL" : " release_date = '" + _dateService.FormatDateForPervasive(row["DateReleased"]) + "'") + ", " +
                            (_dateService.IsNullSapphireDate(row["DateReleased"]) ? " ReleaseTime = NULL" : " ReleaseTime = '" + _dateService.FormatTimeForPervasive(row["DateReleased"]) + "'") + ", " +
                            (_dateService.IsNullSapphireDate(row["DatePaid"]) ? " payment_date = NULL" : " payment_date = '" + _dateService.FormatDateForPervasive(row["DatePaid"]) + "'") + ", " +
                            (_dateService.IsNullSapphireDate(row["DateCancelled"]) ? " cancelled_date = NULL" : " cancelled_date '" + _dateService.FormatDateForPervasive(row["DateCancelled"]) + "'") + ", " +
                            (_dateService.IsNullSapphireDate(row["DateCancelled"]) ? " CancelTime = NULL" : " CancelTime '" + _dateService.FormatTimeForPervasive(row["DateCancelled"]) + "'") + ", " +

                            " payment_amount = " + row["AmtPaid"] + ", " +
                            " subtotal = " + row["AmtSubTotal"] + ", " +
                            " tax = " + row["AmtTax"] + ", " +
                            " total = " + row["AmtTotal"] + ", " +
                            " Community = '" + row["job_no"].ToString().Substring(0, 3) + "'" + ", " +
                            " Product = '" + row["job_no"].ToString().Substring(3, 2) + "'" + ", " +
                            " Building = '" + row["job_no"].ToString().Substring(5, 3) + "'" + ", " +
                            " Unit = '" + row["job_no"].ToString().Substring(8, 4) + "'" + ", " +
                            " taxable_amount = " + row["AmtTaxable"] + ", " +
                            (_dateService.IsNullSapphireDate(row["DateApproved"]) ? " ApprovePaymentDate = NULL" : " ApprovePaymentDate = '" + _dateService.FormatDateForPervasive(row["DateApproved"]) + "'") + ", " +
                            " Invoice = '" + row["RefNumber"] + "'" + ", " +
                            " TaxRate = " + row["TaxPercentage"] + ", " +
                            (_dateService.IsNullSapphireDate(row["DateComplByVnd"]) ? " eSubmittalDate = NULL" : " eSubmittalDate = '" + _dateService.FormatDateForPervasive(row["DateComplByVnd"]) + "'") + ", " +
                            " UserID = 'DI-Reconciliation'" + ", " +
                            " LastModifiedDate = CURDATE()" + ", " +
                            " LastModifiedTime = CURTIME()" + ", " +
                            " SapphirePONumber = " + row["SapphirePONumber"] + ", " +
                            " SapphireObjID = 'JobPOs'" + ", " +
                            " SapphireObjRID = " + row["SapphireObjRID"] +
                        " WHERE" +
                                " po_no = 0" +
                            " AND job_no = '" + row["job_no"] + "'" +
                            " AND activity = " + row["activity"] + ";";

                return sql;
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog("Error creating JobPOSQLUpdate statement.", "", ex);
                return null;
            }
        }

        private string GetPONumber(string jobID, string company = "")
        {
            int po_no = 0;

            if (string.IsNullOrEmpty(company.Trim()))
            {
                _serviceLog.AppendLog(String.Format("Unable to GetNewPONumber for Job {0} using company {1}.", jobID, company.ToString()));
                return null;
            }
            else
            {
                // get new po_no from pervasive
                po_no = int.TryParse(GetNewPONumber(company), out int result) ? result : 0;
                if (po_no < 1)
                {
                    _serviceLog.AppendLog(String.Format("Unable to GetNewPONumber for Job {0} using company {1} from list {2}.", jobID, company.ToString(), "jobPORecordsMatchingPervasiveJobCstActRecords"));
                    return null;
                }
            }

            return po_no.ToString();
        }


        public string CreatePOProcessingDatesSQL(DataRow row, string company = "")
        {
            string sql = String.Empty;

            sql = CreatePOHeaderDatesQuery(row, company);
            sql += CreatePOProcessingDatesUpdateSQL(row, company);

            return sql;
        }


        public string CreatePOProcessingDatesUpdateSQL(DataRow row, string company = "")
        {
            try
            {
                return "UPDATE \"" + company + "\".PO_HEADER" +
                        " SET " +
                            (String.IsNullOrEmpty(row["DateComplByVnd"].ToString()) ? " eSubmittalDate = NULL" : " eSubmittalDate = '" + row["DateComplByVnd"] + "'") + ", " +
                            (String.IsNullOrEmpty(row["DateApproved"].ToString()) ? " ApprovePaymentDate = NULL" : " ApprovePaymentDate = '" + row["DateApproved"] + "'") +
                        " WHERE" +
                                " SapphirePONumber = '" + row["JobPOID"] + "';";
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog("Error creating POProcessingDatesUpdate SQL.", "", ex);
                return null;
            }
        }


        private string CreatePOHeaderDatesQuery(DataRow row, string company = "")
        {
            try
            {
                return "SELECT POHeaderRecordID, po_no, NewJobNumber, job_no, activity, vendor_id, po_type, egm_amount, eSubmittalDate, ApprovePaymentDate, " +
                        "vpo_yes_no, UserID, LastModifiedDate, LastModifiedTime, SapphireObjID, SapphireObjRID " +
                      "FROM \"" + company + "\".PO_HEADER " +
                      "WHERE SapphirePONumber = '" + row["SapphirePONumber"] + "';";
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog("Error creating POHeaderDatesQuery SQL.", "", ex);
                return null;
            }
        }

        private string GetNewPONumber(string company)
        {
            try
            {
                var sql = "exec CS_GetPONumber('" + company + "', 'PO');";
                DataTable dt = _pervasiveClient.QueryPervasiveADO(sql);

                if (dt is null || dt.Rows.Count == 0) { return null; }
                if (!int.TryParse(dt.Rows[0]["poNumber"].ToString(), out int result))
                {
                    _serviceLog.AppendLog(String.Format("Unable to GetNewPONumber using company {0}.", company.ToString()));
                    return null;
                }

                return dt.Rows[0]["poNumber"].ToString();
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);
                return null;
            }
        }
    }
}
