using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JobCostReconciliation.Model;
using JobCostReconciliation.Services;

namespace JobCostReconciliation.UI.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class PurchaseOrderQueueController : ControllerBase
    {

        #region NextRun

        // GET: api/PurchaseOrderQueue/LastRuns
        [HttpGet("[action]")]
        public IEnumerable<PurchaseOrderLastRun> LastRuns()
        {
            PurchaseOrderLastRunService purchaseOrderLastRunService = new PurchaseOrderLastRunService();
            List<Model.PurchaseOrderLastRun> list = purchaseOrderLastRunService.List();

            return Enumerable.Range(1, list.Count-1).Select(i => new Model.PurchaseOrderLastRun
            {
                NextRunId = list[i].NextRunId,
                RunComplete = list[i].RunComplete,
                Status = list[i].Status
            });
        }

        // GET: api/PurchaseOrderQueue/LastRun
        [HttpGet("[action]")]
        public string[] LastRun()
        {
            PurchaseOrderProcessorNextRun purchaseOrderProcessorNextRun = new PurchaseOrderProcessorNextRun();
            string[] lastRunArray = new string[]
            {
                purchaseOrderProcessorNextRun.LastRun.ToString("MM/dd/yyyy h:mm:ss")
            };

            return lastRunArray;
        }

        // GET: api/PurchaseOrderQueue/NextRun
        [HttpGet("[action]")]
        public string[] NextRun()
        {
            PurchaseOrderProcessorNextRun purchaseOrderProcessorNextRun = new PurchaseOrderProcessorNextRun();
            string[] nextRunArray = new string[]
            {
                purchaseOrderProcessorNextRun.NextRun.ToString("M/d/yyyy h:mm:ss")
            };

            return nextRunArray;
        }

        #endregion

        // GET: api/PurchaseOrderQueue/ItemsInQueue
        [HttpGet("[action]")]
        public int[] ItemsInQueue()
        {
            PurchaseOrderQueueService purchaseOrderQueueService = new PurchaseOrderQueueService();

            int[] itemsInQueue = new int[]
            {
                purchaseOrderQueueService.GetNewItems().Count
            };

            return itemsInQueue;
        }

        // GET: api/PurchaseOrderQueue/FailedRecords
        [HttpGet("[action]")]
        public int[] FailedRecords()
        {
            PurchaseOrderQueueService purchaseOrderQueueService = new PurchaseOrderQueueService();

            int[] failedRecords = new int[]
            {
                purchaseOrderQueueService.GetErroredItems().Count
            };

            return failedRecords;
        }


        // GET: api/PurchaseOrderQueue
        [HttpGet]
        public IEnumerable<PurchaseOrderQueue> Get()
        {
            PurchaseOrderQueueService purchaseOrderQueueService = new PurchaseOrderQueueService();
            List<PurchaseOrderQueue> queue = purchaseOrderQueueService.GetNewItems();

            return Enumerable.Range(1, queue.Count - 1).Select(i => new PurchaseOrderQueue
            {
                Activity = queue[i].Activity,
                ApprovePaymentDate = queue[i].ApprovePaymentDate,
                CancelledDate = queue[i].CancelledDate,
                ESubmittalDate = queue[i].ESubmittalDate,
                JobNo = queue[i].JobNo,
                JobPOStatus = queue[i].JobPOStatus,
                JobRID = queue[i].JobRID,
                LoadDateTime = queue[i].LoadDateTime,
                PaymentAmount = queue[i].PaymentAmount,
                ReleaseDate = queue[i].ReleaseDate,
                SapphireLastUpdated = queue[i].SapphireLastUpdated,
                SapphireObjRId = queue[i].SapphireObjRId,
                SapphirePONumber = queue[i].SapphirePONumber,
                SiteNumber = queue[i].SiteNumber,
                Subtotal = queue[i].Subtotal,
                Tax = queue[i].Tax,
                TaxableAmount = queue[i].TaxableAmount,
                TaxRate = queue[i].TaxRate,
                Total = queue[i].Total,
                Vendorid = queue[i].Vendorid,
                Status = queue[i].Status
            });
        }




        public class PurchaseOrderProcessorNextRun
        {
            public DateTime LastRun
            {
                get
                {
                    PurchaseOrderLastRunService purchaseOrderLastRunService = new PurchaseOrderLastRunService();
                    return purchaseOrderLastRunService.GetLastRun();
                }
            }
            public DateTime NextRun
            {
                get
                {
                    return LastRun.AddMinutes(30);
                }
            }
        }
    }
}
