using System;
using JobCostReconciliation.Model.Enums;

namespace JobCostReconciliation.Model
{
    public class PurchaseOrderQueue : EntityBase
    {
        public string Activity { get; set; }
        public DateTime? ApprovePaymentDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string CommunityID { get; set; }
        public DateTime? ESubmittalDate { get; set; }
        public string Invoice { get; set; }
        public string JobNo { get; set; }
        public string JobPOStatus { get; set; }
        public int? JobRID { get; set; }
        public DateTime LoadDateTime { get; set; }
        public double? PaymentAmount { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime SapphireLastUpdated { get; set; }
        public int SapphireObjRId { get; set; }
        public string SapphirePONumber { get; set; }
        public string SiteNumber { get; set; }
        public double? Subtotal { get; set; }
        public double? Tax { get; set; }
        public double? TaxableAmount { get; set; }
        public double? TaxRate { get; set; }
        public double? Total { get; set; }
        public string Vendorid { get; set; }
        public QueueItemStatusType Status { get; set; }
    }
}
