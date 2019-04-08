using System;

namespace JobCostReconciliation.Model
{
    public class POHeader
    {
        public POHeader()
        {
            DatabaseTable = "PO_Header";
            Database = "";
            KeyFields = "SapphireObjID,SapphireObjRID";       //Primary key for Pervasive tables
            DatabaseLookupField = "NewJobNumber";             //Determines what database to use        
        }

        public string Database { get; set; }
        public string DatabaseTable { get; set; }
        public string KeyFields { get; set; }
        public string DatabaseLookupField { get; set; }
        public int idPO_Queue { get; set; }
        public int po_no { get; set; }
        public string NewJobNumber { get; set; }
        public int activity { get; set; }
        public string vendor_id { get; set; }
        public string po_type { get; set; }
        public DateTime? release_date { get; set; }
        public DateTime? cancelled_date { get; set; }
        public Single payment_amount { get; set; }
        public Single subtotal { get; set; }
        public Single tax { get; set; }
        public Single total { get; set; }
        public Single egm_amount { get; set; }
        public int vpo_yes_no { get; set; }
        public string UserID { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Community { get; set; }
        public string Product { get; set; }
        public string Building { get; set; }
        public string Unit { get; set; }
        public Single taxable_amount { get; set; }
        public string job_no { get; set; }
        public DateTime? eSubmittalDate { get; set; }
        public DateTime? ApprovePaymentDate { get; set; }
        public string Invoice { get; set; }
        public Single TaxRate { get; set; }
        public int eMeasurementPO { get; set; }
        public string SapphirePONumber { get; set; }
        public string SapphireObjID { get; set; }
        public UInt64 SapphireObjRID { get; set; }
    }
}
