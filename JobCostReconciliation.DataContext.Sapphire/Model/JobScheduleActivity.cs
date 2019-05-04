using System;

namespace JobCostReconciliation.DataContext.Sapphire.Model
{
    public class JobScheduleActivity
    {
        public int JobSchActRID { get; set; }
        public int JobActRID { get; set; }
        public int VndRID { get; set; }

        private DateTime? _dateComplByVnd;
        public DateTime? DateComplByVnd
        {
            get => _dateComplByVnd;
            set => _dateComplByVnd = value == new DateTime(1900, 1, 1) ? null : value;
        }
    }
}
