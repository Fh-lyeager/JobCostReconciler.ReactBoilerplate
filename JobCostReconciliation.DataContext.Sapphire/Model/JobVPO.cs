using System;

namespace JobCostReconciliation.DataContext.Sapphire.Model
{
    public class JobVPO
    {
        public int JobVPORID { get; set; }
        public string Type { get; set; }
        public decimal AmtTotal { get; set; }
        public string Status { get; set; }
        public string RefNumber { get; set; }
        public string JobVPOID { get; set; }
        public DateTime LastUpdated { get; set; }
 
        public int JobRID { get; set; }
        public virtual Job Job { get; set; }

        public int ActRID { get; set; }
        public virtual Activity Activity { get; set; }

        public int VndRID { get; set; }
        public virtual Vendor Vendor { get; set; }    

        public int ReasonCodeRID { get; set; }
        public virtual ReasonCode ReasonCode { get; set; }


        private DateTime? _dateCancelled;
        public DateTime? DateCancelled
        {
            get => _dateCancelled;
            set => _dateCancelled = value == new DateTime(1900, 1, 1) ? null : value;
        }

        private DateTime? _dateAuthorized;
        public DateTime? DateAuthorized
        {
            get => _dateAuthorized;
            set => _dateAuthorized = value == new DateTime(1900, 1, 1) ? null : value;
        }

        private DateTime? _dateApproved;
        public DateTime? DateApproved
        {
            get => _dateApproved;
            set => _dateApproved = value == new DateTime(1900, 1, 1) ? null : value;
        }

        private DateTime? _dateCompleted;
        public DateTime? DateCompleted
        {
            get => _dateCompleted;
            set => _dateCompleted = value == new DateTime(1900, 1, 1) ? null : value;
        }

    }
}
