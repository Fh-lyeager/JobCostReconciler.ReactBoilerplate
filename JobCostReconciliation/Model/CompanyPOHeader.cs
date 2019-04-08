using System;

namespace JobCostReconciliation.Model
{
    public class CompanyPOHeader
    {
        public string DataSource { get; set; }
        public string SapphireObjID { get; set; }
        public Int64 SapphireObjRID { get; set; }
        public uint po_no { get; set; }
    }
}
