﻿using System;
using System.Data;
using JobCostReconciliation.Data.Clients;
using JobCostReconciliation.Interfaces.Repositories;

namespace JobCostReconciliation.Data.Repositories
{
    public class PurchaseOrderHeaderRepository : IPurchaseOrderHeaderRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public PurchaseOrderHeaderRepository()
        {
            _companyRepository = new CompanyRepository();
        }
        public PurchaseOrderHeaderRepository(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public DataTable GetPervasiveRecords(string dataObject = "", string jobNumber = "")
        {
            try
            {
                var pervasiveQuery = GetPervasiveQueryString(dataObject, jobNumber);

                if (String.IsNullOrEmpty(pervasiveQuery)) { return null; }

                PervasiveClient pervasiveClient = new PervasiveClient();
                var pervasiveRecords = pervasiveClient.QueryPervasiveADO(pervasiveQuery);
                if (pervasiveRecords is null) { return null; }
                if (pervasiveRecords.Rows.Count == 0)
                {
                    return null;
                }

                return pervasiveRecords;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetPervasiveQueryString(string dataObject, string jobNumber = "")
        {
            if (dataObject.Equals("JobVPOs")) return "exec DI_AuditVPOSync();";
            if ((dataObject.Equals("JobPOs") || dataObject.Equals("JobCstActs")) && String.IsNullOrEmpty(jobNumber)) return "exec DI_AuditPOSync();";

            string company;
            if (dataObject.Equals("JobCstActs_EGM") && !String.IsNullOrEmpty(jobNumber))
            {
                company = _companyRepository.GetCompanyByJob(jobNumber);
                return GetEgmTotalsByJobSql(jobNumber, company);
            }

            if (dataObject.Equals("JobCstActs_EGM")) { return "exec DI_AuditEGMTotals;"; }

            company = _companyRepository.GetCompanyByJob(jobNumber);
            return GetQueryStringWithJobFilter(jobNumber, company);
        }

        private string GetEgmTotalsByJobSql(string jobNumber, string company)
        {
            return "SELECT " +
                      "'" + company + "', " +
                      "NewJobNumber, " +
                      "job_no, " +
                      "SUM(egm_amount) AS egm_amount " +
                    "FROM \"" + company + "\".PO_HEADER" +
                      " WHERE vpo_yes_no = 0 " +
                      " AND job_no = '" + jobNumber + "' " +
                    "GROUP BY NewJobNumber, job_no;";
        }

        private string GetQueryStringWithJobFilter(string jobNumber, string company)
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
                        " AND job_no LIKE '" + jobNumber + "%'" +
                    " ORDER BY job_no ASC, activity ASC, po_no ASC";
        }
    }
}