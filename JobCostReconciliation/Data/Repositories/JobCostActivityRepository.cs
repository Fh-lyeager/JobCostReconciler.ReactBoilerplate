using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using JobCostReconciliation.Data.Clients;
using JobCostReconciliation.Interfaces.Clients;
using JobCostReconciliation.Interfaces.Services;
using Pervasive.Data.SqlClient;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public class JobCostActivityRepository : IJobCostActivityRepository
    {
        private readonly IServiceLog _serviceLog;
        private readonly IPervasiveClient _pervasiveClient;
        private readonly ISqlClient _sqlClient;

        public JobCostActivityRepository()
        {

        }

        public JobCostActivityRepository(IServiceLog serviceLog, IPervasiveClient pervasiveClient, ISqlClient sqlClient)
        {
            _serviceLog = serviceLog;
            _pervasiveClient = pervasiveClient;
            _sqlClient = sqlClient;
        }

        public string CreateJobCostSQLInsert(DataRow row, string company = "", string activity = "")
        {
            return "INSERT INTO \"" + company + "\".PO_HEADER" +
                   "(po_no, NewJobNumber, job_no, activity, po_type, egm_amount, vpo_yes_no, vendor_id, " +
                   "UserID, LastModifiedDate, LastModifiedTime, SapphireObjID, SapphireObjRID, CreatedDate, CreatedBy) " +
                   "VALUES (" +
                   "0, " +
                   "'" + row["NewJobNumber"].ToString() + "', " +
                   "'" + row["job_no"].ToString() + "', " +
                   "'" + activity + "', " +
                   "'N', " +
                   "0, " +
                   "0, " +
                   "'" + row["Vendor_ID"].ToString() + "', " +
                   "'" + "DI-Reconciliation" + "', " +
                   " CURDATE(), " +
                   " CURTIME(), " +
                   "'" + row["SapphireObjID"].ToString() + "', " +
                   "" + (int)row["SapphireObjRID"] + "," +
                   " CURDATE(), " +
                   "'" + "DI-Reconciliation" + "');";
        }

        public string CreateJobCostEGMAmountsUpdateSQL(DataRow row, string company = "")
        {
            try
            {
                return "UPDATE \"" + company + "\".PO_HEADER" +
                      " SET " +
                      "     egm_amount = " + row["sap_egm_amt"] + ", " +
                      "     UserID = 'DI-Reconciliation', " +
                      "     LastModifiedDate = CURDATE(), " +
                      "     LastModifiedTime = CURTIME() " +
                      " WHERE" +
                      " job_no = '" + row["JobID"].ToString().Substring(0, 12) + "'" +
                      " AND activity = " + row["ActID"] +
                      ";";
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog("Error creating POProcessingDatesUpdate SQL.", "", ex);
                return null;
            }
        }

        public void InsertJobCstActsRecord(string query, string jobNumber, string activity, int workflowRID = 0)
        {
            try
            {
                _pervasiveClient.NonQueryPervasiveADO(query);
                _serviceLog.InsertLog(workflowRID, "insert job cost activity", query, "success");
            }
            catch (PsqlException pSqlException)
            {
                _serviceLog.InsertLog(workflowRID, "insert job cost activity", query, $"error: {pSqlException.Message}");
                _serviceLog.AppendLog($" failure inserting job cost activity for job {jobNumber} activity {activity}");
                throw new Exception($"Error inserting Job Cost Activity records. {pSqlException.InnerException}");
            }
            catch (Exception exception)
            {
                _serviceLog.InsertLog(workflowRID, "insert job cost activity", query, $"error: {exception.Message}");
                _serviceLog.AppendLog($" failure inserting job cost activity for job {jobNumber} activity {activity}");
                throw new Exception($"Error inserting Job Cost Activity records. {exception.InnerException}");
            }
        }

        public void CreateJobCstActsRecord(DataRow row, string company = "", int workflowRID = 0, string activity = "")
        {
            string actID = row["activity"].ToString();

            if (!String.IsNullOrEmpty(activity))
            {
                actID = activity;
            }

            string sql = CreateJobCostSQLInsert(row, company, actID);
            InsertJobCstActsRecord(sql, row["job_no"].ToString(), actID, workflowRID);
        }
        
        public string CreateResetEgmAmountSQL(string jobNumber, string company = "", int workflowRID = 0, string activity = "")
        {
            string activitySql = "";

            if (!String.IsNullOrEmpty(activity))
            {
                activitySql = " AND activity = " + activity ;
            }

            return "UPDATE \"" + company + "\".PO_HEADER" +
                   " SET egm_amount = 0, " +
                   "     UserID = 'DI-Reconciliation', " +
                   "     LastModifiedDate = CURDATE(), " +
                   "     LastModifiedTime = CURTIME() " +
                   " WHERE job_no = '" + jobNumber + "'" +
                   activitySql +
                   " AND vpo_yes_no = 0";
        }

        public void UpdateEgmAmountsToZero(string sql, int workflowRID = 0)
        {
            try
            {
                _pervasiveClient.NonQueryPervasiveADO(sql);
                _serviceLog.InsertLog(workflowRID, "update egm to 0", sql, "success");
                
            }
            catch (PsqlException pSqlEx)
            {
                _serviceLog.InsertLog(workflowRID, "update egm to 0", sql, $"error: {pSqlEx.Message}");
                _serviceLog.AppendLog($" Failure setting EGM amounts to 0", "", pSqlEx);
            }
            catch (Exception ex)
            {
                _serviceLog.InsertLog(workflowRID, "update egm to 0", sql, $"error: {ex.Message}");
                _serviceLog.AppendLog($" Failure setting EGM amounts to 0", "", ex);
            }
        }

        public void SetEgmAmounts(string jobNumber, int activity, decimal sapEgmAmount, string company = "", int workflowRID = 0)
        {
            string sql = CreateSetEgmAmountsSQL(jobNumber, activity, sapEgmAmount, company);
            try
            {
                _pervasiveClient.NonQueryPervasiveADO(sql);
                _serviceLog.InsertLog(workflowRID, "update egm amounts", sql, "success");
            }
            catch (PsqlException pSqlEx)
            {
                _serviceLog.InsertLog(workflowRID, "update egm amounts", sql, $"error: {pSqlEx.Message}");
                _serviceLog.AppendLog($" Failure setting EGM values for job {jobNumber} activity {activity} with amount {sapEgmAmount}", "", pSqlEx);
            }
            catch (Exception ex)
            {
                _serviceLog.InsertLog(workflowRID, "update egm amounts", sql, $"error: {ex.Message}");
                _serviceLog.AppendLog($" Failure setting EGM values for job {jobNumber} activity {activity} with amount {sapEgmAmount}", "", ex);
            }
        }

        private string CreateSetEgmAmountsSQL(string jobNumber, int activity, decimal sapphireEgmAmount, string company = "", string updateUserId = "DI-Reconciliation")
        {
            return "UPDATE \"" + company + "\".PO_HEADER " +
                   "SET egm_amount = " + sapphireEgmAmount + ", " +
                   "    LastModifiedDate = CURDATE()," +
                   "    LastModifiedTime = CURTIME(), " +
                   "    UserID = '" + updateUserId + "' " +
                   "WHERE POHeaderRecordID =    (" +
                                                    "SELECT TOP 1 POHeaderRecordID " +
                                                    " FROM \"" + company + "\".PO_HEADER" +
                                                    " WHERE job_no = '" + jobNumber + "'" +
                                                        " AND activity = " + activity +
                                                        " AND vpo_yes_no = 0" +
                                                    " ORDER BY POHeaderRecordID ASC" +
                                               ")";
        }

        public void RecalculateEgmValues(string jobNumber, string company = "", int workflowRID = 0)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "dbName", company },
                    { "job_no", jobNumber }
                };

                var storedProc = "IT_RecalcEGMValues";

                _serviceLog.AppendLog($" recalc egm values IT_RecalcEGMValues company: {company} jobNumber: {jobNumber}", "", null);
                _pervasiveClient.StoredProcADO(storedProc, parameters);
                _serviceLog.InsertLog(workflowRID, "recalc egm values", $"{storedProc} company: {company} jobNumber: {jobNumber}", "success");
            }
            catch (Exception ex)
            {
                _serviceLog.InsertLog(workflowRID, "recalc egm values", $"IT_RecalcEGMValues company: {company} jobNumber: {jobNumber}", $"error: {ex.Message}");
                _serviceLog.AppendLog($" Failure during recalc egm values IT_RecalcEGMValues company: {company} jobNumber: {jobNumber} error: {ex.Message}", "", ex);
            }
        }

        public DataTable GetPervasiveJobCstActs(string jobNumber, string company = "")
        {
            try
            {
                string sql = GetJobCstActsByJobSql(jobNumber, company);

                //_serviceLog.AppendLog($" Getting pervasive JobCstActs for job {jobNumber}", "", null);
                DataTable pervasiveJobCstActs = _pervasiveClient.QueryPervasiveADO(sql);

                // if no records returned from po_header
                if (pervasiveJobCstActs is null || !(pervasiveJobCstActs.AsEnumerable().Any()))
                {
                    // this should not happen, and usually will only happen if querying an invalid job number or sapphire query connection and pervasive connection environments do not match

                    // in rare cases, this is a correct result, meaning no po_header records exist in pervasive - usually caused by an error record in PO DI preventing processing
                    //  if this scenario is encountered, a notification should be generated immediately so the PO DI queue can be checked and error records can be resolved
                    //_serviceLog.AppendLog($" Failure getting pervasive JobCstActs for job {jobNumber} company {company}", "", null);
                    throw new Exception($"Workflow failure: Failure getting pervasive job cost records for job {jobNumber}");
                }

                //_serviceLog.AppendLog($" {pervasiveJobCstActs.Rows.Count} Pervasive JobCstActs records found");

                return pervasiveJobCstActs;
            }
            catch (PsqlException pSqlException)
            {
                _serviceLog.AppendLog($" Failure getting pervasive JobCstActs for job {jobNumber} company {company}", "", pSqlException);
                return null;
            }
        }

        private string GetJobCstActsByJobSql(string jobNumber, string company = "")
        {
            return "SELECT " +
                        " SapphireObjRID," +
                        " po_no," +
                        " NewJobNumber," +
                        " activity," +
                        " egm_amount AS p_egm_amount," +
                        " po_type," +
                        " SapphireObjID," +
                        " job_no," +
                        " cancelled_date," +
                        " CONCAT(CONCAT(job_no, '-'), activity) AS JobCstReconcilerID," +
                        " IF (SapphireObjRID > 0, CONCAT(CONCAT(CONCAT(CONCAT(SapphireObjRID, '-'), job_no), '-'), activity), CONCAT(CONCAT(job_no, '-'), activity)) AS JobPOReconcilerID," +
                        " CONCAT(CONCAT(CONCAT(CONCAT(job_no, '-'), activity),'/'),egm_amount) AS EGMReconcilerID," +
                        " POHeaderRecordID," +
                        " Vendor_ID, " +
                        " Release_Date, " +
                        " Approval_Date, " +
                        " Payment_Date, " +
                        " Required_Date, " +
                        " Check_Date, " +
                        " Payment_Amount, " +
                        " Subtotal, " +
                        " Tax, " +
                        " Total, " +
                        " check_no," +
                        " VPO_Yes_No, " +
                        " original_po_no, " +
                        " LastModifiedDate," +
                        " Community, " +
                        " Taxable_Amount, " +
                        " POEffectiveDate, " +
                        " eSubmittalDate, " +
                        " ApprovePaymentDate, " +
                        " eXmitDate, " +
                        " SapphirePONumber" +
                    " FROM \"" + company + "\".PO_HEADER" +
                    " WHERE vpo_yes_no = 0 " +
                        " AND job_no = '" + jobNumber + "'" +
                    " ORDER BY job_no ASC, activity ASC, po_no ASC";
        }

        public DataTable GetEgmJobTotals(List<int> homeRIds)
        {
            string sql = GetEgmJobTotalsSql(homeRIds);
            string connection = ConfigurationManager.ConnectionStrings["SapphireDbContext"].ConnectionString.ToString();

            SqlClient sqlClient = new SqlClient();
            DataTable sapphireEgmTotals = sqlClient.QueryADO(sql, connection);

            // if no records returned from sapphire
            if (sapphireEgmTotals is null || !(sapphireEgmTotals.AsEnumerable().Any()))
            {
                return null;
            }
            return sapphireEgmTotals;
        }

        private string GetEgmJobTotalsSql(List<int> homeRIds)
        {
            var list = (string.Join(",", homeRIds.Distinct().Select(x => x.ToString()).ToArray()));

            return "SELECT " +
                   "Homes.HomeRID, " +
                   "JobNumber = LEFT(Jobs.JobID, 12), " +
                   "JobCstActs_BudgetAmt = SUM(JobCstActs.BudgetAmt), " +
                   "EstApprDate = Homes.EstApprDate " +
                   "FROM " +
                   "Homes " +
                   "INNER JOIN Jobs ON Jobs.HomeRID = Homes.HomeRID " +
                   "INNER JOIN JobCstActs ON JobCstActs.JobRID = Jobs.JobRID " +
                   "INNER JOIN JobCstHdrs ON JobCstHdrs.JobCstHdrRID = JobCstActs.JobCstHdrRID " +
                   "WHERE " +
                   "JobCstHdrs.JobCstHdrRID =  (" +
                   "SELECT TOP 1 JobCstHdrRID " +
                   "FROM JobCstHdrs j2 " +
                   "WHERE j2.JobRID = JobCstHdrs.JobRID " +
                   "AND CAST(j2.CreationDate AS DATE) = CAST(Homes.EstApprDate AS DATE) " +
                   "ORDER BY j2.CreationDate DESC " +
                   ") " +
                   "AND CAST(JobCstHdrs.CreationDate AS DATE) = CAST(Homes.EstApprDate AS DATE) " +
                   "AND Homes.HomeRID IN (" + list + ") " +
                   "GROUP BY Homes.HomeRID, Jobs.JobID, Homes.EstApprDate ";
        }
    }
}
