using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;

using JobCostReconciliation.Data.Clients;
using System.Configuration;
using System.Data;
using System;
using JobCostReconciliation.Interfaces.Clients;

namespace JobCostReconciliation.Data.Repositories
{
    public class SapphireRepository : ISapphireRepository
    {
        private readonly ISqlClient _sqlClient;
        //private readonly IServiceLog _serviceLog;

        public SapphireRepository()
        {

        }

        public SapphireRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public DataTable GetSapphireRecords(string dataObject, string jobNumber = "")
        {
            var sapphireQuery = string.Empty;
            DataTable sapphireRecords = new DataTable();
            
            if (dataObject.Equals("JobCstActs")) { sapphireQuery = GetSapphireJobCostQueryString(jobNumber); }
            if (dataObject.Equals("JobCstActs_EGM")) { sapphireQuery = GetSapphireEgmTotalsSql(jobNumber); }
            if (dataObject.Equals("JobPOs")) { sapphireQuery = GetSapphirePurchaseOrderQueryString(); }
            if (dataObject.Equals("JobVPOs")) { sapphireQuery = GetSapphireVariancePurchaseOrderQueryString(); }
            
            if (!string.IsNullOrEmpty(sapphireQuery))
            {
                var connString = ConfigurationManager.ConnectionStrings["SapphireDbContext"].ConnectionString;

                SqlClient sqlClient = new SqlClient();

                //_serviceLog.AppendLog(String.Format(" Getting Sapphire {0} records", dataObject));
                sapphireRecords = sqlClient.QueryADO(sapphireQuery, connString);
                //_serviceLog.AppendLog(String.Format(" {0} Sapphire {1} records found", sapphireRecords.Rows.Count, dataObject));
            }

            if (sapphireRecords == null || sapphireRecords.Rows.Count == 0) return null;
            return sapphireRecords;
        }
        
        private string GetSapphireJobCostQueryString(string jobNumber = "")
        {
            string jobNumberSql = string.Empty;
            if (!String.IsNullOrEmpty(jobNumber))
            {
                jobNumberSql = " AND Jobs.JobID LIKE '" + jobNumber + "%'";
            }

            return "SELECT " +
                        " JobRID = Jobs.JobRID," +
                        " job_no = LEFT(Jobs.JobID, 12)," +
                        " NewJobNumber = LEFT(Jobs.JobID, 5) + '/' + SUBSTRING(Jobs.JobID, 6, 3) + '/' + SUBSTRING(Jobs.JobID, 9, 4)," +
                        " activity = Acts.ActID," +
                        " sap_egm_amount = JobCstActs.BudgetAmt," +
                        " Vendor_ID = COALESCE(Vnds.VndID, 'YNH00')," +
                        " SapphireObjID = 'JobCstActs'," +
                        " SapphireObjRID = JobCstActs.JobCstActRID," +
                        " JobCstReconcilerID = LEFT(Jobs.JobID, 12) + '-' + Acts.ActID," +
                        " dbName = Lots.SellerName" +
                    " FROM JobCstActs " +
                        " INNER JOIN JobCstHdrs ON JobCstActs.JobCstHdrRID = JobCstHdrs.JobCstHdrRID" +
                        " INNER JOIN Jobs ON JobCstActs.JobRID = Jobs.JobRID" +
                        " INNER JOIN Acts ON JobCstActs.ActRID = Acts.ActRID" +
                        " INNER JOIN Lots ON Jobs.LotRID = Lots.LotRID" +
                        " INNER JOIN Communities ON Lots.CommunityRID = Communities.CommunityRID" +
                        " INNER JOIN Homes ON Homes.HomeRID = Jobs.HomeRID" +
                        " LEFT JOIN JobActs ON JobActs.JobRID = Jobs.JobRID" +
                            " AND JobActs.ActRID = Acts.ActRID" +
                        " LEFT JOIN JobSchActs ON JobSchActs.JobActRID = JobActs.JobActRID" +
                            " AND JobSchActs.ActRID = Acts.ActRID" +
                        " LEFT JOIN Vnds ON Vnds.VndRID = JobSchActs.VndRID" + 
                    " WHERE " +
                            " Communities.ShortID != 'TEST'" +
                        " AND JobCstHdrs.JobCstHdrRID =   (" +
                                                            " SELECT TOP 1 JobCstHdrRID" +
                                                            " FROM JobCstHdrs j2" +
                                                            " WHERE j2.JobRID = JobCstHdrs.JobRID" +
                                                                " AND CAST(j2.CreationDate AS DATE) = CAST(Homes.EstApprDate AS DATE)" +
                                                            " ORDER BY j2.CreationDate DESC" +
                                                        " )" +
                        " AND CAST(JobCstHdrs.CreationDate AS DATE) = CAST(Homes.EstApprDate AS DATE)" +
                        " AND Lots.sellerName IS NOT NULL AND Lots.sellerName != ''" +
                        jobNumberSql +
                    " ORDER BY NewJobNumber ASC, activity ASC;";
        }

        private string GetSapphireEgmTotalsSql(string jobNumber = "")
        {
            string jobNumberSql = string.Empty;
            if (!String.IsNullOrEmpty(jobNumber))
            {
                jobNumberSql = " AND Jobs.JobID LIKE '" + jobNumber + "%'";
            }

            return "SELECT " +
                        " HomeRID = Homes.HomeRID," +
                        " JobID = Jobs.JobID," +
                        " JobNumber = LEFT(Jobs.JobID, 12)," +
                        " NewJobNumber = LEFT(Jobs.JobID, 5) + '/' + SUBSTRING(Jobs.JobID, 6, 3) + '/' + SUBSTRING(Jobs.JobID, 9, 4)," +
                        " JobCstActs_BudgetAmt = SUM(JobCstActs.BudgetAmt)," +
                        " dbName = Lots.SellerName, " +
                        " CAST(Homes.EstApprDate AS DATE) AS EstApprDate" +
                    " FROM Homes " +
                        " INNER JOIN Jobs ON Jobs.HomeRID = Homes.HomeRID" +
                        " INNER JOIN JobCstActs ON JobCstActs.JobRID = Jobs.JobRID" +
                        " INNER JOIN JobCstHdrs ON JobCstHdrs.JobCstHdrRID = JobCstActs.JobCstHdrRID" +
                        " INNER JOIN Lots ON Jobs.LotRID = Lots.LotRID" +
                        " INNER JOIN Communities ON Lots.CommunityRID = Communities.CommunityRID" +
                    " WHERE " +
                            " Communities.ShortID != 'TEST'" +
                        " AND JobCstHdrs.JobCstHdrRID =  (" +
                                                            " SELECT TOP 1 JobCstHdrRID" +
                                                            " FROM JobCstHdrs j2" +
                                                            " WHERE j2.JobRID = JobCstHdrs.JobRID" +
                                                                " AND CAST(j2.CreationDate AS DATE) = CAST(Homes.EstApprDate AS DATE)" +
                                                            " ORDER BY j2.CreationDate DESC" +
                                                        " )" +
                        " AND CAST(JobCstHdrs.CreationDate AS DATE) = CAST(Homes.EstApprDate AS DATE)" +
                        " AND Lots.sellerName IS NOT NULL AND Lots.sellerName != ''" +
                        jobNumberSql +
                    " GROUP BY Homes.HomeRID, Jobs.JobID, Lots.SellerName, Homes.EstApprDate" +
                    " ORDER BY Jobs.JobID ASC;";
        }

        private string GetSapphireEgmQueryString()
        {
            return "SELECT " +
                        " HomeRID = Homes.HomeRID," +
                        " JobRID = Jobs.JobRID," +
                        " JobID = Jobs.JobID," +
                        " job_no = LEFT(Jobs.JobID, 12)," +
                        " NewJobNumber = LEFT(Jobs.JobID, 5) + '/' + SUBSTRING(Jobs.JobID, 6, 3) + '/' + SUBSTRING(Jobs.JobID, 9, 4)," +
                        " activity = Acts.ActID," +
                        " JobCstActs_BudgetAmt = JobCstActs.BudgetAmt," +
                        " Vendor_ID = COALESCE(Vnds.VndID, 'YNH00')," +
                        " SapphireObjID = 'JobCstActs'," +
                        " JobCstActRID = JobCstActs.JobCstActRID," +
                        " dbName = Lots.SellerName, " +
                        " JobCstActs.LastUpdated" +
                    " FROM Homes " +
                        " INNER JOIN Jobs ON Jobs.HomeRID = Homes.HomeRID" +
                        " INNER JOIN JobCstActs ON JobCstActs.JobRID = Jobs.JobRID" +
                        " INNER JOIN JobCstHdrs ON JobCstHdrs.JobCstHdrRID = JobCstActs.JobCstHdrRID" +
                        " INNER JOIN Acts ON Acts.ActRID = JobCstActs.ActRID" +
                        " INNER JOIN Lots ON Jobs.LotRID = Lots.LotRID" +
                        " INNER JOIN Communities ON Lots.CommunityRID = Communities.CommunityRID" +
                        " LEFT JOIN JobActs ON JobActs.JobRID = Jobs.JobRID" +
                            " AND JobActs.ActRID = Acts.ActRID" +
                        " LEFT JOIN JobSchActs ON JobSchActs.JobActRID = JobActs.JobActRID" +
                            " AND JobSchActs.ActRID = Acts.ActRID" +
                        " LEFT JOIN Vnds ON Vnds.VndRID = JobSchActs.VndRID" +
                    " WHERE " +
                            " Communities.ShortID != 'TEST'" +
                        " AND JobCstHdrs.JobCstHdrRID =  (" +
                                                            " SELECT TOP 1 JobCstHdrRID" +
                                                            " FROM JobCstHdrs j2" +
                                                            " WHERE j2.JobRID = JobCstHdrs.JobRID" +
                                                                " AND CAST(j2.CreationDate AS DATE) = CAST(Homes.EstApprDate AS DATE)" +
                                                            " ORDER BY j2.CreationDate DESC" +
                                                        " )" +
                        " AND CAST(JobCstHdrs.CreationDate AS DATE) = CAST(Homes.EstApprDate AS DATE)" +
                        " AND Lots.sellerName IS NOT NULL AND Lots.sellerName != ''" +
                    " ORDER BY NewJobNumber ASC, activity ASC;";
        }

        private string GetSapphirePurchaseOrderQueryString()
        {
            return  "SELECT " +
                    " JobPOs.JobRID," +
                    " job_no = LEFT(Jobs.JobID, 12)," +
                    " NewJobNumber = LEFT(Jobs.JobID, 5) + '/' + SUBSTRING(Jobs.JobID, 6, 3) + '/' + SUBSTRING(Jobs.JobID, 9, 4)," +
                    " activity = Acts.ActID," +
                    " Vendor_ID = COALESCE(Vnds.VndID, 'YNH00')," +
                    " JobPOs.Status," +
                    " JobPOs.DateReleased," +
                    " JobPOs.DateCancelled," +
                    " JobPOs.RefNumber," +
                    " JobPOs.AmtPaid," +
                    " JobPOs.AmtSubTotal," +
                    " JobPOs.AmtTax," +
                    " JobPOs.AmtTotal," +
                    " Communities.CommunityID," +
                    " JobPOs.AmtTaxable," +
                    " Jobs.JobID," +
                    " Lots.LotID," +
                    " JobPOs.DateApproved," +
                    " JobPOs.TaxPercentage," +
                    " JobPOs.DatePaid," +
                    " JobSchActs.DateComplByVnd," +
                    " SapphirePONumber = JobPOs.JobPOID," +
                    " SapphireObjID = 'JobPOs', " +
                    " SapphireObjRID = JobPOs.JobPORID," +
                    " JobCstReconcilerID = CONCAT(LEFT(Jobs.JobID,12), '-', Acts.ActID), " +
                    " JobPOReconcilerID = CONCAT(JobPOs.JobPORID, '-', LEFT(Jobs.JobID, 12), '-', Acts.ActID), " +
                    " dbName = Lots.SellerName " +
                " FROM " +
                    " JobPOs" +
                        " INNER JOIN Acts ON Acts.ActRID = JobPOs.ActRID" +
                        " INNER JOIN Jobs ON Jobs.JobRID = JobPOs.JobRID" +
                        " INNER JOIN Lots ON Lots.LotRID = Jobs.LotRID" +
                        " INNER JOIN Communities ON Lots.CommunityRID = Communities.CommunityRID" +
                        " INNER JOIN Vnds ON Vnds.VndRID = JobPOs.VndRID" +
                        " INNER JOIN JobSchActs ON JobSchActs.JobActRID = JobPOs.JobActRID" +
                                " AND JobSchActs.VndRID = JobPOs.VndRID" +
                " WHERE " +
                        " JobPOs.Status IN('Approved', 'Released', 'Hold', 'Cancelled', 'Completed', 'WorkInProgress', 'Paid')" +
                    " AND JobPOs.JobPORID = (" +
                                                " SELECT TOP 1 JobPORID" +
                                                " FROM JobPOs JP2" +
                                                " WHERE JP2.JobRID = JobPOs.JobRID" +
                                                    " AND JP2.ActRID = JobPOs.ActRID" +
                                                " ORDER BY JP2.DateReleased DESC" +
						                    " )" +
                    " AND Communities.ShortID != 'TEST' " +
                    " AND Lots.sellerName IS NOT NULL AND Lots.sellerName != '' " +
                " ORDER BY Jobs.JobID ASC, Acts.ActID ASC;";
        }

        private string GetSapphireVariancePurchaseOrderQueryString()
        {
            return  " SELECT " +
                    "     Lots.LotID, " +
                    "     Acts.ActID, " +
                    "     Vnds.VndID, " +
                    "     JobVPOs.Type, " +
                    "     reason.ID, " +
                    "     JobVPOs.DateAuthorized, " +
                    "     JobVPOs.DateCancelled, " +
                    "     JobVPOs.AmtTotal, " +
                    "     JobVPOs.Status, " +
                    "     JobVPOs.RefNumber, " +
                    "     Communities.CommunityID, " +
                    "     Jobs.JobID, " +
                    "     JobVPOs.DateCompleted, " +
                    "     JobVPOs.DateApproved, " +
                    "     JobVPOs.JobVPOID, " +
                    "     JobVPOs.JobVPORID " +
                    " FROM " +
                    "     Jobs " +
                    "         INNER JOIN Lots ON Jobs.LotRID = Lots.LotRID " +
                    "         INNER JOIN Communities ON Lots.CommunityRID = Communities.CommunityRID " +
                    "         INNER JOIN JobVPOs ON Jobs.JobRID = JobVPOs.JobRID " +
                    "         INNER JOIN ReasonCodes reason ON JobVPOs.ReasonCodeRID = reason.ReasonCodeRID " +
                    "         INNER JOIN Acts ON JobVPOs.ActRID = Acts.ActRID " +
                    "         INNER JOIN Vnds ON JobVPOs.VndRID = Vnds.VndRID " +
                    " WHERE " +
                    "     JobVPOs.Status  IN  ( " +
                    "                             'Authorized', " +
                    "                             'Work Approved', " +
                    "                             'Approved', " +
                    "                             'Rejected', " +
                    "                             'Completed', " +
                    "                             'BeingFixed', " +
                    "                             'Paid', " +
                    "                             'Void', " +
                    "                             'Cancelled', " +
                    "                             'PaymentReceived' " +
                    "                         ) " +
                    " ORDER BY JobVPOs.JobVPOID ASC";
        }
    }
}
