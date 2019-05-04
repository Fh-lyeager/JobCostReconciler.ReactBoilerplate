using System;

namespace JobCostReconciliation.DataContext.Sapphire.Model
{
    public class JobPO
    {
        public int JobPORID { get; set; }
        public int JobRID { get; set; }
        public int ActRID { get; set; }
        public int VndRID { get; set; }
        public int JobActRID { get; set; }
        public string Status { get; set; }
        public int RAssnToJobPORID { get; set; }
        public decimal AmtPaid { get; set; }
        public decimal AmtSubTotal { get; set; }
        public decimal AmtTax { get; set; }
        public decimal AmtTotal { get; set; }
        public DateTime LastUpdated { get; set; }

        private DateTime? _dateCancelled;
        public DateTime? DateCancelled
        {
            get => _dateCancelled;
            set => _dateCancelled = value == new DateTime(1900, 1, 1) ? null : value;
        }

        private DateTime? _dateReleased;
        public DateTime? DateReleased
        {
            get => _dateReleased;
            set => _dateReleased = value == new DateTime(1900, 1, 1) ? null : value;
        }

        private DateTime? _datePaid;
        public DateTime? DatePaid
        {
            get => _datePaid;
            set => _datePaid = value == new DateTime(1900, 1, 1) ? null : value;
        }

        private DateTime? _dateApproved;
        public DateTime? DateApproved
        {
            get => _dateApproved;
            set => _dateApproved = value == new DateTime(1900, 1, 1) ? null : value;
        }
        public string RefNumber { get; set; }
        public string CheckID { get; set; }
        public decimal AmtTaxable { get; set; }
        public decimal TaxPercentage { get; set; }
        public string JobPOID { get; set; }

        public virtual Job Job { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual Vendor Vendor { get; set; }

    }
}
