using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Model
{
    public class AuditPOSync
    {
        private int _id;
        private string _dataSource;
        private int _sapphireObjRID;
        private string _sapphireObjID;
        private int _po_no;
        private string _po_type;
        private string _NewJobNumber;
        private string _job_no;
        private int _activity;
        private double _p_egm_amount;
        private string _vendorID;
        private DateTime? _releaseDate;
        private DateTime? _approvalDate;
        private DateTime? _paymentDate;
        private DateTime? _cancelledDate;
        private DateTime? _requiredDate;
        private DateTime? _checkDate;
        private double _paymentAmount;
        private double _subtotal;
        private double _tax;
        private double _total;
        private int _checkNumber;
        private int _vpoYesNo;
        private int _originalPONumber;
        private DateTime? _lastModifiedDate;
        private string _community;
        private double _taxableAmount;
        private DateTime? _poEffectiveDate;
        private DateTime? _eSubmittalDate;
        private DateTime? _approvePaymentDate;
        private DateTime? _eXmitDate;
        private string _sapphirePONumber;
        private int _poHeaderRecordID;

        public int ID { get => _id; set => _id = value; }
        public string DataSource { get => _dataSource; set => _dataSource = value; }
        public int SapphireObjRID { get => _sapphireObjRID; set => _sapphireObjRID = value; }
        public string SapphireObjID { get => _sapphireObjID; set => _sapphireObjID = value; }
        public int po_no { get => _po_no; set => _po_no = value; }
        public string po_type { get => _po_type; set => _po_type = value; }
        public string NewJobNumber { get => _NewJobNumber; set => _NewJobNumber = value; }
        public string job_no { get => _job_no; set => _job_no = value; }
        public int activity { get => _activity; set => _activity = value; }
        public double p_egm_amount { get => _p_egm_amount; set => _p_egm_amount = value; }
        public string Vendor_ID { get => _vendorID; set => _vendorID = value; }
        public DateTime? ReleaseDate { get => _releaseDate; set => _releaseDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public DateTime? ApprovalDate { get => _approvalDate; set => _approvalDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public DateTime? PaymentDate { get => _paymentDate; set => _paymentDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public DateTime? CancelledDate { get => _cancelledDate; set => _cancelledDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public DateTime? RequiredDate { get => _requiredDate; set => _requiredDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public DateTime? CheckDate { get => _checkDate; set => _checkDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public double PaymentAmount { get => _paymentAmount; set => _paymentAmount = value; }
        public double Subtotal { get => _subtotal; set => _subtotal = value; }
        public double Tax { get => _tax; set => _tax = value; }
        public double Total { get => _total; set => _total = value; }
        public int CheckNumber { get => _checkNumber; set => _checkNumber = value; }
        public int VPOYesNo { get => _vpoYesNo; set => _vpoYesNo = value; }
        public int OriginalPONumber { get => _originalPONumber; set => _originalPONumber = value; }
        public DateTime? LastModifiedDate { get => _lastModifiedDate; set => _lastModifiedDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public string Community { get => _community; set => _community = value; }
        public double TaxableAmount { get => _taxableAmount; set => _taxableAmount = value; }
        public DateTime? POEffectiveDate { get => _poEffectiveDate; set => _poEffectiveDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public DateTime? eSubmittalDate { get => _eSubmittalDate; set => _eSubmittalDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public DateTime? ApprovePaymentDate { get => _approvePaymentDate; set => _approvePaymentDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public DateTime? eXmitDate { get => _eXmitDate; set => _eXmitDate = value == new DateTime(1900, 1, 1) ? null : value; }
        public string SapphirePONumber { get => _sapphirePONumber; set => _sapphirePONumber = value; }
        public int POHeaderRecordID { get => _poHeaderRecordID; set => _poHeaderRecordID = value; }
        
    }
}
