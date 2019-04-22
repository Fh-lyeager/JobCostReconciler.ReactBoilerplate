using System;
using JobCostReconciliation.Model.Enums;

namespace JobCostReconciliation.Model
{
    public class WorkflowQueue : EntityBase
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string RequestType { get; set; }
        public string Headers { get; set; }
        public string RequestBody { get; set; }
        public string Url { get; set; }
        public DateTime UtcDateTime { get; set; }
        public string Response { get; set; }
        public QueueItemStatusType Status { get; set; }
    }
}
