using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Services
{
    public class ImportService : IImportService
    {
        private readonly ISapphireRepository _sapphireRepository;
        private readonly IPurchaseOrderHeaderRepository _purchaseOrderHeaderRepository;
        private readonly IQueueRepository _queueRepository;
        private readonly IServiceLog _serviceLog;

        public ImportService(ISapphireRepository SapphireRepository, IPurchaseOrderHeaderRepository purchaseOrderHeaderRepository, IQueueRepository QueueRepository, IServiceLog ServiceLog)
        {
            _sapphireRepository = SapphireRepository;
            _purchaseOrderHeaderRepository = purchaseOrderHeaderRepository;
            _queueRepository = QueueRepository;
            _serviceLog = ServiceLog;
        }

        public void ImportPOSyncData()
        {
            // Always truncate for now..
            bool truncate = true;

            /*
            Console.Write("Truncate table? (Y/N) ");
            if (Console.ReadLine() != "Y") truncate = false;
            */

            _serviceLog.AppendLog(String.Format("Importing PO Sync Data{0}", truncate ? " with truncate" : " with update"));
            ImportPOSyncData(truncate);
        }

        public void ImportPOSyncData(bool truncate = false)
        {
            try
            {
                DataTable pervasiveRecords = _purchaseOrderHeaderRepository.GetPervasiveRecords();
                DataTable pervasiveRecordsToUpdate = new DataTable();

                if (truncate)
                {
                    _queueRepository.TruncateQueueTable();
                }
                else
                {
                    var queueRecords = _queueRepository.GetQueueRecords();

                    // Remove items already existing in Queue from pervasive recordset
                    var idsFromQueue = new HashSet<string>(queueRecords.AsEnumerable()
                        .Select(pa => pa.Field<string>("POHeaderRecordID")));

                    var pervasiveRecordsNotFoundInQueue = pervasiveRecords.AsEnumerable()
                        .Where(pq => !idsFromQueue.Contains(pq.Field<string>("POHeaderRecordID")));

                    var pervasiveRecordsToUpdateInQueue = pervasiveRecords.AsEnumerable()
                        .Where(pq => idsFromQueue.Contains(pq.Field<string>("POHeaderRecordID")));

                    pervasiveRecords = pervasiveRecordsNotFoundInQueue.CopyToDataTable();
                    pervasiveRecordsToUpdate = pervasiveRecordsToUpdateInQueue.CopyToDataTable();
                }

                for (int i = 0; i < pervasiveRecords.Rows.Count; i++)
                {
                    string sql = _queueRepository.CreateInsertSQL(pervasiveRecords.Rows[i]);
                    _queueRepository.InsertAuditPOSyncRecord(sql);

                    if (i % 5000 == 0) Console.WriteLine("{0} records processed.", i);
                }

                _serviceLog.AppendLog(String.Format(" {0} PO_Header records imported to Queue", pervasiveRecords.Rows.Count.ToString()));
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.ToString());
            }
        }

    }
}
