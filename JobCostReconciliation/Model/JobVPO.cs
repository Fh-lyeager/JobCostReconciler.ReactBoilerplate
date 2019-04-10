using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Model
{
    //public class JobVPO
    //{
    //}

    public class JobVPOCompareDataRow
    {
        private string _dbName;
        private string _jobVPOID;
        private double _sapAmtTotal;
        private string _jobVPOStatus;
        private DateTime _dateAuthorized;
        private DateTime _dateApproved;
        private DateTime _approvePaymentDate;

        private DateTime _approvalDate;
        private int _sapphireObjRID;
        private string _sapphirePONumber;
        private string _job_no;
        private int _poHeaderRecordID;

        public string DbName { get => _dbName; set => _dbName = value; }
        public string JobVPOID { get => _jobVPOID; set => _jobVPOID = value; }                          // maps to PO_Header.SapphirePONumber
        public double SapphireAmtTotal { get => _sapAmtTotal; set => _sapAmtTotal = value; }
        public string JobVPOStatus { get => _jobVPOStatus; set => _jobVPOStatus = value; }
        public DateTime DateAuthorized { get => _dateAuthorized; set => _dateAuthorized = value; }
        public DateTime DateApproved { get => _dateApproved; set => _dateApproved = value; }
        public DateTime ApprovePaymentDate { get => _approvePaymentDate; set => _approvePaymentDate = value; }

        public DateTime ApprovalDate { get => _approvalDate; set => _approvalDate = value; }            // Pervasive approval_date
        public int SapphireObjRID { get => _sapphireObjRID; set => _sapphireObjRID = value; }           // Pervasive SapphireObjRID
        public string SapphirePONumber { get => _sapphirePONumber; set => _sapphirePONumber = value; }  // Pervasive SapphirePONumber - map to JobVPOID
        public string Job_no { get => _job_no; set => _job_no = value; }                                // Pervasive job_no
        public int POHeaderRecordID { get => _poHeaderRecordID; set => _poHeaderRecordID = value; }     // Pervasive PO_Header record ID column
    }
}
